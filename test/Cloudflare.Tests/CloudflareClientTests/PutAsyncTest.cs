using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using Moq;
using Moq.Protected;

namespace Cloudflare.Core.Tests.CloudflareClientTests
{
	[TestClass]
	public class PutAsyncTest
	{
		private const string BaseUrl = "https://localhost/api/v4/";

		private HttpMessageHandlerMock _httpHandlerMock;
		private Mock<ClientOptions> _clientOptionsMock;
		private Mock<IAuthentication> _authenticationMock;

		private TestClass _request;

		[TestInitialize]
		public void Initialize()
		{
			_httpHandlerMock = new HttpMessageHandlerMock();
			_authenticationMock = new Mock<IAuthentication>();
			_clientOptionsMock = new Mock<ClientOptions>();

			_authenticationMock
				.Setup(a => a.AddHeader(It.IsAny<HttpClient>()))
				.Callback<HttpClient>(c => c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "Some-API-Token"));

			_clientOptionsMock.Setup(o => o.BaseUrl).Returns(BaseUrl);
			_clientOptionsMock.Setup(o => o.Timeout).Returns(TimeSpan.FromSeconds(60));
			_clientOptionsMock.Setup(o => o.MaxRetries).Returns(2);
			_clientOptionsMock.Setup(o => o.DefaultHeaders).Returns(new Dictionary<string, string>());
			_clientOptionsMock.Setup(o => o.DefaultQueryParams).Returns(new Dictionary<string, string>());
			_clientOptionsMock.Setup(o => o.AllowRedirects).Returns(false);
			_clientOptionsMock.Setup(o => o.UseProxy).Returns(false);

			_request = new TestClass
			{
				Int = 54321,
				Str = "Happy Testing!"
			};
		}

		[TestMethod]
		[ExpectedException(typeof(ObjectDisposedException))]
		public async Task ShouldThrowDisposed()
		{
			// Arrange
			var client = GetClient() as CloudflareClient;
			client.Dispose();

			// Act
			await client.PutAsync<object, object>("test", _request);

			// Assert - ObjectDisposedException
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullOnRequestPath(string path)
		{
			// Arrange
			var client = GetClient();

			// Act
			await client.PutAsync<object, object>(path, _request);

			// Assert - ArgumentNullException
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task ShouldThrowArgumentOnRequestPath()
		{
			// Arrange
			var client = GetClient();

			// Act
			await client.PutAsync<object, object>("foo?bar=baz", _request);

			// Assert - ArgumentException
		}

		[TestMethod]
		public async Task ShouldPut()
		{
			// Arrange
			_httpHandlerMock.Responses.Enqueue(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(@"{""success"": true, ""errors"": [], ""messages"": [], ""result"": { ""string"": ""some-string"", ""integer"": 123 }}", Encoding.UTF8, MediaTypeNames.Application.Json),
			});

			var client = GetClient();

			// Act
			var response = await client.PutAsync<TestClass, TestClass>("test", _request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Errors);
			Assert.IsNotNull(response.Messages);
			Assert.IsNull(response.ResultInfo);

			Assert.AreEqual(0, response.Errors.Count);
			Assert.AreEqual(0, response.Messages.Count);

			Assert.IsNotNull(response.Result);
			Assert.AreEqual("some-string", response.Result.Str);
			Assert.AreEqual(123, response.Result.Int);

			Assert.AreEqual(1, _httpHandlerMock.Callbacks.Count);

			var callback = _httpHandlerMock.Callbacks.First();
			Assert.AreEqual(HttpMethod.Put, callback.Method);
			Assert.AreEqual("https://localhost/api/v4/test", callback.Url);
			Assert.AreEqual(@"{""string"":""Happy Testing!"",""integer"":54321}", callback.Content);

			Assert.AreEqual(3, callback.Headers.Count);
			Assert.IsTrue(callback.Headers.ContainsKey("Accept"));
			Assert.IsTrue(callback.Headers.ContainsKey("Authorization"));
			Assert.IsTrue(callback.Headers.ContainsKey("User-Agent"));

			Assert.AreEqual("application/json", callback.Headers["Accept"]);
			Assert.AreEqual("Bearer Some-API-Token", callback.Headers["Authorization"]);
			Assert.AreEqual("AMWD.CloudflareClient/1.0.0", callback.Headers["User-Agent"]);

			_httpHandlerMock.Mock.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());

			VerifyDefaults();
			VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldPutHttpContentDirectly()
		{
			// Arrange
			var stringContent = new StringContent(@"{""test"":""HERE ?""}", Encoding.UTF8, "application/json");
			_httpHandlerMock.Responses.Enqueue(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(@"{""success"": true, ""errors"": [], ""messages"": [], ""result"": { ""string"": ""some-string"", ""integer"": 123 }}", Encoding.UTF8, MediaTypeNames.Application.Json),
			});

			var client = GetClient();

			// Act
			var response = await client.PutAsync<TestClass, StringContent>("test", stringContent);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Errors);
			Assert.IsNotNull(response.Messages);
			Assert.IsNull(response.ResultInfo);

			Assert.AreEqual(0, response.Errors.Count);
			Assert.AreEqual(0, response.Messages.Count);

			Assert.IsNotNull(response.Result);
			Assert.AreEqual("some-string", response.Result.Str);
			Assert.AreEqual(123, response.Result.Int);

			Assert.AreEqual(1, _httpHandlerMock.Callbacks.Count);

			var callback = _httpHandlerMock.Callbacks.First();
			Assert.AreEqual(HttpMethod.Put, callback.Method);
			Assert.AreEqual("https://localhost/api/v4/test", callback.Url);
			Assert.AreEqual(@"{""test"":""HERE ?""}", callback.Content);

			Assert.AreEqual(3, callback.Headers.Count);
			Assert.IsTrue(callback.Headers.ContainsKey("Accept"));
			Assert.IsTrue(callback.Headers.ContainsKey("Authorization"));
			Assert.IsTrue(callback.Headers.ContainsKey("User-Agent"));

			Assert.AreEqual("application/json", callback.Headers["Accept"]);
			Assert.AreEqual("Bearer Some-API-Token", callback.Headers["Authorization"]);
			Assert.AreEqual("AMWD.CloudflareClient/1.0.0", callback.Headers["User-Agent"]);

			_httpHandlerMock.Mock.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());

			VerifyDefaults();
			VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldPutWithoutContent()
		{
			// Arrange
			_httpHandlerMock.Responses.Enqueue(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(@"{""success"": true, ""errors"": [], ""messages"": [], ""result"": { ""string"": ""some-string"", ""integer"": 123 }}", Encoding.UTF8, MediaTypeNames.Application.Json),
			});

			var client = GetClient();

			// Act
			var response = await client.PutAsync<TestClass, object>("putput", null);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Errors);
			Assert.IsNotNull(response.Messages);
			Assert.IsNull(response.ResultInfo);

			Assert.AreEqual(0, response.Errors.Count);
			Assert.AreEqual(0, response.Messages.Count);

			Assert.IsNotNull(response.Result);
			Assert.AreEqual("some-string", response.Result.Str);
			Assert.AreEqual(123, response.Result.Int);

			Assert.AreEqual(1, _httpHandlerMock.Callbacks.Count);

			var callback = _httpHandlerMock.Callbacks.First();
			Assert.AreEqual(HttpMethod.Put, callback.Method);
			Assert.AreEqual("https://localhost/api/v4/putput", callback.Url);
			Assert.IsNull(callback.Content);

			Assert.AreEqual(3, callback.Headers.Count);
			Assert.IsTrue(callback.Headers.ContainsKey("Accept"));
			Assert.IsTrue(callback.Headers.ContainsKey("Authorization"));
			Assert.IsTrue(callback.Headers.ContainsKey("User-Agent"));

			Assert.AreEqual("application/json", callback.Headers["Accept"]);
			Assert.AreEqual("Bearer Some-API-Token", callback.Headers["Authorization"]);
			Assert.AreEqual("AMWD.CloudflareClient/1.0.0", callback.Headers["User-Agent"]);

			_httpHandlerMock.Mock.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());
		}

		[TestMethod]
		[DataRow(HttpStatusCode.Unauthorized)]
		[DataRow(HttpStatusCode.Forbidden)]
		public async Task ShouldThrowAuthenticationExceptionOnStatusCode(HttpStatusCode statusCode)
		{
			// Arrange
			_httpHandlerMock.Responses.Enqueue(new HttpResponseMessage
			{
				StatusCode = statusCode,
				Content = new StringContent(@"{""success"": false, ""errors"": [{  ""code"": ""4711"", ""message"": ""foo & baz."" }, {  ""code"": ""4712"", ""message"": ""Happy Error!"" }], ""messages"": []}", Encoding.UTF8, MediaTypeNames.Application.Json),
			});

			var client = GetClient();

			try
			{
				// Act
				await client.PutAsync<object, object>("foo", _request);
				Assert.Fail();
			}
			catch (AuthenticationException ex)
			{
				// Assert
				Assert.IsNull(ex.InnerException);
				Assert.AreEqual($"4711: foo & baz.{Environment.NewLine}4712: Happy Error!", ex.Message);
			}
		}

		[TestMethod]
		public async Task ShouldReturnPlainText()
		{
			// Arrange
			_clientOptionsMock.Setup(o => o.DefaultQueryParams).Returns(new Dictionary<string, string> { { "bar", "08/15" } });
			_httpHandlerMock.Responses.Enqueue(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent("This is an awesome text ;-)", Encoding.UTF8, MediaTypeNames.Text.Plain),
			});

			var client = GetClient();

			// Act
			var response = await client.PutAsync<string, TestClass>("some-awesome-path", _request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNull(response.Errors);
			Assert.IsNull(response.Messages);
			Assert.IsNull(response.ResultInfo);

			Assert.AreEqual("This is an awesome text ;-)", response.Result);

			Assert.AreEqual(1, _httpHandlerMock.Callbacks.Count);

			var callback = _httpHandlerMock.Callbacks.First();
			Assert.AreEqual(HttpMethod.Put, callback.Method);
			Assert.AreEqual("https://localhost/api/v4/some-awesome-path?bar=08%2F15", callback.Url);
			Assert.AreEqual(@"{""string"":""Happy Testing!"",""integer"":54321}", callback.Content);

			Assert.AreEqual(3, callback.Headers.Count);
			Assert.IsTrue(callback.Headers.ContainsKey("Accept"));
			Assert.IsTrue(callback.Headers.ContainsKey("Authorization"));
			Assert.IsTrue(callback.Headers.ContainsKey("User-Agent"));

			Assert.AreEqual("application/json", callback.Headers["Accept"]);
			Assert.AreEqual("Bearer Some-API-Token", callback.Headers["Authorization"]);
			Assert.AreEqual("AMWD.CloudflareClient/1.0.0", callback.Headers["User-Agent"]);

			_httpHandlerMock.Mock.Protected().Verify("SendAsync", Times.Once(), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());

			_authenticationMock.Verify(m => m.AddHeader(It.IsAny<HttpClient>()), Times.Once);

			_clientOptionsMock.Verify(o => o.BaseUrl, Times.Exactly(2));
			_clientOptionsMock.Verify(o => o.Timeout, Times.Exactly(2));
			_clientOptionsMock.Verify(o => o.MaxRetries, Times.Exactly(2));
			_clientOptionsMock.Verify(o => o.DefaultHeaders, Times.Once);
			_clientOptionsMock.Verify(o => o.DefaultQueryParams, Times.Exactly(2));
			_clientOptionsMock.Verify(o => o.AllowRedirects, Times.Once);
			_clientOptionsMock.Verify(o => o.UseProxy, Times.Exactly(2));
			_clientOptionsMock.Verify(o => o.Proxy, Times.Once);

			VerifyNoOtherCalls();
		}

		[TestMethod]
		[ExpectedException(typeof(JsonReaderException))]
		public async Task ShouldThrowExceptionOnInvalidResponse()
		{
			// Arrange
			_httpHandlerMock.Responses.Enqueue(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent("This is a bad text :p", Encoding.UTF8, MediaTypeNames.Text.Plain),
			});

			var client = GetClient();

			// Act
			await client.PutAsync<TestClass, TestClass>("some-path", _request);
		}

		private void VerifyDefaults()
		{
			_authenticationMock.Verify(m => m.AddHeader(It.IsAny<HttpClient>()), Times.Once);

			_clientOptionsMock.Verify(o => o.BaseUrl, Times.Exactly(2));
			_clientOptionsMock.Verify(o => o.Timeout, Times.Exactly(2));
			_clientOptionsMock.Verify(o => o.MaxRetries, Times.Exactly(2));
			_clientOptionsMock.Verify(o => o.DefaultHeaders, Times.Once);
			_clientOptionsMock.Verify(o => o.DefaultQueryParams, Times.Once);
			_clientOptionsMock.Verify(o => o.AllowRedirects, Times.Once);
			_clientOptionsMock.Verify(o => o.UseProxy, Times.Exactly(2));
			_clientOptionsMock.Verify(o => o.Proxy, Times.Once);
		}

		private void VerifyNoOtherCalls()
		{
			_httpHandlerMock.Mock.VerifyNoOtherCalls();
			_authenticationMock.VerifyNoOtherCalls();
			_clientOptionsMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			var httpClient = new HttpClient(_httpHandlerMock.Mock.Object)
			{
				Timeout = _clientOptionsMock.Object.Timeout,
				BaseAddress = new Uri(BaseUrl),
			};

			httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AMWD.CloudflareClient", "1.0.0"));
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			if (_clientOptionsMock.Object.DefaultHeaders.Count > 0)
			{
				foreach (var headerKvp in _clientOptionsMock.Object.DefaultHeaders)
					httpClient.DefaultRequestHeaders.Add(headerKvp.Key, headerKvp.Value);
			}
			_authenticationMock.Object.AddHeader(httpClient);

			_authenticationMock.Invocations.Clear();
			_clientOptionsMock.Invocations.Clear();

			var client = new CloudflareClient(_authenticationMock.Object, _clientOptionsMock.Object);

			var httpClientField = client.GetType()
				.GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance);

			(httpClientField.GetValue(client) as HttpClient).Dispose();
			httpClientField.SetValue(client, httpClient);

			return client;
		}

		private class TestClass
		{
			[JsonProperty("string")]
			public string Str { get; set; }

			[JsonProperty("integer")]
			public int Int { get; set; }
		}

		private class TestFilter : IQueryParameterFilter
		{
			public IReadOnlyDictionary<string, string> GetQueryParameters()
			{
				return new Dictionary<string, string>
				{
					{ "test", "filter-text" }
				};
			}
		}
	}
}
