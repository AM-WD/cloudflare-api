using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using AMWD.Net.Api.Cloudflare.Dns.Internals;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.Incoming
{
	[TestClass]
	public class UpdateSecondaryZoneConfigurationTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string PeerId = "23ff594956f20c2a721606e94745a8aa";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<IncomingZoneConfiguration> _response;
		private List<(string RequestPath, InternalSecondaryZoneConfigurationRequest Request)> _callbacks;
		private SecondaryZoneConfigurationRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<IncomingZoneConfiguration>
			{
				Success = true,
				Messages = [new ResponseInfo(1000, "Message 1")],
				Errors = [new ResponseInfo(1000, "Error 1")],
				Result = new IncomingZoneConfiguration
				{
					Id = "269d8f4853475ca241c4e730be286b20",
					Name = "www.example.com",
					AutoRefreshSeconds = 86400,
					Peers = [PeerId]
				}
			};

			_request = new SecondaryZoneConfigurationRequest(ZoneId, "www.example.com")
			{
				AutoRefreshSeconds = 86400,
				Peers = [PeerId]
			};
		}

		[TestMethod]
		public async Task ShouldUpdateSecondaryZoneConfiguration()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.UpdateSecondaryZoneConfiguration(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/secondary_dns/incoming", requestPath);
			Assert.IsNotNull(request);

			Assert.AreEqual(_request.Name, request.Name);
			Assert.AreEqual(_request.AutoRefreshSeconds, request.AutoRefreshSeconds);
			CollectionAssert.AreEqual(_request.Peers.ToList(), request.Peers.ToList());

			_clientMock.Verify(m => m.PutAsync<IncomingZoneConfiguration, InternalSecondaryZoneConfigurationRequest>(
				$"/zones/{ZoneId}/secondary_dns/incoming",
				It.IsAny<InternalSecondaryZoneConfigurationRequest>(),
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
				await client.UpdateSecondaryZoneConfiguration(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForAutoRefreshSeconds()
		{
			// Arrange
			_request.AutoRefreshSeconds = -1;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateSecondaryZoneConfiguration(_request, TestContext.CancellationToken);
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
				await client.UpdateSecondaryZoneConfiguration(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		public async Task ShouldThrowArgumentExceptionForInvalidPeerId()
		{
			// Arrange
			_request.Peers = ["invalid-peer-id"]; // invalid format
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentException>(async () =>
			{
				await client.UpdateSecondaryZoneConfiguration(_request, TestContext.CancellationToken);
			});
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PutAsync<IncomingZoneConfiguration, InternalSecondaryZoneConfigurationRequest>(
					It.IsAny<string>(),
					It.IsAny<InternalSecondaryZoneConfigurationRequest>(),
					It.IsAny<CancellationToken>()))
				.Callback<string, InternalSecondaryZoneConfigurationRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
