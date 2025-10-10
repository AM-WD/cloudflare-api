using AMWD.Net.Api.Cloudflare;

namespace Cloudflare.Tests.Extensions
{
	[TestClass]
	public class DateTimeExtensionsTest
	{
		private const string Format = "yyyy-MM-dd'T'HH:mm:ss'Z'";

		[TestMethod]
		public void ShouldReturnIsoStringForUtc()
		{
			// Arrange
			var dateTime = new DateTime(2025, 10, 10, 1, 2, 3, DateTimeKind.Utc);

			// Act
			string isoString = dateTime.ToIso8601Format();

			// Assert
			Assert.AreEqual(dateTime.ToString(Format), isoString);
		}

		[TestMethod]
		public void ShouldReturnIsoStringForUnspecified()
		{
			// Arrange
			var dateTime = new DateTime(2025, 10, 10, 1, 2, 3, DateTimeKind.Unspecified);

			// Act
			string isoString = dateTime.ToIso8601Format();

			// Assert
			Assert.AreEqual(dateTime.ToString(Format), isoString);
		}

		[TestMethod]
		public void ShouldReturnIsoStringForLocal()
		{
			// Arrange
			var dateTime = new DateTime(2025, 10, 10, 1, 2, 3, DateTimeKind.Local);
			var offset = TimeZoneInfo.Local.GetUtcOffset(dateTime);

			// Act
			string isoString = dateTime.ToIso8601Format();

			// Assert
			Assert.AreEqual(dateTime.Subtract(offset).ToString(Format), isoString);
		}
	}
}
