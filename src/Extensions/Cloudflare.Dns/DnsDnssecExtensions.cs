using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Dns.Internals;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Extensions for <see href="https://developers.cloudflare.com/api/resources/dns/subresources/dnssec/">DNS DNSSEC records</see>.
	/// </summary>
	public static class DnsDnssecExtensions
	{
		/// <summary>
		/// Delete DNSSEC.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<string>> DeleteDnssecRecords(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.DeleteAsync<string>($"/zones/{zoneId}/dnssec", null, cancellationToken);
		}

		/// <summary>
		/// Enable or disable DNSSEC.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DNSSEC>> EditDnssecStatus(this ICloudflareClient client, EditDnssecStatusRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			var req = new InternalEditDnssecStatusRequest
			{
				DnssecMultiSigner = request.DnssecMultiSigner,
				DnssecPresigned = request.DnssecPresigned,
				DnssecUseNsec3 = request.DnssecUseNsec3,
				Status = request.Status
			};

			return client.PatchAsync<DNSSEC, InternalEditDnssecStatusRequest>($"/zones/{request.ZoneId}/dnssec", req, cancellationToken);
		}

		/// <summary>
		/// Details about DNSSEC status and configuration.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DNSSEC>> DnssecDetails(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.GetAsync<DNSSEC>($"/zones/{zoneId}/dnssec", null, cancellationToken);
		}
	}
}
