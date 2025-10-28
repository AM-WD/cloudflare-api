using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using AMWD.Net.Api.Cloudflare.Dns.Internals;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.Outgoing
{
	[TestClass]
	public class UpdatePrimaryZoneConfigurationTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string PeerId = "23ff594956f20c2a721606e94745a8aa";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<OutgoingZoneConfiguration> _response;
		private List<(string RequestPath, InternalPrimaryZoneConfigurationRequest Request)> _callbacks;
		private PrimaryZoneConfigurationRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<OutgoingZoneConfiguration>
			{
				Success = true,
				Messages = [new ResponseInfo(1000, "Message 1")],
				Errors = [],
				Result = new OutgoingZoneConfiguration
				{
					Id = "269d8f4853475ca241c4e730be286b20",
					Name = "www.example.com",
					Peers = [PeerId],
					SoaSerial = 12345
				}
			};

			_request = new PrimaryZoneConfigurationRequest(ZoneId, "www.example.com")
			{
				Peers = [PeerId]
			};
		}

		[TestMethod]
		public async Task ShouldUpdatePrimaryZoneConfiguration()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.UpdatePrimaryZoneConfiguration(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, req) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/secondary_dns/outgoing", requestPath);
			Assert.IsNotNull(req);

			Assert.AreEqual(_request.Name, req.Name);
			CollectionAssert.AreEqual(_request.Peers.ToList(), req.Peers.ToList());

			_clientMock.Verify(m => m.PutAsync<OutgoingZoneConfiguration, InternalPrimaryZoneConfigurationRequest>(
				$"/zones/{ZoneId}/secondary_dns/outgoing",
				It.IsAny<InternalPrimaryZoneConfigurationRequest>(),
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
				await client.UpdatePrimaryZoneConfiguration(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForEmptyPeers()
		{
			// Arrange
			_request.Peers = [];
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdatePrimaryZoneConfiguration(_request, TestContext.CancellationToken);
			});
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PutAsync<OutgoingZoneConfiguration, InternalPrimaryZoneConfigurationRequest>(
					It.IsAny<string>(),
					It.IsAny<InternalPrimaryZoneConfigurationRequest>(),
					It.IsAny<CancellationToken>()))
				.Callback<string, InternalPrimaryZoneConfigurationRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
