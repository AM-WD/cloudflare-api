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
	public class CreateZoneHoldTest
	{
		private readonly DateTime _date = new(2025, 10, 10, 20, 30, 40, 0, DateTimeKind.Utc);
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<ZoneHold> _response;

		private List<(string RequestPath, object Request, IQueryParameterFilter QueryFilter)> _callbacks;

		private CreateZoneHoldRequest _request;

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

			_request = new CreateZoneHoldRequest(ZoneId);
		}

		[TestMethod]
		public async Task ShouldCreateZoneHold()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.CreateZoneHold(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/hold", callback.RequestPath);
			Assert.IsNotNull(callback.QueryFilter);

			Assert.IsInstanceOfType<InternalCreateZoneHoldFilter>(callback.QueryFilter);
			Assert.IsNull(((InternalCreateZoneHoldFilter)callback.QueryFilter).IncludeSubdomains);

			_clientMock.Verify(m => m.PostAsync<ZoneHold, object>($"/zones/{ZoneId}/hold", null, It.IsAny<InternalCreateZoneHoldFilter>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldCreateZoneHoldWithSubdomains()
		{
			// Arrange
			_request.IncludeSubdomains = true;
			var client = GetClient();

			// Act
			var response = await client.CreateZoneHold(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/hold", callback.RequestPath);
			Assert.IsNotNull(callback.QueryFilter);

			Assert.IsInstanceOfType<InternalCreateZoneHoldFilter>(callback.QueryFilter);
			Assert.IsTrue(((InternalCreateZoneHoldFilter)callback.QueryFilter).IncludeSubdomains);

			_clientMock.Verify(m => m.PostAsync<ZoneHold, object>($"/zones/{ZoneId}/hold", null, It.IsAny<InternalCreateZoneHoldFilter>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public void ShouldReturnEmptyDictionary()
		{
			// Arrange
			var filter = new InternalCreateZoneHoldFilter();

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[DataTestMethod]
		[DataRow(true)]
		[DataRow(false)]
		public void ShouldReturnQueryParameter(bool includeSubdomains)
		{
			// Arrange
			var filter = new InternalCreateZoneHoldFilter { IncludeSubdomains = includeSubdomains };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(1, dict.Count);
			Assert.IsTrue(dict.ContainsKey("include_subdomains"));
			Assert.AreEqual(includeSubdomains.ToString().ToLower(), dict["include_subdomains"]);
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<ZoneHold, object>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, object, IQueryParameterFilter, CancellationToken>((requestPath, request, queryFilter, _) => _callbacks.Add((requestPath, request, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
