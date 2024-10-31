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
	public class PurgeCachedContentByPrefixTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";
		private readonly DateTime DateTime = new(2024, 10, 10, 20, 30, 40, 0, DateTimeKind.Utc);

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<ZoneIdResponse> _response;

		private List<(string RequestPath, InternalPurgeCacheRequest Request, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<ZoneIdResponse>
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
				Result = new ZoneIdResponse
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
				null,
				"example.com/foo",
				"example.com/bar",
				"example.org/hello/world",
			};

			var client = GetClient();

			// Act
			var response = await client.PurgeCachedContentByPrefix(ZoneId, list);

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
			Assert.IsNull(callback.Request.Tags);
			Assert.IsNull(callback.Request.Hostnames);
			Assert.IsNotNull(callback.Request.Prefixes);
			Assert.AreEqual(3, callback.Request.Prefixes.Count);
			Assert.AreEqual("example.com/foo", callback.Request.Prefixes[0]);
			Assert.AreEqual("example.com/bar", callback.Request.Prefixes[1]);
			Assert.AreEqual("example.org/hello/world", callback.Request.Prefixes[2]);

			Assert.IsNull(callback.QueryFilter);

			_clientMock.Verify(m => m.PostAsync<ZoneIdResponse, InternalPurgeCacheRequest>($"zones/{ZoneId}/purge_cache", It.IsAny<InternalPurgeCacheRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumenNullExceptionIfListIsNull()
		{
			// Arrange
			var client = GetClient();

			// Act
			await client.PurgeCachedContentByPrefix(ZoneId, null);

			// Assert - ArgumentNullException
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<ZoneIdResponse, InternalPurgeCacheRequest>(It.IsAny<string>(), It.IsAny<InternalPurgeCacheRequest>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalPurgeCacheRequest, IQueryParameterFilter, CancellationToken>((requestPath, request, queryFilter, _) => _callbacks.Add((requestPath, request, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
