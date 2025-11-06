namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents the response data for a DNS Firewall configuration on cloudflare.
	/// </summary>
	public class DnsFirewallCluster
	{
		/// <summary>
		/// The identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// Whether to refuse to answer queries for the ANY type.
		/// </summary>
		[JsonProperty("deprecate_any_requests")]
		public bool? DeprecateAnyRequests { get; set; }

		/// <summary>
		/// List of IPs used by DNS Firewall cluster.
		/// </summary>
		[JsonProperty("dns_firewall_ips")]
		public IReadOnlyCollection<string>? DnsFirewallIps { get; set; }

		/// <summary>
		/// Whether to forward client IP (resolver) subnet if no EDNS Client Subnet is sent.
		/// </summary>
		[JsonProperty("ecs_fallback")]
		public bool? EcsFallback { get; set; }

		/// <summary>
		/// <para>
		/// By default, Cloudflare attempts to cache responses for as long as indicated by
		/// the TTL received from upstream nameservers.This setting sets an upper bound on
		/// this duration.For caching purposes, higher TTLs will be decreased to the
		/// maximum value defined by this setting.
		/// </para>
		/// <para>
		/// This setting does not affect the TTL value in the DNS response Cloudflare
		/// returns to clients.Cloudflare will always forward the TTL value received from
		/// upstream nameservers.
		/// </para>
		/// </summary>
		[JsonProperty("maximum_cache_ttl")]
		public int? MaximumCacheTtl { get; set; }

		/// <summary>
		/// <para>
		/// By default, Cloudflare attempts to cache responses for as long as indicated by
		/// the TTL received from upstream nameservers.This setting sets a lower bound on
		/// this duration.For caching purposes, lower TTLs will be increased to the minimum
		/// value defined by this setting.
		/// </para>
		/// <para>
		/// This setting does not affect the TTL value in the DNS response Cloudflare
		/// returns to clients.Cloudflare will always forward the TTL value received from
		/// upstream nameservers.
		/// </para>
		/// </summary>
		/// <remarks>
		/// Note that, even with this setting, there is no guarantee that a response will be
		/// cached for at least the specified duration.Cached responses may be removed
		/// earlier for capacity or other operational reasons.
		/// </remarks>
		[JsonProperty("minimum_cache_ttl")]
		public int? MinimumCacheTtl { get; set; }

		/// <summary>
		/// Last modification of DNS Firewall cluster
		/// </summary>
		[JsonProperty("modified_on")]
		public DateTime? ModifiedOn { get; set; }

		/// <summary>
		/// DNS Firewall cluster name.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }

		/// <summary>
		/// <para>
		/// This setting controls how long DNS Firewall should cache negative responses
		/// (e.g., NXDOMAIN) from the upstream servers.
		/// </para>
		/// <para>
		/// This setting does not affect the TTL value in the DNS response Cloudflare
		/// returns to clients.Cloudflare will always forward the TTL value received from
		/// upstream nameservers.
		/// </para>
		/// </summary>
		[JsonProperty("negative_cache_ttl")]
		public int? NegativeCacheTtl { get; set; }

		/// <summary>
		/// Ratelimit in queries per second per datacenter
		/// (applies to DNS queries sent to the upstream nameservers configured on the cluster).
		/// </summary>
		[JsonProperty("ratelimit")]
		public int? RateLimit { get; set; }

		/// <summary>
		/// Number of retries for fetching DNS responses from upstream nameservers
		/// (not counting the initial attempt).
		/// </summary>
		[JsonProperty("retries")]
		public int? Retries { get; set; }

		/// <summary>
		/// Upstream DNS server IPs.
		/// </summary>
		[JsonProperty("upstream_ips")]
		public IReadOnlyCollection<string>? UpstreamIps { get; set; }

		/// <summary>
		/// Attack mitigation settings.
		/// </summary>
		[JsonProperty("attack_mitigation")]
		public AttackMitigation? AttackMitigation { get; set; }
	}

	/// <summary>
	/// Attack mitigation settings.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/dns-firewall/dns-firewall.ts#L154">Source</see>
	/// </summary>
	public class AttackMitigation
	{
		/// <summary>
		/// When enabled, automatically mitigate random-prefix attacks to protect upstream DNS servers.
		/// </summary>
		[JsonProperty("enabled")]
		public bool? Enabled { get; set; }

		/// <summary>
		/// Only mitigate attacks when upstream servers seem unhealthy.
		/// </summary>
		[JsonProperty("only_when_upstream_unhealthy")]
		public bool? OnlyWhenUpstreamUnhealthy { get; set; }
	}
}
