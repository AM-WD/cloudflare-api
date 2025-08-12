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
	public class RemoveZoneHoldTest
	{
		public TestContext TestContext { get; set; }

		private readonly DateTime _date = new(2025, 10, 10, 20, 30, 40, 0, DateTimeKind.Utc);
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<ZoneHold> _response;

		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		private RemoveZoneHoldRequest _request;

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
					IncludeSubdomains = true
				}
			};

			_request = new RemoveZoneHoldRequest(ZoneId);
		}

		[TestMethod]
		public async Task ShouldRemoveZoneHold()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.RemoveZoneHold(_request, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/hold", requestPath);
			Assert.IsNotNull(queryFilter);

			Assert.IsInstanceOfType<InternalRemoveZoneHoldFilter>(queryFilter);
			Assert.IsNull(((InternalRemoveZoneHoldFilter)queryFilter).HoldAfter);

			_clientMock.Verify(m => m.DeleteAsync<ZoneHold>($"/zones/{ZoneId}/hold", It.IsAny<InternalRemoveZoneHoldFilter>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldRemoveZoneHoldTemporarily()
		{
			// Arrange
			_request.HoldAfter = _date;
			var client = GetClient();

			// Act
			var response = await client.RemoveZoneHold(_request, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/hold", requestPath);
			Assert.IsNotNull(queryFilter);

			Assert.IsInstanceOfType<InternalRemoveZoneHoldFilter>(queryFilter);
			Assert.AreEqual(_date, ((InternalRemoveZoneHoldFilter)queryFilter).HoldAfter);

			_clientMock.Verify(m => m.DeleteAsync<ZoneHold>($"/zones/{ZoneId}/hold", It.IsAny<InternalRemoveZoneHoldFilter>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public void ShouldReturnEmptyDictionary()
		{
			// Arrange
			var filter = new InternalRemoveZoneHoldFilter();

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		public void ShouldReturnQueryParameter()
		{
			// Arrange
			var filter = new InternalRemoveZoneHoldFilter { HoldAfter = _date };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.HasCount(1, dict);
			Assert.IsTrue(dict.ContainsKey("hold_after"));
			Assert.AreEqual("2025-10-10T20:30:40Z", dict["hold_after"]);
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.DeleteAsync<ZoneHold>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
