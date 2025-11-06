namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to update the reverse DNS configuration for a DNS firewall cluster.
	/// </summary>
	public class UpdateDNSFirewallClusterReverseDNSRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UpdateDNSFirewallClusterReverseDNSRequest"/> class.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="dnsFirewallId">The DNS firewall cluster identifier.</param>
		public UpdateDNSFirewallClusterReverseDNSRequest(string accountId, string dnsFirewallId)
		{
			AccountId = accountId;
			DnsFirewallId = dnsFirewallId;
		}

		/// <summary>
		/// The account identifier.
		/// </summary>
		public string AccountId { get; set; }

		/// <summary>
		/// The DNS firewall cluster identifier.
		/// </summary>
		public string DnsFirewallId { get; set; }

		/// <summary>
		/// Map of cluster IP addresses to PTR record contents.
		/// </summary>
		public IReadOnlyDictionary<string, string>? ReverseDNS { get; set; }
	}
}
