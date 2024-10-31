using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones.Internals.Requests
{
	internal class InternalEditZoneRequest
	{
		[JsonProperty("type")]
		public ZoneType? Type { get; set; }

		[JsonProperty("vanity_name_servers")]
		public IList<string>? VanityNameServers { get; set; }
	}
}
