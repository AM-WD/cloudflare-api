using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Url with headers to purge.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the <see cref="ZonePurgeCachedUrlRequest"/> class.
	/// </remarks>
	/// <param name="url">The url to purge.</param>
	public class ZonePurgeCachedUrlRequest(string url)
	{
		/// <summary>
		/// Defined headers to specifiy the purge request.
		/// </summary>
		public Dictionary<string, string> Headers { get; set; } = [];

		/// <summary>
		/// The file url to purge.
		/// </summary>
		public string Url { get; set; } = url;
	}
}
