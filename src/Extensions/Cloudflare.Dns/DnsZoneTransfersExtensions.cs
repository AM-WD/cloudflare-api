using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Dns.Internals;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Extensions for <see href="https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/">DNS Zone Transfers</see>.
	/// </summary>
	public static class DnsZoneTransfersExtensions
	{
		#region ACLs

		/// <summary>
		/// Create ACL.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ACL>> CreateACL(this ICloudflareClient client, CreateACLRequest request, CancellationToken cancellationToken = default)
		{
			request.AccountId.ValidateCloudflareId();

			if (request.IpRangeBaseAddress.AddressFamily == AddressFamily.InterNetwork && request.IpRangeSubnet < 24)
				throw new ArgumentOutOfRangeException(nameof(request.IpRange), "CIDRs are limited to a maximum of /24 for IPv4.");

			if (request.IpRangeBaseAddress.AddressFamily == AddressFamily.InterNetworkV6 && request.IpRangeSubnet < 64)
				throw new ArgumentOutOfRangeException(nameof(request.IpRange), "CIDRs are limited to a maximum of /64 for IPv6.");

			var req = new InternalDnsZoneTransferAclRequest
			{
				Name = request.Name,
				IpRange = request.IpRange
			};

			return client.PostAsync<ACL, InternalDnsZoneTransferAclRequest>($"/accounts/{request.AccountId}/secondary_dns/acls", req, null, cancellationToken);
		}

		/// <summary>
		/// Delete ACL.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="aclId">The access control list identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Identifier>> DeleteACL(this ICloudflareClient client, string accountId, string aclId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();
			aclId.ValidateCloudflareId();

			return client.DeleteAsync<Identifier>($"/accounts/{accountId}/secondary_dns/acls/{aclId}", null, cancellationToken);
		}

		/// <summary>
		/// Get ACL.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="aclId">The access control list identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ACL>> ACLDetails(this ICloudflareClient client, string accountId, string aclId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();
			aclId.ValidateCloudflareId();

			return client.GetAsync<ACL>($"/accounts/{accountId}/secondary_dns/acls/{aclId}", null, cancellationToken);
		}

		/// <summary>
		/// List ACLs.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IReadOnlyCollection<ACL>>> ListACLs(this ICloudflareClient client, string accountId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();

			return client.GetAsync<IReadOnlyCollection<ACL>>($"/accounts/{accountId}/secondary_dns/acls", null, cancellationToken);
		}

		/// <summary>
		/// Update ACL.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ACL>> UpdateACL(this ICloudflareClient client, UpdateDnsZoneTransferAclRequest request, CancellationToken cancellationToken = default)
		{
			request.AccountId.ValidateCloudflareId();
			request.AclId.ValidateCloudflareId();

			if (request.IpRangeBaseAddress.AddressFamily == AddressFamily.InterNetwork && request.IpRangeSubnet < 24)
				throw new ArgumentOutOfRangeException(nameof(request.IpRange), "CIDRs are limited to a maximum of /24 for IPv4.");

			if (request.IpRangeBaseAddress.AddressFamily == AddressFamily.InterNetworkV6 && request.IpRangeSubnet < 64)
				throw new ArgumentOutOfRangeException(nameof(request.IpRange), "CIDRs are limited to a maximum of /64 for IPv6.");

			var req = new InternalDnsZoneTransferAclRequest
			{
				Name = request.Name,
				IpRange = request.IpRange
			};

			return client.PutAsync<ACL, InternalDnsZoneTransferAclRequest>($"/accounts/{request.AccountId}/secondary_dns/acls/{request.AclId}", req, cancellationToken);
		}

		#endregion ACLs

		#region Force AXFR

		/// <summary>
		/// Sends AXFR zone transfer request to primary nameserver(s).
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<string>> ForceAXFR(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.PostAsync<string, object>($"/zones/{zoneId}/secondary_dns/force_axfr", null, null, cancellationToken);
		}

		#endregion Force AXFR

		#region Incoming

		/// <summary>
		/// Create secondary zone configuration for incoming zone transfers.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IncomingZoneConfiguration>> CreateSecondaryZoneConfiguration(this ICloudflareClient client, SecondaryZoneConfigurationRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ArgumentNullException(nameof(request.Name), "The zone name is required.");

			if (request.AutoRefreshSeconds < 0)
				throw new ArgumentOutOfRangeException(nameof(request.AutoRefreshSeconds), "Auto refresh seconds must be greater than or equal to 0.");

			if (request.Peers.Count == 0)
				throw new ArgumentOutOfRangeException(nameof(request.Peers), "At least one peer is required.");

			foreach (string peer in request.Peers)
				peer.ValidateCloudflareId();

			var req = new InternalSecondaryZoneConfigurationRequest
			{
				Name = request.Name,
				AutoRefreshSeconds = request.AutoRefreshSeconds,
				Peers = request.Peers.ToList()
			};

			return client.PostAsync<IncomingZoneConfiguration, InternalSecondaryZoneConfigurationRequest>($"/zones/{request.ZoneId}/secondary_dns/incoming", req, null, cancellationToken);
		}

		/// <summary>
		/// Delete secondary zone configuration for incoming zone transfers.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Identifier>> DeleteSecondaryZoneConfiguration(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.DeleteAsync<Identifier>($"/zones/{zoneId}/secondary_dns/incoming", null, cancellationToken);
		}

		/// <summary>
		/// Get secondary zone configuration for incoming zone transfers.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IncomingZoneConfiguration>> SecondaryZoneConfigurationDetails(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.GetAsync<IncomingZoneConfiguration>($"/zones/{zoneId}/secondary_dns/incoming", null, cancellationToken);
		}

		/// <summary>
		/// Update secondary zone configuration for incoming zone transfers.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IncomingZoneConfiguration>> UpdateSecondaryZoneConfiguration(this ICloudflareClient client, SecondaryZoneConfigurationRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ArgumentNullException(nameof(request.Name), "The zone name is required.");

			if (request.AutoRefreshSeconds < 0)
				throw new ArgumentOutOfRangeException(nameof(request.AutoRefreshSeconds), "Auto refresh seconds must be greater than or equal to 0.");

			if (request.Peers.Count == 0)
				throw new ArgumentOutOfRangeException(nameof(request.Peers), "At least one peer is required.");

			foreach (string peer in request.Peers)
				peer.ValidateCloudflareId();

			var req = new InternalSecondaryZoneConfigurationRequest
			{
				Name = request.Name,
				AutoRefreshSeconds = request.AutoRefreshSeconds,
				Peers = request.Peers.ToList()
			};

			return client.PutAsync<IncomingZoneConfiguration, InternalSecondaryZoneConfigurationRequest>($"/zones/{request.ZoneId}/secondary_dns/incoming", req, cancellationToken);
		}

		#endregion Incoming

		#region Outgoing

		/// <summary>
		/// Create primary zone configuration for outgoing zone transfers.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<OutgoingZoneConfiguration>> CreatePrimaryZoneConfiguration(this ICloudflareClient client, PrimaryZoneConfigurationRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ArgumentNullException(nameof(request.Name), "The zone name is required.");

			if (request.Peers.Count == 0)
				throw new ArgumentOutOfRangeException(nameof(request.Peers), "At least one peer is required.");

			foreach (string peer in request.Peers)
				peer.ValidateCloudflareId();

			var req = new InternalPrimaryZoneConfigurationRequest
			{
				Name = request.Name,
				Peers = request.Peers.ToList()
			};

			return client.PostAsync<OutgoingZoneConfiguration, InternalPrimaryZoneConfigurationRequest>($"/zones/{request.ZoneId}/secondary_dns/outgoing", req, null, cancellationToken);
		}

		/// <summary>
		/// Delete primary zone configuration for outgoing zone transfers.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Identifier>> DeletePrimaryZoneConfiguration(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.DeleteAsync<Identifier>($"/zones/{zoneId}/secondary_dns/outgoing", null, cancellationToken);
		}

		/// <summary>
		/// Disable outgoing zone transfers for primary zone and clears IXFR backlog of primary zone.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		/// <returns>
		/// Referring to the
		/// <see href="https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/outgoing/methods/disable/">documentation</see>,
		/// the text value should be <em>Disabled</em>.
		/// </returns>
		public static Task<CloudflareResponse<string>> DisableOutgoingZoneTransfers(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.PostAsync<string, object>($"/zones/{zoneId}/secondary_dns/outgoing/disable", null, null, cancellationToken);
		}

		/// <summary>
		/// Enable outgoing zone transfers for primary zone.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		/// <returns>
		/// Referring to the
		/// <see href="https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/outgoing/methods/enable/">documentation</see>,
		/// the text value should be <em>Enabled</em>.
		/// </returns>
		public static Task<CloudflareResponse<string>> EnableOutgoingZoneTransfers(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.PostAsync<string, object>($"/zones/{zoneId}/secondary_dns/outgoing/enable", null, null, cancellationToken);
		}

		/// <summary>
		/// Get primary zone configuration for outgoing zone transfers
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<OutgoingZoneConfiguration>> PrimaryZoneConfigurationDetails(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.GetAsync<OutgoingZoneConfiguration>($"/zones/{zoneId}/secondary_dns/outgoing", null, cancellationToken);
		}

		/// <summary>
		/// Update primary zone configuration for outgoing zone transfers.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<OutgoingZoneConfiguration>> UpdatePrimaryZoneConfiguration(this ICloudflareClient client, PrimaryZoneConfigurationRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ArgumentNullException(nameof(request.Name), "The zone name is required.");

			if (request.Peers.Count == 0)
				throw new ArgumentOutOfRangeException(nameof(request.Peers), "At least one peer is required.");

			foreach (string peer in request.Peers)
				peer.ValidateCloudflareId();

			var req = new InternalPrimaryZoneConfigurationRequest
			{
				Name = request.Name,
				Peers = request.Peers.ToList()
			};

			return client.PutAsync<OutgoingZoneConfiguration, InternalPrimaryZoneConfigurationRequest>($"/zones/{request.ZoneId}/secondary_dns/outgoing", req, cancellationToken);
		}

		/// <summary>
		/// Notifies the secondary nameserver(s) and clears IXFR backlog of primary zone.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		/// <returns>
		/// Referring to the
		/// <see href="https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/outgoing/methods/force_notify/">documentation</see>,
		/// the text value should be <em>OK</em>.
		/// </returns>
		public static Task<CloudflareResponse<string>> ForceDNSNotify(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.PostAsync<string, object>($"/zones/{zoneId}/secondary_dns/outgoing/force_notify", null, null, cancellationToken);
		}

		/// <summary>
		/// Enable outgoing zone transfers for primary zone.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		/// <returns>
		/// Referring to the
		/// <see href="https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/outgoing/subresources/status/methods/get/">documentation</see>,
		/// the text value should be <em>Enabled</em> or <em>Disabled</em>.
		/// </returns>
		public static Task<CloudflareResponse<string>> GetOutgoingZoneTransferStatus(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.GetAsync<string>($"/zones/{zoneId}/secondary_dns/outgoing/status", null, cancellationToken);
		}

		#endregion Outgoing

		#region Peers

		/// <summary>
		/// List Peers.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IReadOnlyCollection<Peer>>> ListPeers(this ICloudflareClient client, string accountId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();

			return client.GetAsync<IReadOnlyCollection<Peer>>($"/accounts/{accountId}/secondary_dns/peers", null, cancellationToken);
		}

		/// <summary>
		/// Get Peer.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="peerId">The peer identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Peer>> PeerDetails(this ICloudflareClient client, string accountId, string peerId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();
			peerId.ValidateCloudflareId();

			return client.GetAsync<Peer>($"/accounts/{accountId}/secondary_dns/peers/{peerId}", null, cancellationToken);
		}

		/// <summary>
		/// Create Peer.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Peer>> CreatePeer(this ICloudflareClient client, CreatePeerRequest request, CancellationToken cancellationToken = default)
		{
			request.AccountId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ArgumentNullException(nameof(request.Name), "The peer name is required.");

			var req = new InternalCreatePeerRequest
			{
				Name = request.Name
			};

			return client.PostAsync<Peer, InternalCreatePeerRequest>($"/accounts/{request.AccountId}/secondary_dns/peers", req, null, cancellationToken);
		}

		/// <summary>
		/// Modify Peer.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Peer>> UpdatePeer(this ICloudflareClient client, UpdatePeerRequest request, CancellationToken cancellationToken = default)
		{
			request.AccountId.ValidateCloudflareId();
			request.PeerId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ArgumentNullException(nameof(request.Name), "The peer name is required.");

			if (request.Port < 0 || 65535 < request.Port)
				throw new ArgumentOutOfRangeException(nameof(request.Port), "The port must be between 0 and 65535.");

			var req = new InternalUpdatePeerRequest
			{
				Name = request.Name,
				Ip = request.IpAddress,
				IxfrEnable = request.IXFREnable,
				Port = request.Port,
				TSigId = request.TSIGId
			};

			return client.PutAsync<Peer, InternalUpdatePeerRequest>($"/accounts/{request.AccountId}/secondary_dns/peers/{request.PeerId}", req, cancellationToken);
		}

		/// <summary>
		/// Delete Peer.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="peerId">The peer identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Identifier>> DeletePeer(this ICloudflareClient client, string accountId, string peerId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();
			peerId.ValidateCloudflareId();

			return client.DeleteAsync<Identifier>($"/accounts/{accountId}/secondary_dns/peers/{peerId}", null, cancellationToken);
		}

		#endregion Peers

		#region TSIGs

		/// <summary>
		/// List TSIGs.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IReadOnlyCollection<TSIG>>> ListTSIGs(this ICloudflareClient client, string accountId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();

			return client.GetAsync<IReadOnlyCollection<TSIG>>($"/accounts/{accountId}/secondary_dns/tsigs", null, cancellationToken);
		}

		/// <summary>
		/// Get TSIG.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="tsigId">The TSIG identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<TSIG>> TSIGDetails(this ICloudflareClient client, string accountId, string tsigId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();
			tsigId.ValidateCloudflareId();

			return client.GetAsync<TSIG>($"/accounts/{accountId}/secondary_dns/tsigs/{tsigId}", null, cancellationToken);
		}

		/// <summary>
		/// Create TSIG.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<TSIG>> CreateTSIG(this ICloudflareClient client, CreateTSIGRequest request, CancellationToken cancellationToken = default)
		{
			request.AccountId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ArgumentNullException(nameof(request.Name), "The TSIG name is required.");

			if (!Enum.IsDefined(typeof(TSigAlgorithm), request.Algorithm))
				throw new ArgumentOutOfRangeException(nameof(request.Algorithm), "The TSIG algorithm is invalid.");

			if (string.IsNullOrWhiteSpace(request.Secret))
				throw new ArgumentNullException(nameof(request.Secret), "The TSIG secret is required.");

			var req = new InternalTSIGRequest
			{
				Name = request.Name,
				Algorithm = request.Algorithm,
				Secret = request.Secret
			};

			return client.PostAsync<TSIG, InternalTSIGRequest>($"/accounts/{request.AccountId}/secondary_dns/tsigs", req, null, cancellationToken);
		}

		/// <summary>
		/// Modify TSIG.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<TSIG>> UpdateTSIG(this ICloudflareClient client, UpdateTSIGRequest request, CancellationToken cancellationToken = default)
		{
			request.AccountId.ValidateCloudflareId();
			request.TSigId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ArgumentNullException(nameof(request.Name), "The TSIG name is required.");

			if (!Enum.IsDefined(typeof(TSigAlgorithm), request.Algorithm))
				throw new ArgumentOutOfRangeException(nameof(request.Algorithm), "The TSIG algorithm is invalid.");

			if (string.IsNullOrWhiteSpace(request.Secret))
				throw new ArgumentNullException(nameof(request.Secret), "The TSIG secret is required.");

			var req = new InternalTSIGRequest
			{
				Name = request.Name,
				Algorithm = request.Algorithm,
				Secret = request.Secret
			};

			return client.PutAsync<TSIG, InternalTSIGRequest>($"/accounts/{request.AccountId}/secondary_dns/tsigs/{request.TSigId}", req, cancellationToken);
		}

		/// <summary>
		/// Delete TSIG.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="tsigId">The TSIG identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Identifier>> DeleteTSIG(this ICloudflareClient client, string accountId, string tsigId, CancellationToken cancellationToken = default)
		{
			accountId.ValidateCloudflareId();
			tsigId.ValidateCloudflareId();

			return client.DeleteAsync<Identifier>($"/accounts/{accountId}/secondary_dns/tsigs/{tsigId}", null, cancellationToken);
		}

		#endregion TSIGs
	}
}
