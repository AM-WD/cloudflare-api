using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AMWD.Net.Api.Cloudflare.Auth
{
	/// <summary>
	/// Implements the interface to authenticate using an API token.
	/// </summary>
	public class ApiTokenAuthentication : IAuthentication
	{
		private readonly string _apiToken;

		/// <summary>
		/// Initializes a new instance of the <see cref="ApiTokenAuthentication"/> class.
		/// </summary>
		/// <param name="apiToken">The API token.</param>
		public ApiTokenAuthentication(string apiToken)
		{
			if (string.IsNullOrWhiteSpace(apiToken))
				throw new ArgumentNullException(nameof(apiToken));

			_apiToken = apiToken;
		}

		/// <inheritdoc />
		public void AddHeader(HttpClient httpClient)
		{
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiToken);
		}
	}
}
