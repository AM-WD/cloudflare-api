using System.Linq;
using System.Net.Http;
using AMWD.Net.Api.Cloudflare;

namespace Cloudflare.Core.Tests.Auth
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

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("  ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldArgumentNullExceptionForEmailAddress(string emailAddress)
		{
			// Arrange
			string apiKey = "some-api-key";

			// Act
			new ApiKeyAuthentication(emailAddress, apiKey);

			// Assert - ArgumentNullException
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("  ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldArgumentNullExceptionForApiKey(string apiKey)
		{
			// Arrange
			string emailAddress = "test@example.com";

			// Act
			new ApiKeyAuthentication(emailAddress, apiKey);

			// Assert - ArgumentNullException
		}

		[DataTestMethod]
		[DataRow("test")]
		[DataRow("test@example")]
		[DataRow("example.com")]
		[ExpectedException(typeof(ArgumentException))]
		public void ShouldArgumentExceptionForInvalidEmailAddress(string emailAddress)
		{
			// Arrange
			string apiKey = "some-api-key";

			// Act
			new ApiKeyAuthentication(emailAddress, apiKey);

			// Assert - ArgumentException
		}
	}
}
