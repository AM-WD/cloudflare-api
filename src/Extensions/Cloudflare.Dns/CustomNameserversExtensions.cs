using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Dns.Internals;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Extensions for <see href="https://developers.cloudflare.com/api/resources/custom_nameservers/">Account Custom Nameservers</see>.
	/// </summary>
	public static class CustomNameserversExtensions
	{
		/// <summary>
		/// Add Account Custom Nameserver.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<CustomNameserver>> AddCustomNameserver(this ICloudflareClient client, AddCustomNameserverRequest request, CancellationToken cancellationToken = default)
		{
			request.AccountId.ValidateCloudflareId();

			var req = new InternalAddCustomNameserverRequest
			{
				NameserverName = request.NameserverName,
				NameserverSet = request.NameserverSet
			};

			return client.PostAsync<CustomNameserver, InternalAddCustomNameserverRequest>($"/accounts/{request.AccountId}/custom_ns", req, null, cancellationToken);
		}

		/// <summary>
		/// Delete Account Custom Nameserver.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="nameserverId">The nameserver identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IReadOnlyCollection<string>>> DeleteCustomNameserver(this ICloudflareClient client, string accountId, string nameserverId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(nameserverId))
				throw new ArgumentNullException(nameof(nameserverId));

			return client.DeleteAsync<IReadOnlyCollection<string>>($"/accounts/{accountId}/custom_ns/{nameserverId}", null, cancellationToken);
		}

		/// <summary>
		/// List an account's custom nameservers.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IReadOnlyCollection<CustomNameserver>>> ListCustomNameserver(this ICloudflareClient client, string accountId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();

			return client.GetAsync<IReadOnlyCollection<CustomNameserver>>($"/accounts/{accountId}/custom_ns", null, cancellationToken);
		}
	}
}
