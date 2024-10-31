using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using AMWD.Net.Api.Cloudflare.Zones.Internals.Filters;
using Moq;

namespace Cloudflare.Zones.Tests.Hold
{
	[TestClass]
	public class DeleteZoneHoldTest
	{
		private readonly DateTime _date = new(2024, 10, 10, 20, 30, 40, 0, DateTimeKind.Utc);
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<ZoneHold> _response;

		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<ZoneHold>
			{
				Success = true,
				Messages = [
					new ResponseInfo
					{
						Code = 1000,
						Message = "Message 1",
					}
				],
				Errors = [
					new ResponseInfo
					{
						Code = 1000,
						Message = "Error 1",
					}
				],
				Result = new ZoneHold
				{
					Hold = true,
					HoldAfter = _date,
					IncludeSubdomains = "true"
				}
			};
		}

		[TestMethod]
		public async Task ShouldDeleteZoneHold()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.DeleteZoneHold(ZoneId);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"zones/{ZoneId}/hold", callback.RequestPath);
			Assert.IsNotNull(callback.QueryFilter);

			Assert.IsInstanceOfType<DeleteZoneHoldFilter>(callback.QueryFilter);
			Assert.IsNull(((DeleteZoneHoldFilter)callback.QueryFilter).HoldAfter);

			_clientMock.Verify(m => m.DeleteAsync<ZoneHold>($"zones/{ZoneId}/hold", It.IsAny<DeleteZoneHoldFilter>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldDeleteZoneHoldTemporarily()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.DeleteZoneHold(ZoneId, holdAfter: _date);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"zones/{ZoneId}/hold", callback.RequestPath);
			Assert.IsNotNull(callback.QueryFilter);

			Assert.IsInstanceOfType<DeleteZoneHoldFilter>(callback.QueryFilter);
			Assert.AreEqual(_date, ((DeleteZoneHoldFilter)callback.QueryFilter).HoldAfter);

			_clientMock.Verify(m => m.DeleteAsync<ZoneHold>($"zones/{ZoneId}/hold", It.IsAny<DeleteZoneHoldFilter>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public void ShouldReturnEmptyDictionary()
		{
			// Arrange
			var filter = new DeleteZoneHoldFilter();

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		public void ShouldReturnQueryParameter()
		{
			// Arrange
			var filter = new DeleteZoneHoldFilter { HoldAfter = _date };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(1, dict.Count);
			Assert.IsTrue(dict.ContainsKey("hold_after"));
			Assert.AreEqual("2024-10-10T20:30:40Z", dict["hold_after"]);
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
