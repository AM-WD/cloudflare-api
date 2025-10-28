using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.Outgoing
{
	[TestClass]
	public class DisableOutgoingZoneTransfersTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<string> _response;
		private List<(string RequestPath, object Request)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<string>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = "Disabled"
			};
		}

		[TestMethod]
		public async Task ShouldDisableOutgoingZoneTransfers()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.DisableOutgoingZoneTransfers(ZoneId, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/secondary_dns/outgoing/disable", requestPath);
			Assert.IsNull(request);

			_clientMock.Verify(m => m.PostAsync<string, object>(
				$"/zones/{ZoneId}/secondary_dns/outgoing/disable",
				null,
				null,
				TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<string, object>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, object, IQueryParameterFilter, CancellationToken>((requestPath, request, _, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
