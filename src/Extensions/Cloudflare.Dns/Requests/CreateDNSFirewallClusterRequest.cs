namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to create a DNS Firewall cluster with specific configuration settings.
	/// </summary>
	public class CreateDNSFirewallClusterRequest : DNSFirewallClusterRequestBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateDNSFirewallClusterRequest"/> class.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="name">DNS Firewall cluster name.</param>
		public CreateDNSFirewallClusterRequest(string accountId, string name)
			: base(accountId)
		{
			Name = name;
		}

		/// <summary>
		/// DNS Firewall cluster name.
		/// </summary>
		public string Name { get; set; }
	}
}
