using System.Linq;
using System.Net.Http;
using AMWD.Net.Api.Cloudflare;

namespace Cloudflare.Tests.Auth
{
	[TestClass]
	public class ApiKeyAuthenticationTest
	{
		[TestMethod]
		public void ShouldAddHeaders()
		{
			// Arrange
			string emailAddress = "test@example.com";
			string apiKey = "some-api-key";

			var auth = new ApiKeyAuthentication(emailAddress, apiKey);
			using var clt = new HttpClient();

			// Act
			auth.AddHeader(clt);

			// Assert
			Assert.IsTrue(clt.DefaultRequestHeaders.Contains("X-Auth-Email"));
			Assert.IsTrue(clt.DefaultRequestHeaders.Contains("X-Auth-Key"));

			Assert.AreEqual(emailAddress, clt.DefaultRequestHeaders.GetValues("X-Auth-Email").First());
			Assert.AreEqual(apiKey, clt.DefaultRequestHeaders.GetValues("X-Auth-Key").First());
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("  ")]
		public void ShouldArgumentNullExceptionForEmailAddress(string emailAddress)
		{
			// Arrange
			string apiKey = "some-api-key";

			// Act & Assert
			Assert.ThrowsExactly<ArgumentNullException>(() => new ApiKeyAuthentication(emailAddress, apiKey));
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("  ")]
		public void ShouldArgumentNullExceptionForApiKey(string apiKey)
		{
			// Arrange
			string emailAddress = "test@example.com";

			// Act & Assert
			Assert.ThrowsExactly<ArgumentNullException>(() => new ApiKeyAuthentication(emailAddress, apiKey));
		}

		[TestMethod]
		[DataRow("test")]
		[DataRow("test@example")]
		[DataRow("example.com")]
		public void ShouldArgumentExceptionForInvalidEmailAddress(string emailAddress)
		{
			// Arrange
			string apiKey = "some-api-key";

			// Act & Assert
			Assert.ThrowsExactly<ArgumentException>(() => new ApiKeyAuthentication(emailAddress, apiKey));
		}
	}
}
