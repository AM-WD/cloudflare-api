using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.Peers
{
	[TestClass]
	public class ListPeersTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<IReadOnlyCollection<Peer>> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<IReadOnlyCollection<Peer>>
			{
				Success = true,
				Messages = [new ResponseInfo(1000, "Message 1")],
				Errors = [],
				Result =
				[
					new Peer("023e105f4ecef8ad9ca31a8372d0c351", "peer-a")
					{
						IpAddress = "192.0.2.1",
						IXFREnabled = true,
						Port = 5353,
						TSIGId = "tsig-1"
					},
					new Peer("023e105f4ecef8ad9ca31a8372d0c352", "peer-b")
					{
						IpAddress = "2001:db8::1",
						IXFREnabled = false,
						Port = 53,
						TSIGId = null
					}
				]
			};
		}

		[TestMethod]
		public async Task ShouldListPeers()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ListPeers(AccountId, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);
			Assert.HasCount(2, response.Result);

			var peers = response.Result.ToList();
			Assert.AreEqual("peer-a", peers[0].Name);
			Assert.AreEqual("192.0.2.1", peers[0].IpAddress);
			Assert.IsTrue(peers[0].IXFREnabled ?? false);
			Assert.AreEqual(5353, peers[0].Port);
			Assert.AreEqual("tsig-1", peers[0].TSIGId);

			Assert.AreEqual("peer-b", peers[1].Name);
			Assert.AreEqual("2001:db8::1", peers[1].IpAddress);
			Assert.IsFalse(peers[1].IXFREnabled ?? false);
			Assert.AreEqual(53, peers[1].Port);
			Assert.IsNull(peers[1].TSIGId);

			Assert.HasCount(1, _callbacks);
			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/secondary_dns/peers", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<IReadOnlyCollection<Peer>>($"/accounts/{AccountId}/secondary_dns/peers", null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<IReadOnlyCollection<Peer>>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
