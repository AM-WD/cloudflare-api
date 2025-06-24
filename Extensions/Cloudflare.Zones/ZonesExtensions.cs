using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Zones.Internals;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Extensions for <see href="https://developers.cloudflare.com/api/resources/zones/">Zones</see>.
	/// </summary>
	public static class ZonesExtensions
	{
		/// <summary>
		/// Create Zone.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Zone>> CreateZone(this ICloudflareClient client, CreateZoneRequest request, CancellationToken cancellationToken = default)
		{
			request.AccountId?.ValidateCloudflareId();
			request.Name.ValidateLength(253, nameof(request.Name));

			if (request.Type.HasValue && !Enum.IsDefined(typeof(ZoneType), request.Type.Value))
				throw new ArgumentOutOfRangeException(nameof(request.Type));

			var req = new InternalCreateZoneRequest(
				request.AccountId,
				request.Name,
				request.Type
			);

			return client.PostAsync<Zone, InternalCreateZoneRequest>($"/zones", req, cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Deletes an existing zone.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Identifier>> DeleteZone(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.DeleteAsync<Identifier>($"/zones/{zoneId}", cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Edits a zone.
		/// </summary>
		/// <remarks>
		/// <strong>Only one zone property can be changed at a time.</strong>
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Zone>> EditZone(this ICloudflareClient client, EditZoneRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			if (request.Paused.HasValue && (request.Type.HasValue || request.VanityNameServers != null))
				throw new CloudflareException("Only one zone property can be changed at a time.");

			if (request.Type.HasValue && request.VanityNameServers != null)
				throw new CloudflareException("Only one zone property can be changed at a time.");

			if (request.Type.HasValue && !Enum.IsDefined(typeof(ZoneType), request.Type.Value))
				throw new ArgumentOutOfRangeException(nameof(request.Type));

			var req = new InternalEditZoneRequest
			{
				Paused = request.Paused,
				Type = request.Type
			};

			if (request.VanityNameServers != null)
				req.VanityNameServers = request.VanityNameServers.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

			return client.PatchAsync<Zone, InternalEditZoneRequest>($"/zones/{request.ZoneId}", req, cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Zone Details.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Zone>> ZoneDetails(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.GetAsync<Zone>($"/zones/{zoneId}", cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Lists, searches, sorts, and filters your zones.
		/// </summary>
		/// <remarks>
		/// Listing zones across more than 500 accounts is currently not allowed.
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="options">Filter options.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IReadOnlyCollection<Zone>>> ListZones(this ICloudflareClient client, ListZonesFilter? options = null, CancellationToken cancellationToken = default)
		{
			return client.GetAsync<IReadOnlyCollection<Zone>>($"/zones", queryFilter: options, cancellationToken: cancellationToken);
		}
	}
}
