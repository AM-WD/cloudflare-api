using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsFirewallExtensions.ReverseDns
{
	[TestClass]
	public class ShowDNSFirewallClusterReverseDNSTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string ClusterId = "023e105f4ecef8ad9ca31a8372d0c355";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<ReverseDnsResponse> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<ReverseDnsResponse>
			{
				Success = true,
				Messages = [new ResponseInfo(1000, "Message 1")],
				Errors = [new ResponseInfo(1000, "Error 1")],
				Result = new ReverseDnsResponse
				{
					ReverseDNS = new Dictionary<string, string>
					{
						["192.0.2.1"] = "ptr.example.com"
					}
				}
			};
		}

		[TestMethod]
		public async Task ShouldShowDnsFirewallClusterReverseDns()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ShowDNSFirewallClusterReverseDNS(AccountId, ClusterId, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);
			Assert.IsTrue(response.Result.ReverseDNS?.ContainsKey("192.0.2.1") ?? false);
			Assert.AreEqual("ptr.example.com", response.Result.ReverseDNS!["192.0.2.1"]);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_firewall/{ClusterId}/reverse_dns", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<ReverseDnsResponse>($"/accounts/{AccountId}/dns_firewall/{ClusterId}/reverse_dns", null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<ReverseDnsResponse>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
