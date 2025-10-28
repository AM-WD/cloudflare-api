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
	public class CreatePeerTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<Peer> _response;
		private List<(string RequestPath, InternalCreatePeerRequest Request)> _callbacks;
		private CreatePeerRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = new List<(string, InternalCreatePeerRequest)>();

			_response = new CloudflareResponse<Peer>
			{
				Success = true,
				Messages = new List<ResponseInfo> { new ResponseInfo(1000, "Message 1") },
				Errors = new List<ResponseInfo>(),
				Result = new Peer("023e105f4ecef8ad9ca31a8372d0c351", "peer-a")
				{
					IpAddress = "192.0.2.1",
					IXFREnabled = true,
					Port = 5353,
					TSIGId = "tsig-1"
				}
			};

			_request = new CreatePeerRequest(AccountId, "peer-a");
		}

		[TestMethod]
		public async Task ShouldCreatePeer()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.CreatePeer(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, req) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/secondary_dns/peers", requestPath);
			Assert.IsNotNull(req);
			Assert.AreEqual(_request.Name, req.Name);

			_clientMock.Verify(m => m.PostAsync<Peer, InternalCreatePeerRequest>(
				$"/accounts/{AccountId}/secondary_dns/peers",
				It.IsAny<InternalCreatePeerRequest>(),
				null,
				TestContext.CancellationToken), Times.Once);

			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public async Task ShouldThrowArgumentNullExceptionForName(string name)
		{
			// Arrange
			_request.Name = name;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () =>
			{
				await client.CreatePeer(_request, TestContext.CancellationToken);
			});
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<Peer, InternalCreatePeerRequest>(It.IsAny<string>(), It.IsAny<InternalCreatePeerRequest>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalCreatePeerRequest, IQueryParameterFilter, CancellationToken>((requestPath, request, _, __) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
