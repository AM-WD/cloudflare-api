namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// The zone metadata.
	/// </summary>
	public class ZoneMetaData
	{
		/// <summary>
		/// The zone is only configured for CDN.
		/// </summary>
		[JsonProperty("cdn_only")]
		public bool CdnOnly { get; set; }

		/// <summary>
		/// Number of Custom Certificates the zone can have.
		/// </summary>
		[JsonProperty("custom_certificate_quota")]
		public int CustomCertificateQuota { get; set; }

		/// <summary>
		/// The zone is only configured for DNS.
		/// </summary>
		[JsonProperty("dns_only")]
		public bool DnsOnly { get; set; }

		/// <summary>
		/// The zone is setup with Foundation DNS.
		/// </summary>
		[JsonProperty("foundation_dns")]
		public bool FoundationDns { get; set; }

		/// <summary>
		/// Number of Page Rules a zone can have.
		/// </summary>
		[JsonProperty("page_rule_quota")]
		public int PageRuleQuota { get; set; }

		/// <summary>
		/// The zone has been flagged for phishing.
		/// </summary>
		[JsonProperty("phishing_detected")]
		public bool PhishingDetected { get; set; }

		/// <summary>
		/// Step.
		/// </summary>
		[JsonProperty("step")]
		public int Step { get; set; }
	}
}
