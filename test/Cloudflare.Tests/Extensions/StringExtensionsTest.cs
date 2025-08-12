using AMWD.Net.Api.Cloudflare;

namespace Cloudflare.Tests.Extensions
{
	[TestClass]
	public class StringExtensionsTest
	{
		[TestMethod]
		public void ShouldValidateId()
		{
			// Arrange
			string id = "023e105f4ecef8ad9ca31a8372d0c353";

			// Act
			id.ValidateCloudflareId();

			// Assert - no exception thrown
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("  ")]
		public void ShouldThrowArgumentNullExceptionForValidateId(string name)
		{
			// Arrange

			// Act & Assert
			Assert.ThrowsExactly<ArgumentNullException>(() => name.ValidateCloudflareId());
		}

		[TestMethod]
		public void ShouldThrowArgumentOutOfRangeExceptionForValidateId()
		{
			// Arrange
			string id = new('a', 33);

			// Act & Assert
			Assert.ThrowsExactly<ArgumentException>(() => id.ValidateCloudflareId());
		}

		[TestMethod]
		[DataRow("023e105f4ecef8ad9ca31a8372d0c35")]
		[DataRow("023e105f4ecef8ad9ca31a8372d0C353")]
		[DataRow("023e105f4ecef8ad9ca31a8372d0y353")]
		public void ShouldThrowArgumentExceptionForValidateId(string id)
		{
			// Arrange

			// Act & Assert
			Assert.ThrowsExactly<ArgumentException>(() => id.ValidateCloudflareId());
		}

		[TestMethod]
		public void ShouldValidateName()
		{
			// Arrange
			string name = "Example Account Name";

			// Act
			name.ValidateCloudflareName();

			// Assert - no exception thrown
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("  ")]
		public void ShouldThrowArgumentNullExceptionForValidateName(string name)
		{
			// Arrange

			// Act & Assert
			Assert.ThrowsExactly<ArgumentNullException>(() => name.ValidateCloudflareName());
		}

		[TestMethod]
		public void ShouldThrowArgumentOutOfRangeExceptionForValidateName()
		{
			// Arrange
			string name = new('a', 254);

			// Act & Assert
			Assert.ThrowsExactly<ArgumentException>(() => name.ValidateCloudflareName());
		}

		[TestMethod]
		public void ShouldValidateEmail()
		{
			// Arrange
			string email = "test@example.com";

			// Act
			email.ValidateCloudflareEmailAddress();

			// Assert - no exception thrown
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("  ")]
		public void ShouldThrowArgumentNullExceptionForValidateEmail(string email)
		{
			// Arrange

			// Act & Assert
			Assert.ThrowsExactly<ArgumentNullException>(() => email.ValidateCloudflareEmailAddress());
		}

		[TestMethod]
		[DataRow("test")]
		[DataRow("test@example")]
		[DataRow("example.com")]
		public void ShouldThrowArgumentExceptionForValidateEmail(string email)
		{
			// Arrange

			// Act & Assert
			Assert.ThrowsExactly<ArgumentException>(() => email.ValidateCloudflareEmailAddress());
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("SomeExampleString")]
		public void ShouldValidateLength(string str)
		{
			// Arrange

			// Act
			str.ValidateLength(30, nameof(str));

			// Assert - no exception thrown
		}

		[TestMethod]
		public void ShouldThrowArgumentExceptionForValidateLength()
		{
			// Arrange
			string str = "SomeExampleString";

			// Act & Assert
			Assert.ThrowsExactly<ArgumentException>(() => str.ValidateLength(10, nameof(str)));
		}
	}
}
