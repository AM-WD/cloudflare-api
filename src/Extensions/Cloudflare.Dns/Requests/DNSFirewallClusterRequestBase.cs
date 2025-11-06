namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to create a DNS Firewall cluster with specific configuration settings.
	/// </summary>
	public abstract class DNSFirewallClusterRequestBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateDNSFirewallClusterRequest"/> class.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		public DNSFirewallClusterRequestBase(string accountId)
		{
			AccountId = accountId;
		}

		/// <summary>
		/// The account identifier.
		/// </summary>
		public string AccountId { get; set; }

		/// <summary>
		/// Upstream DNS server IPs.
		/// </summary>
		public IReadOnlyCollection<string>? UpstreamIps { get; set; }

		/// <summary>
		/// Attack mitigation settings.
		/// </summary>
		public AttackMitigation? AttackMitigation { get; set; }

		/// <summary>
		/// Whether to refuse to answer queries for the ANY type.
		/// </summary>
		public bool? DeprecateAnyRequests { get; set; }

		/// <summary>
		/// Whether to forward client IP (resolver) subnet if no EDNS Client Subnet is sent.
		/// </summary>
		public bool? EcsFallback { get; set; }

		/// <summary>
		/// <para>
		/// By default, Cloudflare attempts to cache responses for as long as indicated by the TTL received from upstream nameservers.
		/// This setting sets an upper bound on this duration.
		/// For caching purposes, higher TTLs will be decreased to the maximum value defined by this setting.
		/// </para>
		/// <para>
		/// This setting does not affect the TTL value in the DNS response Cloudflare returns to clients.
		/// Cloudflare will always forward the TTL value received from upstream nameservers.
		/// </para>
		/// </summary>
		public int? MaximumCacheTtl { get; set; }

		/// <summary>
		/// <para>
		/// By default, Cloudflare attempts to cache responses for as long as indicated by the TTL received from upstream nameservers.
		/// This setting sets a lower bound on this duration.
		/// For caching purposes, lower TTLs will be increased to the minimum value defined by this setting.
		/// </para>
		/// <para>
		/// This setting does not affect the TTL value in the DNS response Cloudflare returns to clients.
		/// Cloudflare will always forward the TTL value received from upstream nameservers.
		/// </para>
		/// </summary>
		/// <remarks>
		/// Note that, even with this setting, there is no guarantee that a response will be cached for at least the specified duration.
		/// Cached responses may be removed earlier for capacity or other operational reasons.
		/// </remarks>
		public int? MinimumCacheTtl { get; set; }

		/// <summary>
		/// <para>
		/// This setting controls how long DNS Firewall should cache negative responses (e.g., NXDOMAIN) from the upstream servers.
		/// </para>
		/// <para>
		/// This setting does not affect the TTL value in the DNS response Cloudflare returns to clients.
		/// Cloudflare will always forward the TTL value received from upstream nameservers.
		/// </para>
		/// </summary>
		public int? NegativeCacheTtl { get; set; }

		/// <summary>
		/// Ratelimit in queries per second per datacenter (applies to DNS queries sent to the upstream nameservers configured on the cluster).
		/// </summary>
		public int? RateLimit { get; set; }

		/// <summary>
		/// Number of retries for fetching DNS responses from upstream nameservers (not counting the initial attempt).
		/// </summary>
		public int? Retries { get; set; }
	}
}
