using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Url with headers to purge.
	/// </summary>
	public class ZonePurgeCachedUrlRequest
	{
		/// <summary>
		/// Defined headers to specifiy the purge request.
		/// </summary>
		public Dictionary<string, string> Headers { get; set; } = [];

		/// <summary>
		/// The file url to purge.
		/// </summary>
		public string Url { get; set; }
	}
}
