using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Zones.Zones.InternalRequests;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Extends the <see cref="ICloudflareClient"/> with methods for working with zones.
	/// </summary>
	public static class ZoneExtensions
	{
		/// <summary>
		/// Lists, searches, sorts, and filters your zones.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="options">Filter options (optional).</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IReadOnlyList<Zone>>> ListZones(this ICloudflareClient client, ListZonesFilter options = null, CancellationToken cancellationToken = default)
		{
			return client.GetAsync<IReadOnlyList<Zone>>("zones", options, cancellationToken);
		}

		/// <summary>
		/// Get details for a zone.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Zone>> ZoneDetails(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			return client.GetAsync<Zone>($"zones/{zoneId}", cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Create a new zone.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="request">The request information.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Zone>> CreateZone(this ICloudflareClient client, CreateZoneRequest request, CancellationToken cancellationToken = default)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

			request.AccountId.ValidateCloudflareId();
			request.Name.ValidateCloudflareName();

			if (!RegexPatterns.ZoneName.IsMatch(request.Name))
				throw new ArgumentException("Does not match the zone name pattern", nameof(request.Name));

			if (!Enum.IsDefined(typeof(ZoneType), request.Type))
				throw new ArgumentOutOfRangeException(nameof(request.Type));

			var req = new CreateRequest
			{
				Account = new AccountBase { Id = request.AccountId },
				Name = request.Name,
				Type = request.Type
			};

			return client.PostAsync<Zone, CreateRequest>("zones", req, cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Deletes an existing zone.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ZoneIdResponse>> DeleteZone(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			return client.DeleteAsync<ZoneIdResponse>($"zones/{zoneId}", cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Edits a zone.
		/// </summary>
		/// <remarks>
		/// Only one zone property can be changed at a time.
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="request">The request information.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Zone>> EditZone(this ICloudflareClient client, EditZoneRequest request, CancellationToken cancellationToken = default)
		{
			request.Id.ValidateCloudflareId();

			if (request.Type.HasValue && request.VanityNameServers != null)
				throw new CloudflareException("Only one zone property can be changed at a time.");

			if (request.Type.HasValue && !Enum.IsDefined(typeof(ZoneType), request.Type.Value))
				throw new ArgumentOutOfRangeException(nameof(request.Type));

			var req = new EditRequest();

			if (request.Type.HasValue)
				req.Type = request.Type.Value;

			if (request.VanityNameServers != null)
				req.VanityNameServers = request.VanityNameServers.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

			return client.PatchAsync<Zone, EditRequest>($"zones/{request.Id}", req, cancellationToken);
		}

		// Triggeres a new activation check for a PENDING Zone. This can be triggered every 5 min for paygo/ent customers, every hour for FREE Zones.

		/// <summary>
		/// Triggeres a new activation check for a <see cref="ZoneStatus.Pending"/> zone.
		/// </summary>
		/// <remarks>
		/// This can be triggered every 5 min for paygo/enterprise customers, every hour for FREE Zones.
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ZoneIdResponse>> RerunActivationCheck(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			return client.PutAsync<ZoneIdResponse, object>($"zones/{zoneId}/activation_check", null, cancellationToken);
		}
	}
}
