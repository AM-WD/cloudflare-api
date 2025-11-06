using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsFirewallExtensions
{
	[TestClass]
	public class DNSFirewallClusterDetailsTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string ClusterId = "023e105f4ecef8ad9ca31a8372d0c355";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<DnsFirewallCluster> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<DnsFirewallCluster>
			{
				Success = true,
				Messages =
				[
					new ResponseInfo(1000, "Message 1")
				],
				Errors =
				[
					new ResponseInfo(1000, "Error 1")
				],
				Result = new DnsFirewallCluster
				{
					Id = ClusterId,
					Name = "example-cluster",
					ModifiedOn = DateTime.Parse("2025-01-01T12:00:00Z"),
					DeprecateAnyRequests = true,
					EcsFallback = false,
					MaximumCacheTtl = 3600,
					MinimumCacheTtl = 60,
					NegativeCacheTtl = 30,
					RateLimit = 1000,
					Retries = 2,
					DnsFirewallIps = ["10.0.0.1"],
					UpstreamIps = ["192.0.2.1"],
					AttackMitigation = new AttackMitigation { Enabled = true, OnlyWhenUpstreamUnhealthy = false }
				}
			};
		}

		[TestMethod]
		public async Task ShouldGetDnsFirewallClusterDetails()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.DNSFirewallClusterDetails(AccountId, ClusterId, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);
			Assert.AreEqual(ClusterId, response.Result.Id);
			Assert.AreEqual("example-cluster", response.Result.Name);
			Assert.IsTrue(response.Result.DeprecateAnyRequests ?? false);
			Assert.AreEqual(3600, response.Result.MaximumCacheTtl);
			Assert.AreEqual(60, response.Result.MinimumCacheTtl);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_firewall/{ClusterId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<DnsFirewallCluster>($"/accounts/{AccountId}/dns_firewall/{ClusterId}", null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<DnsFirewallCluster>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
