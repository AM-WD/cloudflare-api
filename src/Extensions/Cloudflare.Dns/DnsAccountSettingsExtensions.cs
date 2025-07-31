using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Dns.Internals;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Extensions for <see href="https://developers.cloudflare.com/api/resources/dns/subresources/settings/subresources/account/">DNS Account Settings</see>.
	/// </summary>
	public static class DnsAccountSettingsExtensions
	{
		/// <summary>
		/// Update DNS settings for a zone default of an account.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsAccountSettings>> UpdateDnsAccountSettings(this ICloudflareClient client, UpdateDnsAccountSettingsRequest request, CancellationToken cancellationToken = default)
		{
			request.AccountId.ValidateCloudflareId();

			var req = new InternalUpdateDnsAccountSettingsRequest();

			if (request.ZoneDefaults != null)
			{
				req.ZoneDefaults = new InternalDnsAccountZoneDefaults();

				if (request.ZoneDefaults.ZoneMode.HasValue && !Enum.IsDefined(typeof(DnsZoneMode), request.ZoneDefaults.ZoneMode))
					throw new ArgumentOutOfRangeException($"{nameof(request.ZoneDefaults)}.{nameof(request.ZoneDefaults.ZoneMode)}", request.ZoneDefaults.ZoneMode, "Value must be one of the ZoneMode enum values.");

				if (request.ZoneDefaults.Nameservers != null && !Enum.IsDefined(typeof(DnsAccountNameserversType), request.ZoneDefaults.Nameservers.Type))
					throw new ArgumentOutOfRangeException($"{nameof(request.ZoneDefaults)}.{nameof(request.ZoneDefaults.Nameservers)}.{nameof(request.ZoneDefaults.Nameservers.Type)}", request.ZoneDefaults.Nameservers.Type, "Value must be one of the NameserverType enum values.");

				if (request.ZoneDefaults.NameserverTtl.HasValue && (request.ZoneDefaults.NameserverTtl < 30 || 86400 < request.ZoneDefaults.NameserverTtl))
					throw new ArgumentOutOfRangeException($"{nameof(request.ZoneDefaults)}.{nameof(request.ZoneDefaults.NameserverTtl)}", request.ZoneDefaults.NameserverTtl, "Value must be between 30 and 86400.");

				if (request.ZoneDefaults.SOA != null)
				{
					string paramNameBase = $"{nameof(request.ZoneDefaults)}.{nameof(request.ZoneDefaults.SOA)}";

					if (request.ZoneDefaults.SOA.Expire < 86400 || 2419200 < request.ZoneDefaults.SOA.Expire)
						throw new ArgumentOutOfRangeException($"{paramNameBase}.{nameof(request.ZoneDefaults.SOA.Expire)}", request.ZoneDefaults.SOA.Expire, "Value must be between 86400 and 2419200.");

					if (request.ZoneDefaults.SOA.MinimumTtl < 60 || 86400 < request.ZoneDefaults.SOA.MinimumTtl)
						throw new ArgumentOutOfRangeException($"{paramNameBase}.{nameof(request.ZoneDefaults.SOA.MinimumTtl)}", request.ZoneDefaults.SOA.MinimumTtl, "Value must be between 60 and 86400.");

					if (string.IsNullOrWhiteSpace(request.ZoneDefaults.SOA.PrimaryNameserver))
						throw new ArgumentNullException($"{paramNameBase}.{nameof(request.ZoneDefaults.SOA.PrimaryNameserver)}");

					if (request.ZoneDefaults.SOA.Refresh < 600 || 86400 < request.ZoneDefaults.SOA.Refresh)
						throw new ArgumentOutOfRangeException($"{paramNameBase}.{nameof(request.ZoneDefaults.SOA.Refresh)}", request.ZoneDefaults.SOA.Refresh, "Value must be between 600 and 86400.");

					if (request.ZoneDefaults.SOA.Retry < 600 || 86400 < request.ZoneDefaults.SOA.Retry)
						throw new ArgumentOutOfRangeException($"{paramNameBase}.{nameof(request.ZoneDefaults.SOA.Retry)}", request.ZoneDefaults.SOA.Retry, "Value must be between 600 and 86400.");

					if (request.ZoneDefaults.SOA.TimeToLive < 300 || 86400 < request.ZoneDefaults.SOA.TimeToLive)
						throw new ArgumentOutOfRangeException($"{paramNameBase}.{nameof(request.ZoneDefaults.SOA.TimeToLive)}", request.ZoneDefaults.SOA.TimeToLive, "Value must be between 300 and 86400.");

					if (string.IsNullOrWhiteSpace(request.ZoneDefaults.SOA.ZoneAdministrator))
						throw new ArgumentNullException($"{paramNameBase}.{nameof(request.ZoneDefaults.SOA.ZoneAdministrator)}");
				}

				req.ZoneDefaults.FlattenAllCnames = request.ZoneDefaults.FlattenAllCnames;
				req.ZoneDefaults.FoundationDns = request.ZoneDefaults.FoundationDns;
				req.ZoneDefaults.InternalDns = request.ZoneDefaults.InternalDns;
				req.ZoneDefaults.MultiProvider = request.ZoneDefaults.MultiProvider;
				req.ZoneDefaults.Nameservers = request.ZoneDefaults.Nameservers;
				req.ZoneDefaults.NameserverTtl = request.ZoneDefaults.NameserverTtl;
				req.ZoneDefaults.SecondaryOverrides = request.ZoneDefaults.SecondaryOverrides;
				req.ZoneDefaults.SOA = request.ZoneDefaults.SOA;
				req.ZoneDefaults.ZoneMode = request.ZoneDefaults.ZoneMode;
			}

			return client.PatchAsync<DnsAccountSettings, InternalUpdateDnsAccountSettingsRequest>($"/accounts/{request.AccountId}/dns_settings", req, cancellationToken);
		}

		/// <summary>
		/// Show DNS settings for a zone default of an account.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsAccountSettings>> ShowDnsAccountSettings(this ICloudflareClient client, string accountId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();

			return client.GetAsync<DnsAccountSettings>($"/accounts/{accountId}/dns_settings", null, cancellationToken);
		}

		#region Views

		/// <summary>
		/// Create Internal DNS View for an account.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<InternalDnsView>> CreateInternalDnsView(this ICloudflareClient client, CreateInternalDnsViewRequest request, CancellationToken cancellationToken = default)
		{
			request.AccountId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ArgumentNullException(nameof(request.Name));

			if (request.Name.Length > 255)
				throw new ArgumentOutOfRangeException(nameof(request.Name), request.Name, "The Name length must be between 1 and 255 characters.");

			var req = new InternalModifyInternalDnsViewRequest
			{
				Name = request.Name,
				Zones = request.ZoneIds
			};

			return client.PostAsync<InternalDnsView, InternalModifyInternalDnsViewRequest>($"/accounts/{request.AccountId}/dns_settings/views", req, null, cancellationToken);
		}

		/// <summary>
		/// Delete an existing Internal DNS View.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="viewId">The view identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Identifier>> DeleteInternalDnsView(this ICloudflareClient client, string accountId, string viewId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();
			viewId.ValidateCloudflareId();

			return client.DeleteAsync<Identifier>($"/accounts/{accountId}/dns_settings/views/{viewId}", null, cancellationToken);
		}

		/// <summary>
		/// Update an existing Internal DNS View.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<InternalDnsView>> UpdateInternalDnsView(this ICloudflareClient client, UpdateInternalDnsViewRequest request, CancellationToken cancellationToken = default)
		{
			request.AccountId.ValidateCloudflareId();
			request.ViewId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ArgumentNullException(nameof(request.Name));

			if (request.Name.Length > 255)
				throw new ArgumentOutOfRangeException(nameof(request.Name), request.Name, "The Name length must be between 1 and 255 characters.");

			var req = new InternalModifyInternalDnsViewRequest
			{
				Name = request.Name,
				Zones = request.ZoneIds
			};

			return client.PatchAsync<InternalDnsView, InternalModifyInternalDnsViewRequest>($"/accounts/{request.AccountId}/dns_settings/views/{request.ViewId}", req, cancellationToken);
		}

		/// <summary>
		/// Get DNS Internal View.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="viewId">The view identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<InternalDnsView>> InternalDnsViewDetails(this ICloudflareClient client, string accountId, string viewId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();
			viewId.ValidateCloudflareId();

			return client.GetAsync<InternalDnsView>($"/accounts/{accountId}/dns_settings/views/{viewId}", null, cancellationToken);
		}

		/// <summary>
		/// List DNS Internal Views for an Account.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="options">Filter options (optional).</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IReadOnlyCollection<InternalDnsView>>> ListInternalDnsViews(this ICloudflareClient client, string accountId, ListInternalDnsViewsFilter? options = null, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();

			return client.GetAsync<IReadOnlyCollection<InternalDnsView>>($"/accounts/{accountId}/dns_settings/views", options, cancellationToken);
		}

		#endregion Views
	}
}
