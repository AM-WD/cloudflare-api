using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Zones.Internals;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Extensions for <see href="https://developers.cloudflare.com/api/resources/zones/subresources/holds/">Zone Holds</see>.
	/// </summary>
	public static class ZoneHoldsExtensions
	{
		/// <summary>
		/// Enforce a zone hold on the zone, blocking the creation and activation of zones with this zone's hostname.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ZoneHold>> CreateZoneHold(this ICloudflareClient client, CreateZoneHoldRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			var filter = new InternalCreateZoneHoldFilter
			{
				IncludeSubdomains = request.IncludeSubdomains
			};

			return client.PostAsync<ZoneHold, object>($"/zones/{request.ZoneId}/hold", null, filter, cancellationToken);
		}

		/// <summary>
		/// Stop enforcement of a zone hold on the zone, permanently or temporarily,
		/// allowing the creation and activation of zones with this zone's hostname.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ZoneHold>> RemoveZoneHold(this ICloudflareClient client, RemoveZoneHoldRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			var filter = new InternalRemoveZoneHoldFilter
			{
				HoldAfter = request.HoldAfter
			};

			return client.DeleteAsync<ZoneHold>($"/zones/{request.ZoneId}/hold", filter, cancellationToken);
		}

		/// <summary>
		/// Update the <see cref="UpdateZoneHoldRequest.HoldAfter"/> and/or <see cref="UpdateZoneHoldRequest.IncludeSubdomains"/> values on an existing zone hold.
		/// The hold is enabled if the <see cref="UpdateZoneHoldRequest.HoldAfter"/> is in the past.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ZoneHold>> UpdateZoneHold(this ICloudflareClient client, UpdateZoneHoldRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			var req = new InternalUpdateZoneHoldRequest
			{
				HoldAfter = request.HoldAfter,
				IncludeSubdomains = request.IncludeSubdomains
			};

			return client.PatchAsync<ZoneHold, InternalUpdateZoneHoldRequest>($"/zones/{request.ZoneId}/hold", req, cancellationToken);
		}

		/// <summary>
		/// Retrieve whether the zone is subject to a zone hold, and metadata about the hold.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ZoneHold>> GetZoneHold(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.GetAsync<ZoneHold>($"/zones/{zoneId}/hold", cancellationToken: cancellationToken);
		}
	}
}
