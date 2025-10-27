using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using AMWD.Net.Api.Cloudflare.Zones.Internals;
using Moq;

namespace Cloudflare.Zones.Tests.ZoneHoldsExtensions
{
	[TestClass]
	public class UpdateZoneHoldTest
	{
		public TestContext TestContext { get; set; }

		private readonly DateTime _date = new(2024, 10, 10, 20, 30, 40, 0, DateTimeKind.Utc);
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<ZoneHold> _response;

		private List<(string RequestPath, InternalUpdateZoneHoldRequest Request)> _callbacks;

		private UpdateZoneHoldRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<ZoneHold>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = new ZoneHold
				{
					Hold = true,
					HoldAfter = _date,
					IncludeSubdomains = false
				}
			};

			_request = new UpdateZoneHoldRequest(ZoneId)
			{
				HoldAfter = _date,
				IncludeSubdomains = true
			};
		}

		[TestMethod]
		public async Task ShouldUpdateZoneHold()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.UpdateZoneHold(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/hold", requestPath);

			Assert.IsNotNull(request);
			Assert.AreEqual(_date, request.HoldAfter);
			Assert.IsTrue(request.IncludeSubdomains);
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PatchAsync<ZoneHold, InternalUpdateZoneHoldRequest>(It.IsAny<string>(), It.IsAny<InternalUpdateZoneHoldRequest>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalUpdateZoneHoldRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
