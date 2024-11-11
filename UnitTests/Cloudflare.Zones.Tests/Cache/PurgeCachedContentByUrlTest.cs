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
	public class PurgeCachedContentByUrlTest
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
		public async Task ShouldPurgeCachedContentByUrl()
		{
			// Arrange
			var list = new List<ZonePurgeCachedUrlRequest>
			{
				new("https://example.com/foo.txt"),
				new("https://example.com/bar.baz"),
			};

			var client = GetClient();

			// Act
			var response = await client.PurgeCachedContentByUrl(ZoneId, list);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"zones/{ZoneId}/purge_cache", callback.RequestPath);

			Assert.IsNotNull(callback.Request);
			Assert.IsNull(callback.Request.PurgeEverything);
			Assert.IsNotNull(callback.Request.Urls);
			Assert.AreEqual(2, callback.Request.Urls.Count);
			Assert.AreEqual("https://example.com/foo.txt", callback.Request.Urls[0]);
			Assert.AreEqual("https://example.com/bar.baz", callback.Request.Urls[1]);
			Assert.IsNull(callback.Request.UrlsWithHeaders);
			Assert.IsNull(callback.Request.Tags);
			Assert.IsNull(callback.Request.Hostnames);
			Assert.IsNull(callback.Request.Prefixes);

			Assert.IsNull(callback.QueryFilter);

			_clientMock.Verify(m => m.PostAsync<IdResponse, InternalPurgeCacheRequest>($"zones/{ZoneId}/purge_cache", It.IsAny<InternalPurgeCacheRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldPurgeCachedContentByUrlWithHeader()
		{
			// Arrange
			var list = new List<ZonePurgeCachedUrlRequest>
			{
				new("https://example.com/foo.txt") { Headers = new Dictionary<string, string> { { "X-Test1", "Test" } } },
				new("https://example.com/bar.baz") { Headers = new Dictionary<string, string> { { "X-Test2", "Test" } } },
			};

			var client = GetClient();

			// Act
			var response = await client.PurgeCachedContentByUrl(ZoneId, list);

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
			Assert.IsNotNull(callback.Request.UrlsWithHeaders);
			Assert.AreEqual(2, callback.Request.UrlsWithHeaders.Count);
			Assert.AreEqual("https://example.com/foo.txt", callback.Request.UrlsWithHeaders[0].Url);
			Assert.AreEqual("Test", callback.Request.UrlsWithHeaders[0].Headers["X-Test1"]);
			Assert.AreEqual("https://example.com/bar.baz", callback.Request.UrlsWithHeaders[1].Url);
			Assert.AreEqual("Test", callback.Request.UrlsWithHeaders[1].Headers["X-Test2"]);
			Assert.IsNull(callback.Request.Tags);
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
			await client.PurgeCachedContentByUrl(ZoneId, null);

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
