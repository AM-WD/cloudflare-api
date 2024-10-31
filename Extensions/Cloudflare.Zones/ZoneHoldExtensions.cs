using System;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Zones.Internals.Filters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Extends the <see cref="ICloudflareClient"/> with methods for working with zones.
	/// </summary>
	public static class ZoneHoldExtensions
	{
		/// <summary>
		/// Retrieve whether the zone is subject to a zone hold, and metadata about the hold.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ZoneHold>> GetZoneHold(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			return client.GetAsync<ZoneHold>($"zones/{zoneId}/hold", cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Enforce a zone hold on the zone, blocking the creation and activation of zones with this zone's hostname.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="includeSubdomains">
		/// If set, the zone hold will extend to block any subdomain of the given zone, as well as SSL4SaaS Custom Hostnames.
		/// For example, a zone hold on a zone with the hostname 'example.com' and <paramref name="includeSubdomains"/>=<see langword="true"/> will block 'example.com', 'staging.example.com', 'api.staging.example.com', etc.
		/// </param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ZoneHold>> CreateZoneHold(this ICloudflareClient client, string zoneId, bool includeSubdomains = false, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			var filter = new CreateZoneHoldFilter
			{
				IncludeSubdomains = includeSubdomains
			};
			return client.PostAsync<ZoneHold, object>($"zones/{zoneId}/hold", null, filter, cancellationToken);
		}

		/// <summary>
		/// Stop enforcement of a zone hold on the zone, permanently or temporarily, allowing the creation and activation of zones with this zone's hostname.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="holdAfter">
		/// If <paramref name="holdAfter"/> is provided, the hold will be temporarily disabled, then automatically re-enabled by the system at the time specified.
		/// Otherwise, the hold will be disabled indefinitely.
		/// </param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ZoneHold>> DeleteZoneHold(this ICloudflareClient client, string zoneId, DateTime? holdAfter = null, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			var filter = new DeleteZoneHoldFilter
			{
				HoldAfter = holdAfter
			};
			return client.DeleteAsync<ZoneHold>($"zones/{zoneId}/hold", filter, cancellationToken);
		}
	}
}
