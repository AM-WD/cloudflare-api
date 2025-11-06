namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to create a DNS Firewall cluster with specific configuration settings.
	/// </summary>
	public class UpdateDNSFirewallClusterRequest : DNSFirewallClusterRequestBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateDNSFirewallClusterRequest"/> class.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="dnsFirewallId">DNS Firewall cluster name.</param>
		public UpdateDNSFirewallClusterRequest(string accountId, string dnsFirewallId)
			: base(accountId)
		{
			DnsFirewallId = dnsFirewallId;
		}

		/// <summary>
		/// The DNS firewall cluster identifier.
		/// </summary>
		public string DnsFirewallId { get; set; }

		/// <summary>
		/// DNS Firewall cluster name.
		/// </summary>
		public string? Name { get; set; }
	}
}
