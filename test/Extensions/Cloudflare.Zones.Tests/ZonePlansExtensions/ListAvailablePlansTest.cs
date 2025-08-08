using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using Moq;

namespace Cloudflare.Zones.Tests.ZonePlansExtensions
{
	[TestClass]
	public class ListAvailablePlansTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<IReadOnlyCollection<AvailableRatePlan>> _response;

		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<IReadOnlyCollection<AvailableRatePlan>>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = []
			};
		}

		[TestMethod]
		public async Task ShouldReturnAvailablePlan()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ListAvailablePlans(ZoneId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/available_plans", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<IReadOnlyCollection<AvailableRatePlan>>($"/zones/{ZoneId}/available_plans", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<IReadOnlyCollection<AvailableRatePlan>>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
