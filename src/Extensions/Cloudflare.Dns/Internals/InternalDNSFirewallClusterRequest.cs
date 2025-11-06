namespace AMWD.Net.Api.Cloudflare.Dns.Internals
{
	internal class InternalDNSFirewallClusterRequest
	{
		[JsonProperty("name")]
		public string? Name { get; set; }

		[JsonProperty("upstream_ips")]
		public IReadOnlyCollection<string>? UpstreamIps { get; set; }

		[JsonProperty("attack_mitigation")]
		public AttackMitigation? AttackMitigation { get; set; }

		[JsonProperty("deprecate_any_requests")]
		public bool? DeprecateAnyRequests { get; set; }

		[JsonProperty("ecs_fallback")]
		public bool? EcsFallback { get; set; }

		[JsonProperty("maximum_cache_ttl")]
		public int? MaximumCacheTtl { get; set; }

		[JsonProperty("minimum_cache_ttl")]
		public int? MinimumCacheTtl { get; set; }

		[JsonProperty("negative_cache_ttl")]
		public int? NegativeCacheTtl { get; set; }

		[JsonProperty("ratelimit")]
		public int? RateLimit { get; set; }

		[JsonProperty("retries")]
		public int? Retries { get; set; }
	}
}
