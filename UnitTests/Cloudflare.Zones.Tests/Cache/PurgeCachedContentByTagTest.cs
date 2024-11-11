using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using AMWD.Net.Api.Cloudflare.Zones.Internals.Requests;
using Moq;

namespace Cloudflare.Zones.Tests.Cache
{
	[TestClass]
	public class PurgeCachedContentByTagTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";
		private readonly DateTime DateTime = new(2024, 10, 10, 20, 30, 40, 0, DateTimeKind.Utc);

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<IdResponse> _response;

		private List<(string RequestPath, InternalPurgeCacheRequest Request, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<IdResponse>
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
				Result = new IdResponse
				{
					Id = ZoneId,
				}
			};
		}

		[TestMethod]
		public async Task ShouldPurgeCachedContent()
		{
			// Arrange
			var list = new List<string>
			{
				"Tag-1",
				"",
				"Tag-2",
			};

			var client = GetClient();

			// Act
			var response = await client.PurgeCachedContentByTag(ZoneId, list);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"zones/{ZoneId}/purge_cache", callback.RequestPath);

			Assert.IsNotNull(callback.Request);
			Assert.IsNull(callback.Request.PurgeEverything);
			Assert.IsNull(callback.Request.Urls);
			Assert.IsNull(callback.Request.UrlsWithHeaders);
			Assert.IsNotNull(callback.Request.Tags);
			Assert.AreEqual(2, callback.Request.Tags.Count);
			Assert.AreEqual("Tag-1", callback.Request.Tags[0]);
			Assert.AreEqual("Tag-2", callback.Request.Tags[1]);
			Assert.IsNull(callback.Request.Hostnames);
			Assert.IsNull(callback.Request.Prefixes);

			Assert.IsNull(callback.QueryFilter);

			_clientMock.Verify(m => m.PostAsync<IdResponse, InternalPurgeCacheRequest>($"zones/{ZoneId}/purge_cache", It.IsAny<InternalPurgeCacheRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumenNullExceptionIfListIsNull()
		{
			// Arrange
			var client = GetClient();

			// Act
			await client.PurgeCachedContentByTag(ZoneId, null);

			// Assert - ArgumentNullException
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<IdResponse, InternalPurgeCacheRequest>(It.IsAny<string>(), It.IsAny<InternalPurgeCacheRequest>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalPurgeCacheRequest, IQueryParameterFilter, CancellationToken>((requestPath, request, queryFilter, _) => _callbacks.Add((requestPath, request, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
