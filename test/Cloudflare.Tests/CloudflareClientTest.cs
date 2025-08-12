using System.Linq;
using System.Net.Http;
using System.Reflection;
using AMWD.Net.Api.Cloudflare;
using Moq;
using Moq.Protected;

namespace Cloudflare.Tests
{
	[TestClass]
	public class CloudflareClientTest
	{
		private Mock<HttpMessageHandler> _httpHandlerMock;
		private Mock<ClientOptions> _clientOptionsMock;
		private Mock<IAuthentication> _authenticationMock;

		[TestInitialize]
		public void Initialize()
		{
			_httpHandlerMock = new Mock<HttpMessageHandler>();
			_authenticationMock = new Mock<IAuthentication>();
			_clientOptionsMock = new Mock<ClientOptions>();

			_clientOptionsMock.Setup(o => o.BaseUrl).Returns("http://localhost/api/v4/");
			_clientOptionsMock.Setup(o => o.Timeout).Returns(TimeSpan.FromSeconds(60));
			_clientOptionsMock.Setup(o => o.MaxRetries).Returns(2);
			_clientOptionsMock.Setup(o => o.DefaultHeaders).Returns(new Dictionary<string, string>());
			_clientOptionsMock.Setup(o => o.DefaultQueryParams).Returns(new Dictionary<string, string>());
			_clientOptionsMock.Setup(o => o.AllowRedirects).Returns(false);
			_clientOptionsMock.Setup(o => o.UseProxy).Returns(false);
		}

		[TestMethod]
		public void ShouldInitializeWithEmailAndKey()
		{
			// Arrange
			string email = "test@example.com";
			string apiKey = "some-api-key";

			// Act
			using var client = new CloudflareClient(email, apiKey);

			// Assert
			var httpClient = client.GetType()
				.GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(client) as HttpClient;

			Assert.IsNotNull(httpClient);
			Assert.AreEqual(email, httpClient.DefaultRequestHeaders.GetValues("X-Auth-Email").First());
			Assert.AreEqual(apiKey, httpClient.DefaultRequestHeaders.GetValues("X-Auth-Key").First());

			VerifyNoOtherCalls();
		}

		[TestMethod]
		public void ShouldInitializeWithApiToken()
		{
			// Arrange
			string token = "some-special-api-token";

			// Act
			using var client = new CloudflareClient(token);

			// Assert
			var httpClient = client.GetType()
				.GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)
				.GetValue(client) as HttpClient;

			Assert.IsNotNull(httpClient);
			Assert.AreEqual($"Bearer {token}", httpClient.DefaultRequestHeaders.GetValues("Authorization").First());

			VerifyNoOtherCalls();
		}

		[TestMethod]
		public void ShouldThrowArgumentNullOnMissingAuthentication()
		{
			// Arrange

			// Act & Assert
			Assert.ThrowsExactly<ArgumentNullException>(() =>
			{
				using var client = new CloudflareClient((IAuthentication)null);
			});
		}

		[TestMethod]
		public void ShouldAddCustomDefaultHeaders()
		{
			// Arrange
			var clientOptions = new ClientOptions();
			clientOptions.DefaultHeaders.Add("SomeKey", "SomeValue");

			// Act
			using var client = new CloudflareClient("token", clientOptions);

			// Assert
			var httpClient = client.GetType()
				.GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance)
				.GetValue(client) as HttpClient;

			Assert.IsNotNull(httpClient);
			Assert.IsTrue(httpClient.DefaultRequestHeaders.Contains("SomeKey"));
			Assert.AreEqual("SomeValue", httpClient.DefaultRequestHeaders.GetValues("SomeKey").First());

			VerifyNoOtherCalls();
		}

		[TestMethod]
		public void ShouldDisposeHttpClient()
		{
			// Arrange
			var client = GetClient();

			// Act
			client.Dispose();

			// Assert
			_httpHandlerMock.Protected().Verify("Dispose", Times.Once(), exactParameterMatch: true, true);

			VerifyDefault();
			VerifyNoOtherCalls();
		}

		[TestMethod]
		public void ShouldAllowMultipleDispose()
		{
			// Arrange
			var client = GetClient();

			// Act
			client.Dispose();
			client.Dispose();

			// Assert
			_httpHandlerMock.Protected().Verify("Dispose", Times.Once(), exactParameterMatch: true, true);

			VerifyDefault();
			VerifyNoOtherCalls();
		}

		[TestMethod]
		public void ShouldAssertClientOptions()
		{
			// Arrange + Act
			var client = GetClient();

			// Assert
			VerifyDefault();
			VerifyNoOtherCalls();
		}

		[TestMethod]
		public void ShouldThrowArgumentNullForBaseUrlOnAssertClientOptions()
		{
			// Arrange
			_clientOptionsMock
				.Setup(o => o.BaseUrl)
				.Returns((string)null);

			// Act & Assert
			Assert.ThrowsExactly<ArgumentNullException>(() =>
			{
				var client = GetClient();
			});
		}

		[TestMethod]
		public void ShouldThrowArgumentOutOfRangeForTimeoutOnAssertClientOptions()
		{
			// Arrange
			_clientOptionsMock
				.Setup(o => o.Timeout)
				.Returns(TimeSpan.Zero);

			// Act & Assert
			Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
			{
				var client = GetClient();
			});
		}

		[TestMethod]
		[DataRow(-1)]
		[DataRow(11)]
		public void ShouldThrowArgumentOutOfRangeForMaxRetriesOnAssertClientOptions(int maxRetries)
		{
			// Arrange
			_clientOptionsMock
				.Setup(o => o.MaxRetries)
				.Returns(maxRetries);

			// Act & Assert
			Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
			{
				var client = GetClient();
			});
		}

		[TestMethod]
		public void ShouldThrowArgumentNullForUseProxyOnAssertClientOptions()
		{
			// Arrange
			_clientOptionsMock
				.Setup(o => o.UseProxy)
				.Returns(true);

			// Act & Assert
			Assert.ThrowsExactly<ArgumentNullException>(() =>
			{
				var client = GetClient();
			});
		}

		private void VerifyDefault()
		{
			_clientOptionsMock.VerifyGet(o => o.AllowRedirects, Times.Once);
			_clientOptionsMock.VerifyGet(o => o.BaseUrl, Times.Exactly(2));
			_clientOptionsMock.VerifyGet(o => o.DefaultHeaders, Times.Once);
			_clientOptionsMock.VerifyGet(o => o.MaxRetries, Times.Exactly(2));
			_clientOptionsMock.VerifyGet(o => o.Proxy, Times.Once);
			_clientOptionsMock.VerifyGet(o => o.Timeout, Times.Exactly(2));
			_clientOptionsMock.VerifyGet(o => o.UseProxy, Times.Exactly(2));

			_authenticationMock.Verify(a => a.AddHeader(It.IsAny<HttpClient>()), Times.Once);
		}

		private void VerifyNoOtherCalls()
		{
			_httpHandlerMock.VerifyNoOtherCalls();
			_clientOptionsMock.VerifyNoOtherCalls();
			_authenticationMock.VerifyNoOtherCalls();
		}

		private CloudflareClient GetClient()
		{
			var httpClient = new HttpClient(_httpHandlerMock.Object);
			var client = new CloudflareClient(_authenticationMock.Object, _clientOptionsMock.Object);

			var httpClientField = client.GetType()
				.GetField("_httpClient", BindingFlags.NonPublic | BindingFlags.Instance);

			(httpClientField.GetValue(client) as HttpClient).Dispose();
			httpClientField.SetValue(client, httpClient);

			return client;
		}
	}
}
