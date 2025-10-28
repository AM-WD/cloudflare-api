using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using AMWD.Net.Api.Cloudflare.Dns.Internals;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.Peers
{
	[TestClass]
	public class UpdatePeerTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string PeerId = "23ff594956f20c2a721606e94745a8aa";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<Peer> _response;
		private List<(string RequestPath, InternalUpdatePeerRequest Request)> _callbacks;
		private UpdatePeerRequest _request;

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

			_request = new UpdatePeerRequest(AccountId, PeerId, "peer-a")
			{
				IpAddress = "192.0.2.1",
				IXFREnable = true,
				Port = 5353,
				TSIGId = "tsig-1"
			};
		}

		[TestMethod]
		public async Task ShouldUpdatePeer()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.UpdatePeer(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);
			var (requestPath, req) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/secondary_dns/peers/{PeerId}", requestPath);

			Assert.IsNotNull(req);
			Assert.AreEqual(_request.Name, req.Name);
			Assert.AreEqual(_request.IpAddress, req.Ip);
			Assert.AreEqual(_request.IXFREnable, req.IxfrEnable);
			Assert.AreEqual(_request.Port, req.Port);
			Assert.AreEqual(_request.TSIGId, req.TSigId);

			_clientMock.Verify(m => m.PutAsync<Peer, InternalUpdatePeerRequest>(
				$"/accounts/{AccountId}/secondary_dns/peers/{PeerId}",
				It.IsAny<InternalUpdatePeerRequest>(),
				TestContext.CancellationToken), Times.Once);

			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public async Task ShouldThrowArgumentNullExceptionForName(string name)
		{
			_request.Name = name;
			var client = GetClient();

			await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () =>
			{
				await client.UpdatePeer(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForPortLessThanZero()
		{
			_request.Port = -1;
			var client = GetClient();

			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdatePeer(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForPortGreaterThan65535()
		{
			_request.Port = 70000;
			var client = GetClient();

			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdatePeer(_request, TestContext.CancellationToken);
			});
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PutAsync<Peer, InternalUpdatePeerRequest>(It.IsAny<string>(), It.IsAny<InternalUpdatePeerRequest>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalUpdatePeerRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
