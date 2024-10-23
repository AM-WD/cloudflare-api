using System;
using System.Linq;
using System.Runtime.Serialization;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// Extension methods for <see cref="Enum"/>s.
	/// </summary>
	public static class EnumExtensions
	{
		/// <summary>
		/// Gets the <see cref="EnumMemberAttribute.Value"/> of the <see cref="Enum"/> when available, otherwise the <see cref="Enum.ToString()"/>.
		/// </summary>
		/// <param name="value">The enum value.</param>
		public static string GetEnumMemberValue(this Enum value)
		{
			var fieldInfo = value.GetType().GetField(value.ToString());
			if (fieldInfo == null)
				return value.ToString();

			var enumMember = fieldInfo
				.GetCustomAttributes(typeof(EnumMemberAttribute), inherit: false)
				.Cast<EnumMemberAttribute>()
				.FirstOrDefault();

			if (enumMember == null)
				return value.ToString();

			return enumMember.Value;
		}
	}
}
