using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones.Cache.InternalRequests
{
	internal class UrlWithHeaders
	{
		[JsonProperty("headers")]
		public Dictionary<string, string> Headers { get; set; } = [];

		[JsonProperty("url")]
		public string Url { get; set; }
	}
}
