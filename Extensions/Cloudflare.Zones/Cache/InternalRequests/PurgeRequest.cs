using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones.Cache.InternalRequests
{
	internal class PurgeRequest
	{
		[JsonProperty("purge_everything")]
		public bool? PurgeEverything { get; set; }

		[JsonProperty("tags")]
		public IList<string> Tags { get; set; }

		[JsonProperty("hosts")]
		public IList<string> Hostnames { get; set; }

		[JsonProperty("prefixes")]
		public IList<string> Prefixes { get; set; }

		[JsonProperty("files")]
		public IList<string> Urls { get; set; }

		[JsonProperty("files")]
		public IList<UrlWithHeaders> UrlsWithHeaders { get; set; }
	}
}
