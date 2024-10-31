using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones.Internals.Requests
{
	internal class InternalPurgeCacheRequest
	{
		[JsonProperty("purge_everything")]
		public bool? PurgeEverything { get; set; }

		[JsonProperty("tags")]
		public IList<string>? Tags { get; set; }

		[JsonProperty("hosts")]
		public IList<string>? Hostnames { get; set; }

		[JsonProperty("prefixes")]
		public IList<string>? Prefixes { get; set; }

		[JsonProperty("files")]
		public IList<string>? Urls { get; set; }

		[JsonProperty("files")]
		public IList<PurgeUrlWithHeaders>? UrlsWithHeaders { get; set; }
	}
}
