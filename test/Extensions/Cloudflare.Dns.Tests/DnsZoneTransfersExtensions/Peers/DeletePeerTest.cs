using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.Peers
{
	[TestClass]
	public class DeletePeerTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string PeerId = "23ff594956f20c2a721606e94745a8aa";

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
				Errors = [],
				Result = new Identifier
				{
					Id = PeerId
				}
			};
		}

		[TestMethod]
		public async Task ShouldDeletePeer()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.DeletePeer(AccountId, PeerId, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);
			Assert.AreEqual(PeerId, response.Result.Id);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/secondary_dns/peers/{PeerId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.DeleteAsync<Identifier>($"/accounts/{AccountId}/secondary_dns/peers/{PeerId}", null, TestContext.CancellationToken), Times.Once);
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
