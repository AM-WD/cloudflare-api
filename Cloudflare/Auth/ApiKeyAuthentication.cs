using System;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace AMWD.Net.Api.Cloudflare.Auth
{
	/// <summary>
	/// Implements the interface to authenticate using an API key and email address.
	/// </summary>
	public class ApiKeyAuthentication : IAuthentication
	{
		private static readonly Regex _emailCheckRegex = new(@"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?$", RegexOptions.Compiled);
		private readonly string _emailAddress;
		private readonly string _apiKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="ApiKeyAuthentication"/> class.
		/// </summary>
		/// <param name="emailAddress">The email address.</param>
		/// <param name="apiKey">The global API key.</param>
		public ApiKeyAuthentication(string emailAddress, string apiKey)
		{
			if (string.IsNullOrWhiteSpace(emailAddress))
				throw new ArgumentNullException(nameof(emailAddress));

			if (string.IsNullOrWhiteSpace(apiKey))
				throw new ArgumentNullException(nameof(apiKey));

			if (!_emailCheckRegex.IsMatch(emailAddress))
				throw new ArgumentException("Invalid email address", nameof(emailAddress));

			_emailAddress = emailAddress;
			_apiKey = apiKey;
		}

		/// <inheritdoc />
		public void AddHeader(HttpClient httpClient)
		{
			httpClient.DefaultRequestHeaders.Add("X-Auth-Email", _emailAddress);
			httpClient.DefaultRequestHeaders.Add("X-Auth-Key", _apiKey);
		}
	}
}
