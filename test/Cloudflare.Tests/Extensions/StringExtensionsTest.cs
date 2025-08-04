using System;
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
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowArgumentNullExceptionForValidateId(string name)
		{
			// Arrange

			// Act
			name.ValidateCloudflareId();

			// Assert - ArgumentNullException
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ShouldThrowArgumentOutOfRangeExceptionForValidateId()
		{
			// Arrange
			string id = new('a', 33);

			// Act
			id.ValidateCloudflareId();

			// Assert - ArgumentException
		}

		[TestMethod]
		[DataRow("023e105f4ecef8ad9ca31a8372d0c35")]
		[DataRow("023e105f4ecef8ad9ca31a8372d0C353")]
		[DataRow("023e105f4ecef8ad9ca31a8372d0y353")]
		[ExpectedException(typeof(ArgumentException))]
		public void ShouldThrowArgumentExceptionForValidateId(string id)
		{
			// Arrange

			// Act
			id.ValidateCloudflareId();

			// Assert - ArgumentException
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
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowArgumentNullExceptionForValidateName(string name)
		{
			// Arrange

			// Act
			name.ValidateCloudflareName();

			// Assert - ArgumentNullException
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ShouldThrowArgumentOutOfRangeExceptionForValidateName()
		{
			// Arrange
			string name = new('a', 254);

			// Act
			name.ValidateCloudflareName();

			// Assert - ArgumentException
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
		[ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowArgumentNullExceptionForValidateEmail(string email)
		{
			// Arrange

			// Act
			email.ValidateCloudflareEmailAddress();

			// Assert - ArgumentNullException
		}

		[TestMethod]
		[DataRow("test")]
		[DataRow("test@example")]
		[DataRow("example.com")]
		[ExpectedException(typeof(ArgumentException))]
		public void ShouldThrowArgumentExceptionForValidateEmail(string email)
		{
			// Arrange

			// Act
			email.ValidateCloudflareEmailAddress();

			// Assert - ArgumentException
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
		[ExpectedException(typeof(ArgumentException))]
		public void ShouldThrowArgumentExceptionForValidateLength()
		{
			// Arrange
			string str = "SomeExampleString";

			// Act
			str.ValidateLength(10, nameof(str));

			// Assert - ArgumentException
		}
	}
}
