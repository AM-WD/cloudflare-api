using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Zones.Internals.Requests;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Extensions for security center section of a zone.
	/// </summary>
	public static class SecurityCenterExtensions
	{
		/// <summary>
		/// Delete security.txt
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static async Task<CloudflareResponse> DeleteSecurityTxt(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			return await client.DeleteAsync<object>($"zones/{zoneId}/security-center/securitytxt", cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Get security.txt.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<SecurityTxt>> GetSecurityTxt(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			return client.GetAsync<SecurityTxt>($"zones/{zoneId}/security-center/securitytxt", cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Update security.txt
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="request">The request information.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static async Task<CloudflareResponse> UpdateSecurityTxt(this ICloudflareClient client, UpdateSecurityTxtRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			var req = new InternalUpdateSecurityTxtRequest();

			if (request.Acknowledgements != null)
				req.Acknowledgements = request.Acknowledgements.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

			if (request.Canonical != null)
				req.Canonical = request.Canonical.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

			if (request.Contact != null)
				req.Contact = request.Contact.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

			req.Enabled = request.Enabled;

			if (request.Encryption != null)
				req.Encryption = request.Encryption.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

			if (request.Expires.HasValue)
				req.Expires = request.Expires.Value.ToUniversalTime();

			if (request.Hiring != null)
				req.Hiring = request.Hiring.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

			if (request.Policy != null)
				req.Policy = request.Policy.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

			if (request.PreferredLanguages != null)
				req.PreferredLanguages = string.Join(", ", request.PreferredLanguages.Where(s => !string.IsNullOrWhiteSpace(s)));

			return await client.PutAsync<object, InternalUpdateSecurityTxtRequest>($"zones/{request.ZoneId}/security-center/securitytxt", req, cancellationToken);
		}
	}
}
