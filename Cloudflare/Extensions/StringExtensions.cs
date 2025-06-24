using System.Text.RegularExpressions;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// Extension methods for <see cref="string"/>s.
	/// </summary>
	public static class StringExtensions
	{
		private static readonly Regex _idCheckRegex = new(@"^[0-9a-f]{32}$", RegexOptions.Compiled);
		private static readonly Regex _emailCheckRegex = new(@"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$", RegexOptions.Compiled);

		/// <summary>
		/// Validate basic information for a Cloudflare ID.
		/// </summary>
		/// <remarks>
		/// An Cloudflare ID has max. 32 characters.
		/// </remarks>
		/// <param name="id">The string to check.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="id"/> is <see langword="null"/> or any kind of whitespace.</exception>
		/// <exception cref="ArgumentOutOfRangeException">The <paramref name="id"/> has more than 32 characters.</exception>
		public static void ValidateCloudflareId(this string id)
		{
			if (string.IsNullOrWhiteSpace(id))
				throw new ArgumentNullException(nameof(id));

			id.ValidateLength(32, nameof(id));

			if (!_idCheckRegex.IsMatch(id))
				throw new ArgumentException("Invalid Cloudflare ID", nameof(id));
		}

		/// <summary>
		/// Validate basic information for a Cloudflare name.
		/// </summary>
		/// <remarks>
		/// An Cloudflare name has max. 253 characters.
		/// </remarks>
		/// <param name="name">The string to check.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="name"/> is <see langword="null"/> or any kind of whitespace.</exception>
		/// <exception cref="ArgumentOutOfRangeException">The <paramref name="name"/> has more than 253 characters.</exception>
		public static void ValidateCloudflareName(this string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));

			name.ValidateLength(253, nameof(name));
		}

		/// <summary>
		/// Validate basic information for an email address.
		/// </summary>
		/// <param name="emailAddress">The string to check.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="emailAddress"/> is <see langword="null"/> or any kind of whitespace.</exception>
		/// <exception cref="ArgumentException">The <paramref name="emailAddress"/> does not match the regular expression pattern for an email address.</exception>
		public static void ValidateCloudflareEmailAddress(this string emailAddress)
		{
			if (string.IsNullOrWhiteSpace(emailAddress))
				throw new ArgumentNullException(nameof(emailAddress));

			if (!_emailCheckRegex.IsMatch(emailAddress))
				throw new ArgumentException("Invalid email address", nameof(emailAddress));
		}

		/// <summary>
		/// Validate the length of a string.
		/// </summary>
		/// <param name="str">The string to check.</param>
		/// <param name="length">The max. length.</param>
		/// <param name="paramName">The name of the parameter to check.</param>
		/// <exception cref="ArgumentException">The <paramref name="str"/> is longer than <paramref name="length"/>.</exception>
		public static void ValidateLength(this string str, int length, string paramName)
		{
			if (str?.Length > length)
				throw new ArgumentException($"The value of '{paramName}' is too long. Only {length} characters are allowed.");
		}
	}
}
