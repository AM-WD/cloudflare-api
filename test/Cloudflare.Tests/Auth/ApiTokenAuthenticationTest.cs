using System.Net.Http;
using AMWD.Net.Api.Cloudflare;

namespace Cloudflare.Tests.Auth
{
	[TestClass]
	public class ApiTokenAuthenticationTest
	{
		[TestMethod]
		public void ShouldAddHeader()
		{
			// Arrange
			string apiToken = "some-api-token";

			var auth = new ApiTokenAuthentication(apiToken);
			using var clt = new HttpClient();

			// Act
			auth.AddHeader(clt);

			// Assert
			Assert.IsTrue(clt.DefaultRequestHeaders.Contains("Authorization"));

			Assert.AreEqual("Bearer", clt.DefaultRequestHeaders.Authorization.Scheme);
			Assert.AreEqual(apiToken, clt.DefaultRequestHeaders.Authorization.Parameter);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("  ")]
		public void ShouldArgumentNullExceptionForEmailAddress(string apiToken)
		{
			// Arrange

			// Act & Assert
			Assert.ThrowsExactly<ArgumentNullException>(() => new ApiTokenAuthentication(apiToken));
		}
	}
}
