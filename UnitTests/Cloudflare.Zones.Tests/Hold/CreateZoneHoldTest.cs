using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using Moq;

namespace Cloudflare.Zones.Tests.Hold
{
	[TestClass]
	public class CreateZoneHoldTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";
		private readonly DateTime DateTime = new(2024, 10, 10, 20, 30, 40, 0, DateTimeKind.Utc);

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<ZoneHold> _response;

		private List<(string RequestPath, object Request, IQueryParameterFilter QueryFilter)> _callbacks;

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
					HoldAfter = DateTime,
					IncludeSubdomains = "true"
				}
			};
		}

		[TestMethod]
		public async Task ShouldCreateZoneHold()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.CreateZoneHold(ZoneId, includeSubdomains: false);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"zones/{ZoneId}/hold", callback.RequestPath);
			Assert.IsNotNull(callback.QueryFilter);

			Assert.IsInstanceOfType<CreateZoneHoldFilter>(callback.QueryFilter);
			Assert.IsFalse(((CreateZoneHoldFilter)callback.QueryFilter).IncludeSubdomains);

			_clientMock.Verify(m => m.PostAsync<ZoneHold, object>($"zones/{ZoneId}/hold", null, It.IsAny<CreateZoneHoldFilter>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldCreateZoneHoldWithSubdomains()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.CreateZoneHold(ZoneId, includeSubdomains: true);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"zones/{ZoneId}/hold", callback.RequestPath);
			Assert.IsNotNull(callback.QueryFilter);

			Assert.IsInstanceOfType<CreateZoneHoldFilter>(callback.QueryFilter);
			Assert.IsTrue(((CreateZoneHoldFilter)callback.QueryFilter).IncludeSubdomains);

			_clientMock.Verify(m => m.PostAsync<ZoneHold, object>($"zones/{ZoneId}/hold", null, It.IsAny<CreateZoneHoldFilter>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public void ShouldReturnEmptyDictionary()
		{
			// Arrange
			var filter = new CreateZoneHoldFilter();

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
			var filter = new CreateZoneHoldFilter { IncludeSubdomains = true };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(1, dict.Count);
			Assert.IsTrue(dict.ContainsKey("include_subdomains"));
			Assert.AreEqual("true", dict["include_subdomains"]);
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
