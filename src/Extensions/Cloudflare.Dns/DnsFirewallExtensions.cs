using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Dns.Internals;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Extensions for the <see href="https://developers.cloudflare.com/api/resources/dns_firewall/">DNS Firewall</see>.
	/// </summary>
	public static class DnsFirewallExtensions
	{
		/// <summary>
		/// List DNS Firewall clusters for an account.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="options">Filter options (optional).</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IReadOnlyCollection<DnsFirewallCluster>>> ListDNSFirewallClusters(this ICloudflareClient client, string accountId, ListDNSFirewallClustersFilter? options = null, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();

			return client.GetAsync<IReadOnlyCollection<DnsFirewallCluster>>($"/accounts/{accountId}/dns_firewall", options, cancellationToken);
		}

		/// <summary>
		/// Show a single DNS Firewall cluster for an account.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="dnsFirewallId">The DNS firewall identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsFirewallCluster>> DNSFirewallClusterDetails(this ICloudflareClient client, string accountId, string dnsFirewallId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();
			dnsFirewallId.ValidateCloudflareId();

			return client.GetAsync<DnsFirewallCluster>($"/accounts/{accountId}/dns_firewall/{dnsFirewallId}", null, cancellationToken);
		}

		/// <summary>
		/// Create a DNS Firewall cluster.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsFirewallCluster>> CreateDNSFirewallCluster(this ICloudflareClient client, CreateDNSFirewallClusterRequest request, CancellationToken cancellationToken = default)
		{
			request.AccountId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ArgumentException("DNS Firewall cluster name must be provided.", nameof(request.Name));

			request.Name = request.Name.Trim();

			if (request.Name.Length > 160)
				throw new ArgumentException("DNS Firewall cluster name must not exceed 160 characters.", nameof(request.Name));

			if (request.MaximumCacheTtl.HasValue && (request.MaximumCacheTtl < 30 || 36000 < request.MaximumCacheTtl))
				throw new ArgumentOutOfRangeException(nameof(request.MaximumCacheTtl), "Maximum cache TTL must be between 30 and 36000.");

			if (request.MinimumCacheTtl.HasValue && (request.MinimumCacheTtl < 30 || 36000 < request.MinimumCacheTtl))
				throw new ArgumentOutOfRangeException(nameof(request.MinimumCacheTtl), "Minimum cache TTL must be between 30 and 36000.");

			if (request.NegativeCacheTtl.HasValue && (request.NegativeCacheTtl < 30 || 36000 < request.NegativeCacheTtl))
				throw new ArgumentOutOfRangeException(nameof(request.NegativeCacheTtl), "Negative cache TTL must be between 30 and 36000.");

			if (request.RateLimit.HasValue && (request.RateLimit < 100 || 1_000_000_000 < request.RateLimit))
				throw new ArgumentOutOfRangeException(nameof(request.RateLimit), "Ratelimit must be between 100 and 1,000,000,000 seconds.");

			if (request.Retries.HasValue && (request.Retries < 0 || 2 < request.Retries))
				throw new ArgumentOutOfRangeException(nameof(request.Retries), "Retries must be between 0 and 2.");

			var req = new InternalDNSFirewallClusterRequest
			{
				Name = request.Name,
				UpstreamIps = request.UpstreamIps,
				AttackMitigation = request.AttackMitigation,
				DeprecateAnyRequests = request.DeprecateAnyRequests,
				EcsFallback = request.EcsFallback,
				MaximumCacheTtl = request.MaximumCacheTtl,
				MinimumCacheTtl = request.MinimumCacheTtl,
				NegativeCacheTtl = request.NegativeCacheTtl,
				RateLimit = request.RateLimit,
				Retries = request.Retries
			};

			return client.PostAsync<DnsFirewallCluster, InternalDNSFirewallClusterRequest>($"/accounts/{request.AccountId}/dns_firewall", req, null, cancellationToken);
		}

		/// <summary>
		/// Modify the configuration of a DNS Firewall cluster.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsFirewallCluster>> UpdateDNSFirewallCluster(this ICloudflareClient client, UpdateDNSFirewallClusterRequest request, CancellationToken cancellationToken = default)
		{
			request.AccountId.ValidateCloudflareId();
			request.DnsFirewallId.ValidateCloudflareId();

			request.Name = request.Name?.Trim();

			if (request.Name?.Length > 160)
				throw new ArgumentException("DNS Firewall cluster name must not exceed 160 characters.", nameof(request.Name));

			if (request.MaximumCacheTtl.HasValue && (request.MaximumCacheTtl < 30 || 36000 < request.MaximumCacheTtl))
				throw new ArgumentOutOfRangeException(nameof(request.MaximumCacheTtl), "Maximum cache TTL must be between 30 and 36000.");

			if (request.MinimumCacheTtl.HasValue && (request.MinimumCacheTtl < 30 || 36000 < request.MinimumCacheTtl))
				throw new ArgumentOutOfRangeException(nameof(request.MinimumCacheTtl), "Minimum cache TTL must be between 30 and 36000.");

			if (request.NegativeCacheTtl.HasValue && (request.NegativeCacheTtl < 30 || 36000 < request.NegativeCacheTtl))
				throw new ArgumentOutOfRangeException(nameof(request.NegativeCacheTtl), "Negative cache TTL must be between 30 and 36000.");

			if (request.RateLimit.HasValue && (request.RateLimit < 100 || 1_000_000_000 < request.RateLimit))
				throw new ArgumentOutOfRangeException(nameof(request.RateLimit), "Ratelimit must be between 100 and 1,000,000,000 seconds.");

			if (request.Retries.HasValue && (request.Retries < 0 || 2 < request.Retries))
				throw new ArgumentOutOfRangeException(nameof(request.Retries), "Retries must be between 0 and 2.");

			var req = new InternalDNSFirewallClusterRequest
			{
				Name = request.Name,
				UpstreamIps = request.UpstreamIps,
				AttackMitigation = request.AttackMitigation,
				DeprecateAnyRequests = request.DeprecateAnyRequests,
				EcsFallback = request.EcsFallback,
				MaximumCacheTtl = request.MaximumCacheTtl,
				MinimumCacheTtl = request.MinimumCacheTtl,
				NegativeCacheTtl = request.NegativeCacheTtl,
				RateLimit = request.RateLimit,
				Retries = request.Retries
			};

			return client.PatchAsync<DnsFirewallCluster, InternalDNSFirewallClusterRequest>($"/accounts/{request.AccountId}/dns_firewall", req, cancellationToken);
		}

		/// <summary>
		/// Delete a DNS Firewall cluster.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="dnsFirewallId">The DNS firewall identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Identifier>> DeleteDNSFirewallCluster(this ICloudflareClient client, string accountId, string dnsFirewallId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();
			dnsFirewallId.ValidateCloudflareId();

			return client.DeleteAsync<Identifier>($"/accounts/{accountId}/dns_firewall/{dnsFirewallId}", null, cancellationToken);
		}
	}
}
