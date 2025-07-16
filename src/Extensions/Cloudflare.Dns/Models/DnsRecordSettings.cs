namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// IP Settings for the DNS record.
	/// </summary>
	public class DnsRecordSettings
	{
		/// <summary>
		/// When enabled, only A records will be generated, and AAAA records will not be
		/// created. This setting is intended for exceptional cases. Note that this option
		/// only applies to proxied records and it has no effect on whether Cloudflare
		/// communicates with the origin using IPv4 or IPv6.
		/// </summary>
		[JsonProperty("ipv4_only")]
		public bool? IPv4Only { get; set; }

		/// <summary>
		/// When enabled, only AAAA records will be generated, and A records will not be
		/// created. This setting is intended for exceptional cases. Note that this option
		/// only applies to proxied records and it has no effect on whether Cloudflare
		/// communicates with the origin using IPv4 or IPv6.
		/// </summary>
		[JsonProperty("ipv6_only")]
		public bool? IPv6Only { get; set; }
	}
}
