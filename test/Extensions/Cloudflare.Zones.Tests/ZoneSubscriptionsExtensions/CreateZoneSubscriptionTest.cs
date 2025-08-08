using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using AMWD.Net.Api.Cloudflare.Zones.Internals;
using Moq;

namespace Cloudflare.Zones.Tests.ZoneSubscriptionsExtensions
{
	[TestClass]
	public class CreateZoneSubscriptionTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<Subscription> _response;

		private List<(string RequestPath, InternalCreateZoneSubscriptionRequest Request)> _callbacks;

		private CreateZoneSubscriptionRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<Subscription>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = new Subscription()
			};

			_request = new CreateZoneSubscriptionRequest(ZoneId)
			{
				Frequency = Frequency.Quarterly,
				RatePlan = new RatePlan
				{
					Id = RatePlanId.Business,
					PublicName = "Business Plan"
				}
			};
		}

		[TestMethod]
		public async Task ShouldCreateZoneSubscription()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.CreateZoneSubscription(_request, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/subscription", requestPath);
			Assert.IsNotNull(request);

			Assert.AreEqual(_request.Frequency, request.Frequency);
			Assert.AreEqual(_request.RatePlan, request.RatePlan);

			_clientMock.Verify(m => m.PostAsync<Subscription, InternalCreateZoneSubscriptionRequest>($"/zones/{ZoneId}/subscription", It.IsAny<InternalCreateZoneSubscriptionRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<Subscription, InternalCreateZoneSubscriptionRequest>(It.IsAny<string>(), It.IsAny<InternalCreateZoneSubscriptionRequest>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalCreateZoneSubscriptionRequest, IQueryParameterFilter, CancellationToken>((requestPath, request, _, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
