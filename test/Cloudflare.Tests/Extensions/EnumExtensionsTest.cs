using System.Runtime.Serialization;
using AMWD.Net.Api.Cloudflare;

namespace Cloudflare.Tests.Extensions
{
	[TestClass]
	public class EnumExtensionsTest
	{
		[TestMethod]
		public void ShouldReturnEnumMemberValue()
		{
			// Arrange
			var enumValue = EnumWithAttribute.One;

			// Act
			string val = enumValue.GetEnumMemberValue();

			// Assert
			Assert.AreEqual("eins", val);
		}

		[TestMethod]
		public void ShouldReturnStringMissingAttribute()
		{
			// Arrange
			var enumValue = EnumWithoutAttribute.Two;

			// Act
			string val = enumValue.GetEnumMemberValue();

			// Assert
			Assert.AreEqual("Two", val);
		}

		[TestMethod]
		public void ShouldReturnString()
		{
			// Arrange
			EnumWithAttribute enumValue = 0;

			// Act
			string val = enumValue.GetEnumMemberValue();

			// Assert
			Assert.AreEqual("0", val);
		}

		public enum EnumWithAttribute
		{
			[EnumMember(Value = "eins")]
			One = 1,

			[EnumMember(Value = "zwei")]
			Two = 2,
		}

		public enum EnumWithoutAttribute
		{
			One = 1,

			Two = 2,
		}
	}
}
