namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Update DNS settings request.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the <see cref="UpdateDnsSettingsRequest"/> class.
	/// </remarks>
	/// <param name="id">The zone identifier.</param>
	public class UpdateDnsSettingsRequest(string id)
	{

		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string Id { get; set; } = id;

		/// <summary>
		/// Whether to flatten all CNAME records in the zone. Note that, due to DNS limitations,
		/// a CNAME record at the zone apex will always be flattened.
		/// </summary>
		public bool? FlattenAllCnames { get; set; }

		/// <summary>
		/// Whether to enable Foundation DNS Advanced Nameservers on the zone.
		/// </summary>
		public bool? FoundationDns { get; set; }

		/// <summary>
		/// Whether to enable multi-provider DNS, which causes Cloudflare to activate the zone even when non-Cloudflare NS records exist,
		/// and to respect NS records at the zone apex during outbound zone transfers.
		/// </summary>
		public bool? MultiProvider { get; set; }

		/// <summary>
		/// Settings determining the nameservers through which the zone should be available.
		/// </summary>
		public Nameserver? Nameservers { get; set; }

		/// <summary>
		/// The time to live (TTL) of the zone's nameserver (NS) records.
		/// </summary>
		/// <value>Unit: seconds. Range: <c>30 &lt;=</c> X <c>&lt;= 86400</c></value>
		public int? NameserverTtl { get; set; }

		/// <summary>
		/// Allows a Secondary DNS zone to use (proxied) override records and CNAME flattening at the zone apex.
		/// </summary>
		public bool? SecondaryOverrides { get; set; }

		/// <summary>
		/// Components of the zone's SOA record.
		/// </summary>
		public StartOfAuthority? SOA { get; set; }

		/// <summary>
		/// Whether the zone mode is a regular or CDN/DNS only zone.
		/// </summary>
		public ZoneMode? Mode { get; set; }
	}
}
