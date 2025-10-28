using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.Outgoing
{
	[TestClass]
	public class PrimaryZoneConfigurationDetailsTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string PeerId = "23ff594956f20c2a721606e94745a8aa";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<OutgoingZoneConfiguration> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

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
		}

		[TestMethod]
		public async Task ShouldGetPrimaryZoneConfiguration()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.PrimaryZoneConfigurationDetails(ZoneId, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);
			Assert.AreEqual("269d8f4853475ca241c4e730be286b20", response.Result.Id);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/secondary_dns/outgoing", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<OutgoingZoneConfiguration>($"/zones/{ZoneId}/secondary_dns/outgoing", null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<OutgoingZoneConfiguration>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
