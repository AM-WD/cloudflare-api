using System;
using System.Collections.Generic;
using System.Net;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// Options for the Cloudflare API.
	/// </summary>
	public class ClientOptions
	{
		/// <summary>
		/// Gets or sets the default base url for the API.
		/// </summary>
		public virtual string BaseUrl { get; set; } = "https://api.cloudflare.com/client/v4/";

		/// <summary>
		/// Gets or sets the default timeout for the API.
		/// </summary>
		public virtual TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(60);

		/// <summary>
		/// Gets or sets the maximum number of retries for the API.
		/// </summary>
		/// <remarks>
		/// The API may respond with an 5xx error and a X-Should-Retry header indicating that the request should be retried.
		/// </remarks>
		public virtual int MaxRetries { get; set; } = 2;

		/// <summary>
		/// Gets or sets additional default headers to every request.
		/// </summary>
		public virtual IDictionary<string, string> DefaultHeaders { get; set; } = new Dictionary<string, string>();

		/// <summary>
		/// Gets or sets additional default query parameters to every request.
		/// </summary>
		public virtual IDictionary<string, string> DefaultQueryParams { get; set; } = new Dictionary<string, string>();

		/// <summary>
		/// Gets or sets a value indicating whether to allow redirects.
		/// </summary>
		public virtual bool AllowRedirects { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to use a proxy.
		/// </summary>
		public virtual bool UseProxy { get; set; }

		/// <summary>
		/// Gets or sets the proxy information.
		/// </summary>
		public virtual IWebProxy Proxy { get; set; }
	}
}
