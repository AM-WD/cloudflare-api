using System;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Zones.Internals.Requests;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Extends the <see cref="ICloudflareClient"/> with methods for working with zones.
	/// </summary>
	public static class ZoneDnsSettingsExtensions
	{
		/// <summary>
		/// Show DNS settings for a zone.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ZoneDnsSetting>> ShowZoneDnsSettings(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			return client.GetAsync<ZoneDnsSetting>($"zones/{zoneId}/dns_settings", cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Update DNS settings for a zone.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="request">The update request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ZoneDnsSetting>> UpdateZoneDnsSettings(this ICloudflareClient client, UpdateDnsSettingsRequest request, CancellationToken cancellationToken = default)
		{
			request.Id.ValidateCloudflareId();

			if (request.Mode.HasValue && !Enum.IsDefined(typeof(ZoneMode), request.Mode))
				throw new ArgumentOutOfRangeException(nameof(request.Mode), request.Mode, "Value must be one of the ZoneMode enum values.");

			if (request.Nameservers != null && !Enum.IsDefined(typeof(NameserverType), request.Nameservers.Type))
				throw new ArgumentOutOfRangeException(nameof(request.Nameservers.Type), request.Nameservers.Type, "Value must be one of the NameserverType enum values.");

			if (request.NameserverTtl.HasValue && (request.NameserverTtl < 30 || 86400 < request.NameserverTtl))
				throw new ArgumentOutOfRangeException(nameof(request.NameserverTtl), request.NameserverTtl, "Value must be between 30 and 86400.");

			if (request.SOA != null)
			{
				if (request.SOA.Expire < 86400 || 2419200 < request.SOA.Expire)
					throw new ArgumentOutOfRangeException(nameof(request.SOA.Expire), request.SOA.Expire, "Value must be between 86400 and 2419200.");

				if (request.SOA.MinimumTtl < 60 || 86400 < request.SOA.MinimumTtl)
					throw new ArgumentOutOfRangeException(nameof(request.SOA.MinimumTtl), request.SOA.MinimumTtl, "Value must be between 60 and 86400.");

				if (string.IsNullOrWhiteSpace(request.SOA.PrimaryNameserver))
					throw new ArgumentNullException(nameof(request.SOA.PrimaryNameserver));

				if (request.SOA.Refresh < 600 || 86400 < request.SOA.Refresh)
					throw new ArgumentOutOfRangeException(nameof(request.SOA.Refresh), request.SOA.Refresh, "Value must be between 600 and 86400.");

				if (request.SOA.Retry < 600 || 86400 < request.SOA.Retry)
					throw new ArgumentOutOfRangeException(nameof(request.SOA.Retry), request.SOA.Retry, "Value must be between 600 and 86400.");

				if (request.SOA.Ttl < 300 || 86400 < request.SOA.Ttl)
					throw new ArgumentOutOfRangeException(nameof(request.SOA.Ttl), request.SOA.Ttl, "Value must be between 300 and 86400.");

				if (string.IsNullOrWhiteSpace(request.SOA.ZoneAdministrator))
					throw new ArgumentNullException(nameof(request.SOA.ZoneAdministrator));
			}

			var req = new InternalUpdateDnsSettingsRequest
			{
				FlattenAllCnames = request.FlattenAllCnames,
				FoundationDns = request.FoundationDns,
				Mode = request.Mode,
				MultiProvider = request.MultiProvider,
				Nameservers = request.Nameservers,
				NameserverTtl = request.NameserverTtl,
				SecondaryOverrides = request.SecondaryOverrides,
			};

			if (request.SOA != null)
			{
				req.Soa = new StartOfAuthority
				(
					expire: request.SOA.Expire,
					minimumTtl: request.SOA.MinimumTtl,
					primaryNameserver: request.SOA.PrimaryNameserver,
					refresh: request.SOA.Refresh,
					retry: request.SOA.Retry,
					ttl: request.SOA.Ttl,
					zoneAdministrator: request.SOA.ZoneAdministrator
				);
			}

			return client.PatchAsync<ZoneDnsSetting, InternalUpdateDnsSettingsRequest>($"zones/{request.Id}/dns_settings", req, cancellationToken);
		}
	}
}
