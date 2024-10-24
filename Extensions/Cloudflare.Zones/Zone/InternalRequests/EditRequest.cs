using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones.Zones.InternalRequests
{
	internal class EditRequest
	{
		[JsonProperty("type")]
		public ZoneType? Type { get; set; }

		[JsonProperty("vanity_name_servers")]
		public IList<string> VanityNameServers { get; set; }
	}
}
