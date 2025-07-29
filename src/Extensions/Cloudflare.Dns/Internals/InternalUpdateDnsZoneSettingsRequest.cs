namespace AMWD.Net.Api.Cloudflare.Dns.Internals
{
	internal class InternalUpdateDnsZoneSettingsRequest
	{
		[JsonProperty("flatten_all_cnames")]
		public bool? FlattenAllCnames { get; set; }

		[JsonProperty("foundation_dns")]
		public bool? FoundationDns { get; set; }

		[JsonProperty("internal_dns")]
		public DnsZoneInternalDns? InternalDns { get; set; }

		[JsonProperty("multi_provider")]
		public bool? MultiProvider { get; set; }

		[JsonProperty("nameservers")]
		public DnsZoneNameservers? Nameservers { get; set; }

		[JsonProperty("ns_ttl")]
		public int? NameserverTtl { get; set; }

		[JsonProperty("secondary_overrides")]
		public bool? SecondaryOverrides { get; set; }

		[JsonProperty("soa")]
		public DnsZoneSoa? SOA { get; set; }

		[JsonProperty("zone_mode")]
		public DnsZoneMode? ZoneMode { get; set; }
	}
}
