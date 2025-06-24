using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Zones.Internals;
using Newtonsoft.Json.Linq;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Extensions for <see href="https://developers.cloudflare.com/api/resources/registrar/">Registrar</see>.
	/// </summary>
	public static class RegistrarExtensions
	{
		/// <summary>
		/// Show individual domain.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account id.</param>
		/// <param name="domainName">The domain name.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<JToken>> GetDomain(this ICloudflareClient client, string accountId, string domainName, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(domainName))
				throw new ArgumentNullException(nameof(domainName));

			return client.GetAsync<JToken>($"/accounts/{accountId}/registrar/domains/{domainName}", cancellationToken: cancellationToken);
		}

		/// <summary>
		/// List domains handled by Registrar.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account id.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IReadOnlyCollection<Domain>>> ListDomains(this ICloudflareClient client, string accountId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();

			return client.GetAsync<IReadOnlyCollection<Domain>>($"/accounts/{accountId}/registrar/domains", cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Update individual domain.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<JToken>> UpdateDomain(this ICloudflareClient client, UpdateDomainRequest request, CancellationToken cancellationToken = default)
		{
			request.AccountId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(request.DomainName))
				throw new ArgumentNullException(nameof(request.DomainName));

			var req = new InternalUpdateDomainRequest
			{
				AutoRenew = request.AutoRenew,
				Locked = request.Locked,
				Privacy = request.Privacy
			};

			return client.PutAsync<JToken, InternalUpdateDomainRequest>($"/accounts/{request.AccountId}/registrar/domains/{request.DomainName}", req, cancellationToken);
		}
	}
}
