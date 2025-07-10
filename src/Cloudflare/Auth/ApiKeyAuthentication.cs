using System.Net.Http;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// Implements the interface to authenticate using an API key and email address.
	/// </summary>
	public class ApiKeyAuthentication : IAuthentication
	{
		private readonly string _emailAddress;
		private readonly string _apiKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="ApiKeyAuthentication"/> class.
		/// </summary>
		/// <param name="emailAddress">The email address.</param>
		/// <param name="apiKey">The global API key.</param>
		public ApiKeyAuthentication(string emailAddress, string apiKey)
		{
			emailAddress.ValidateCloudflareEmailAddress();

			if (string.IsNullOrWhiteSpace(apiKey))
				throw new ArgumentNullException(nameof(apiKey));

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
