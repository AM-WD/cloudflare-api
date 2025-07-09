using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Zones.Internals;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Extensions for <see href="https://developers.cloudflare.com/api/resources/zones/subresources/subscriptions/">Zone Subscriptions</see>.
	/// </summary>
	public static class ZoneSubscriptionsExtensions
	{
		/// <summary>
		/// Create a zone subscription, either plan or add-ons.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Subscription>> CreateZoneSubscription(this ICloudflareClient client, CreateZoneSubscriptionRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			var req = new InternalCreateZoneSubscriptionRequest
			{
				Frequency = request.Frequency,
				RatePlan = request.RatePlan
			};

			return client.PostAsync<Subscription, InternalCreateZoneSubscriptionRequest>($"/zones/{request.ZoneId}/subscription", req, null, cancellationToken);
		}

		/// <summary>
		/// Lists zone subscription details.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Subscription>> ZoneSubscriptionDetails(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.GetAsync<Subscription>($"/zones/{zoneId}/subscription", null, cancellationToken);
		}

		/// <summary>
		/// Updates zone subscriptions, either plan or add-ons.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Subscription>> UpdateZoneSubscription(this ICloudflareClient client, UpdateZoneSubscriptionRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			var req = new InternalUpdateZoneSubscriptionRequest
			{
				Frequency = request.Frequency,
				RatePlan = request.RatePlan
			};

			return client.PutAsync<Subscription, InternalUpdateZoneSubscriptionRequest>($"/zones/{request.ZoneId}/subscription", req, cancellationToken);
		}
	}
}
