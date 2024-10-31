namespace AMWD.Net.Api.Cloudflare.Zones.Internals.Requests
{
	internal class InternalUpdateDnsSettingsRequest
	{
		[JsonProperty("flatten_all_cnames")]
		public bool? FlattenAllCnames { get; set; }

		[JsonProperty("foundation_dns")]
		public bool? FoundationDns { get; set; }

		[JsonProperty("multi_provider")]
		public bool? MultiProvider { get; set; }

		[JsonProperty("nameservers")]
		public Nameserver? Nameservers { get; set; }

		[JsonProperty("ns_ttl")]
		public int? NameserverTtl { get; set; }

		[JsonProperty("secondary_overrides")]
		public bool? SecondaryOverrides { get; set; }

		[JsonProperty("soa")]
		public StartOfAuthority? Soa { get; set; }

		[JsonProperty("zone_mode")]
		public ZoneMode? Mode { get; set; }
	}
}
