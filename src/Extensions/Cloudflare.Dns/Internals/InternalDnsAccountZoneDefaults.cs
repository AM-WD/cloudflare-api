namespace AMWD.Net.Api.Cloudflare.Dns.Internals
{
	internal class InternalDnsAccountZoneDefaults
	{
		[JsonProperty("flatten_all_cnames")]
		public bool? FlattenAllCnames { get; set; }

		[JsonProperty("foundation_dns")]
		public bool? FoundationDns { get; set; }

		[JsonProperty("internal_dns")]
		public DnsAccountInternalDns? InternalDns { get; set; }

		[JsonProperty("multi_provider")]
		public bool? MultiProvider { get; set; }

		[JsonProperty("nameservers")]
		public DnsAccountNameservers? Nameservers { get; set; }

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
