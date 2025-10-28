using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.Peers
{
	[TestClass]
	public class PeerDetailsTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string PeerId = "23ff594956f20c2a721606e94745a8aa";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<Peer> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<Peer>
			{
				Success = true,
				Messages = [new ResponseInfo(1000, "Message 1")],
				Errors = [],
				Result = new Peer(PeerId, "peer-a")
				{
					IpAddress = "192.0.2.1",
					IXFREnabled = true,
					Port = 5353,
					TSIGId = "tsig-1"
				}
			};
		}

		[TestMethod]
		public async Task ShouldGetPeerDetails()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.PeerDetails(AccountId, PeerId, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);
			Assert.AreEqual(PeerId, response.Result.Id);
			Assert.AreEqual("peer-a", response.Result.Name);
			Assert.AreEqual("192.0.2.1", response.Result.IpAddress);
			Assert.IsTrue(response.Result.IXFREnabled ?? false);
			Assert.AreEqual(5353, response.Result.Port);
			Assert.AreEqual("tsig-1", response.Result.TSIGId);

			Assert.HasCount(1, _callbacks);
			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/secondary_dns/peers/{PeerId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<Peer>($"/accounts/{AccountId}/secondary_dns/peers/{PeerId}", null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<Peer>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
