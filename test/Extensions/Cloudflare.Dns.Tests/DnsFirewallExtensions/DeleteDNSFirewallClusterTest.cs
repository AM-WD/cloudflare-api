using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsFirewallExtensions
{
	[TestClass]
	public class DeleteDNSFirewallClusterTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string ClusterId = "023e105f4ecef8ad9ca31a8372d0c355";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<Identifier> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<Identifier>
			{
				Success = true,
				Messages = [new ResponseInfo(1000, "Message 1")],
				Errors = [new ResponseInfo(1000, "Error 1")],
				Result = new Identifier { Id = ClusterId }
			};
		}

		[TestMethod]
		public async Task ShouldDeleteDnsFirewallCluster()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.DeleteDNSFirewallCluster(AccountId, ClusterId, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);
			Assert.AreEqual(ClusterId, response.Result.Id);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_firewall/{ClusterId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.DeleteAsync<Identifier>($"/accounts/{AccountId}/dns_firewall/{ClusterId}", null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.DeleteAsync<Identifier>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
