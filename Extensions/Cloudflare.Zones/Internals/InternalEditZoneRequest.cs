namespace AMWD.Net.Api.Cloudflare.Zones.Internals
{
	internal class InternalEditZoneRequest
	{
		[JsonProperty("paused")]
		public bool? Paused { get; set; }

		[JsonProperty("type")]
		public ZoneType? Type { get; set; }

		[JsonProperty("vanity_name_servers")]
		public IReadOnlyCollection<string>? VanityNameServers { get; set; }
	}
}
