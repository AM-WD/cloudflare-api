namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// The DNS settings.
	/// </summary>
	public class ZoneDnsSetting
	{
		/// <summary>
		/// Whether to flatten all CNAME records in the zone. Note that, due to DNS limitations,
		/// a CNAME record at the zone apex will always be flattened.
		/// </summary>
		[JsonProperty("flatten_all_cnames")]
		public bool FlattenAllCnames { get; set; }

		/// <summary>
		/// Whether to enable Foundation DNS Advanced Nameservers on the zone.
		/// </summary>
		[JsonProperty("foundation_dns")]
		public bool FoundationDns { get; set; }

		/// <summary>
		/// Whether to enable multi-provider DNS, which causes Cloudflare to activate the zone even when non-Cloudflare NS records exist,
		/// and to respect NS records at the zone apex during outbound zone transfers.
		/// </summary>
		[JsonProperty("multi_provider")]
		public bool MultiProvider { get; set; }

		/// <summary>
		/// Settings determining the nameservers through which the zone should be available.
		/// </summary>
		[JsonProperty("nameservers")]
		public Nameserver? Nameservers { get; set; }

		/// <summary>
		/// The time to live (TTL) of the zone's nameserver (NS) records.
		/// </summary>
		// 30 <= X <= 86400
		[JsonProperty("ns_ttl")]
		public int NameserverTtl { get; set; }

		/// <summary>
		/// Allows a Secondary DNS zone to use (proxied) override records and CNAME flattening at the zone apex.
		/// </summary>
		[JsonProperty("secondary_overrides")]
		public bool SecondaryOverrides { get; set; }

		/// <summary>
		/// Components of the zone's SOA record.
		/// </summary>
		[JsonProperty("soa")]
		public StartOfAuthority? SOA { get; set; }

		/// <summary>
		/// Whether the zone mode is a regular or CDN/DNS only zone.
		/// </summary>
		[JsonProperty("zone_mode")]
		public ZoneMode Mode { get; set; }
	}
}
