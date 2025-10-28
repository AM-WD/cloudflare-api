namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents an Access Control List (ACL) entry, which defines the allowed IP ranges for DNS zone transfers.
	/// </summary>
	public class ACL
	{
		/// <summary>
		/// The unique identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// Allowed IPv4/IPv6 address range of primary or secondary nameservers.
		/// This will be applied for the entire account.
		/// </summary>
		/// <remarks>
		/// The IP range is used to allow additional NOTIFY IPs for secondary zones and IPs Cloudflare allows AXFR/IXFR requests from for primary zones.
		/// CIDRs are limited to a maximum of /24 for IPv4 and /64 for IPv6 respectively.
		/// </remarks>
		[JsonProperty("ip_range")]
		public string? IpRange { get; set; }

		/// <summary>
		/// The name of the ACL.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }
	}
}
