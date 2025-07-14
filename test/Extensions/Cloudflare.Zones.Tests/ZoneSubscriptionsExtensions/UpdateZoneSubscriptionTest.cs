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
	public class UpdateZoneSubscriptionTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<Subscription> _response;

		private List<(string RequestPath, InternalUpdateZoneSubscriptionRequest Request)> _callbacks;

		private UpdateZoneSubscriptionRequest _request;

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

			_request = new UpdateZoneSubscriptionRequest(ZoneId)
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
		public async Task ShouldUpdateZoneSubscription()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.UpdateZoneSubscription(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/subscription", callback.RequestPath);
			Assert.IsNotNull(callback.Request);

			Assert.AreEqual(_request.Frequency, callback.Request.Frequency);
			Assert.AreEqual(_request.RatePlan, callback.Request.RatePlan);

			_clientMock.Verify(m => m.PutAsync<Subscription, InternalUpdateZoneSubscriptionRequest>($"/zones/{ZoneId}/subscription", It.IsAny<InternalUpdateZoneSubscriptionRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PutAsync<Subscription, InternalUpdateZoneSubscriptionRequest>(It.IsAny<string>(), It.IsAny<InternalUpdateZoneSubscriptionRequest>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalUpdateZoneSubscriptionRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
