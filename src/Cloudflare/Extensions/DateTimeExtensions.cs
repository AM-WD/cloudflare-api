namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// Extension methods for <see cref="DateTime"/>s.
	/// </summary>
	public static class DateTimeExtensions
	{
		private const string Iso8601Format = "yyyy-MM-dd'T'HH:mm:ss'Z'";

		/// <summary>
		/// Converts the specified <see cref="DateTime"/> to its ISO 8601 string representation.
		/// </summary>
		/// <remarks>
		/// This method ensures that the resulting string conforms to the ISO 8601 standard, which is
		/// commonly used for representing dates and times in a machine-readable format.
		/// </remarks>
		/// <param name="dateTime"> The <see cref="DateTime"/> instance to convert.</param>
		/// <returns>
		/// A string representing the <paramref name="dateTime"/> in ISO 8601 format.
		/// <br/>
		/// If the <see cref="DateTime.Kind"/> property is <see cref="DateTimeKind.Local"/>, the value is first converted to <see cref="DateTimeKind.Utc"/>.
		/// <br/>
		/// If the <see cref="DateTime.Kind"/> is <see cref="DateTimeKind.Unspecified"/>, it is treated as <see cref="DateTimeKind.Utc"/>.
		/// </returns>
		public static string ToIso8601Format(this DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Local)
				return dateTime.ToUniversalTime().ToString(Iso8601Format);

			// DateTimeKind.Unspecified is treated as UTC
			return dateTime.ToString(Iso8601Format);
		}
	}
}
