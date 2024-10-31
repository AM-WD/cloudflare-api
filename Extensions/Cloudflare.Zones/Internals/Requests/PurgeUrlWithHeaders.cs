using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones.Internals.Requests
{
	internal class PurgeUrlWithHeaders
	{
		[JsonProperty("headers")]
		public Dictionary<string, string> Headers { get; set; } = [];

		[JsonProperty("url")]
		public string? Url { get; set; }
	}
}
