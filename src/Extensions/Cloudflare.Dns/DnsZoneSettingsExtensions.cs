using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Dns.Internals;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Extensions for <see href="https://developers.cloudflare.com/api/resources/dns/subresources/settings/subresources/zone/">DNS Zone Settings</see>.
	/// </summary>
	public static class DnsZoneSettingsExtensions
	{
		/// <summary>
		/// Update DNS settings for a zone.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsZoneSettings>> UpdateDnsZoneSettings(this ICloudflareClient client, UpdateDnsZoneSettingsRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			if (request.ZoneMode.HasValue && !Enum.IsDefined(typeof(DnsZoneMode), request.ZoneMode))
				throw new ArgumentOutOfRangeException(nameof(request.ZoneMode), request.ZoneMode, "Value must be one of the ZoneMode enum values.");

			if (request.Nameservers != null && !Enum.IsDefined(typeof(DnsZoneNameserversType), request.Nameservers.Type))
				throw new ArgumentOutOfRangeException($"{nameof(request.Nameservers)}.{nameof(request.Nameservers.Type)}", request.Nameservers.Type, "Value must be one of the NameserverType enum values.");

			if (request.NameserverTtl.HasValue && (request.NameserverTtl < 30 || 86400 < request.NameserverTtl))
				throw new ArgumentOutOfRangeException(nameof(request.NameserverTtl), request.NameserverTtl, "Value must be between 30 and 86400.");

			if (request.SOA != null)
			{
				string paramNameBase = $"{nameof(request.SOA)}";

				if (request.SOA.Expire < 86400 || 2419200 < request.SOA.Expire)
					throw new ArgumentOutOfRangeException($"{paramNameBase}.{nameof(request.SOA.Expire)}", request.SOA.Expire, "Value must be between 86400 and 2419200.");

				if (request.SOA.MinimumTtl < 60 || 86400 < request.SOA.MinimumTtl)
					throw new ArgumentOutOfRangeException($"{paramNameBase}.{nameof(request.SOA.MinimumTtl)}", request.SOA.MinimumTtl, "Value must be between 60 and 86400.");

				if (string.IsNullOrWhiteSpace(request.SOA.PrimaryNameserver))
					throw new ArgumentNullException($"{paramNameBase}.{nameof(request.SOA.PrimaryNameserver)}");

				if (request.SOA.Refresh < 600 || 86400 < request.SOA.Refresh)
					throw new ArgumentOutOfRangeException($"{paramNameBase}.{nameof(request.SOA.Refresh)}", request.SOA.Refresh, "Value must be between 600 and 86400.");

				if (request.SOA.Retry < 600 || 86400 < request.SOA.Retry)
					throw new ArgumentOutOfRangeException($"{paramNameBase}.{nameof(request.SOA.Retry)}", request.SOA.Retry, "Value must be between 600 and 86400.");

				if (request.SOA.TimeToLive < 300 || 86400 < request.SOA.TimeToLive)
					throw new ArgumentOutOfRangeException($"{paramNameBase}.{nameof(request.SOA.TimeToLive)}", request.SOA.TimeToLive, "Value must be between 300 and 86400.");

				if (string.IsNullOrWhiteSpace(request.SOA.ZoneAdministrator))
					throw new ArgumentNullException($"{paramNameBase}.{nameof(request.SOA.ZoneAdministrator)}");
			}

			var req = new InternalUpdateDnsZoneSettingsRequest
			{
				FlattenAllCnames = request.FlattenAllCnames,
				FoundationDns = request.FoundationDns,
				InternalDns = request.InternalDns,
				MultiProvider = request.MultiProvider,
				Nameservers = request.Nameservers,
				NameserverTtl = request.NameserverTtl,
				SecondaryOverrides = request.SecondaryOverrides,
				SOA = request.SOA,
				ZoneMode = request.ZoneMode
			};

			return client.PatchAsync<DnsZoneSettings, InternalUpdateDnsZoneSettingsRequest>($"/zones/{request.ZoneId}/dns_settings", req, cancellationToken);
		}

		/// <summary>
		/// Show DNS settings for a zone.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsZoneSettings>> ShowDnsZoneSettings(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.GetAsync<DnsZoneSettings>($"/zones/{zoneId}/dns_settings", null, cancellationToken);
		}
	}
}
