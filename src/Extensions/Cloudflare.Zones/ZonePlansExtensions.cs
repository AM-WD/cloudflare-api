using System.Threading;
using System.Threading.Tasks;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Extensions for <see href="https://developers.cloudflare.com/api/resources/zones/subresources/plans/">Zone Plans</see>.
	/// </summary>
	public static class ZonePlansExtensions
	{
		/// <summary>
		/// Details of the available plan that the zone can subscribe to.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="planId">The plan identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<AvailableRatePlan>> AvailablePlanDetails(this ICloudflareClient client, string zoneId, string planId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			planId.ValidateCloudflareId();

			return client.GetAsync<AvailableRatePlan>($"/zones/{zoneId}/available_plans/{planId}", cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Lists available plans the zone can subscribe to.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IReadOnlyCollection<AvailableRatePlan>>> ListAvailablePlans(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.GetAsync<IReadOnlyCollection<AvailableRatePlan>>($"/zones/{zoneId}/available_plans", cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Lists all rate plans the zone can subscribe to.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<RatePlanGetResponse>> ListAvailableRatePlans(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.GetAsync<RatePlanGetResponse>($"/zones/{zoneId}/available_rate_plans", cancellationToken: cancellationToken);
		}
	}
}
