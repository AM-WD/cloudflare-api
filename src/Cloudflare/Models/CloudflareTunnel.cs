using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// A Cloudflare Tunnel that connects your origin to Cloudflare's edge.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L132">Source</see>
	/// </summary>
	public class CloudflareTunnel
	{
		/// <summary>
		/// UUID of the tunnel.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// Cloudflare account ID.
		/// </summary>
		[JsonProperty("account_tag")]
		public string? AccountTag { get; set; }

		/// <summary>
		/// Active connections.
		/// </summary>
		[JsonProperty("connections")]
		[Obsolete("This field will start returning an empty array. To fetch the connections of a given tunnel, please use the dedicated endpoint '/accounts/{account_id}/{tunnel_type}/{tunnel_id}/connections'.")]
		public IReadOnlyCollection<CloudflareTunnelConnection>? Connections { get; set; }

		/// <summary>
		/// Timestamp of when the tunnel established at least one connection to Cloudflare's edge.
		/// If <see langword="null"/>, the tunnel is inactive.
		/// </summary>
		[JsonProperty("conns_active_at")]
		public DateTime? ConnectionsActiveAt { get; set; }

		/// <summary>
		/// Timestamp of when the tunnel became inactive (no connections to Cloudflare's edge).
		/// If <see langword="null"/>, the tunnel is active.
		/// </summary>
		[JsonProperty("conns_inactive_at")]
		public DateTime? ConnectionsInactiveAt { get; set; }

		/// <summary>
		/// Timestamp of when the resource was created.
		/// </summary>
		[JsonProperty("created_at")]
		public DateTime? CreatedAt { get; set; }

		/// <summary>
		/// Timestamp of when the resource was deleted.
		/// If <see langword="null"/>, the resource has not been deleted.
		/// </summary>
		[JsonProperty("deleted_at")]
		public DateTime? DeletedAt { get; set; }

		/// <summary>
		/// Metadata associated with the tunnel.
		/// </summary>
		[JsonProperty("metadata")]
		public object? MetaData { get; set; }

		/// <summary>
		/// A user-friendly name for a tunnel.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }

		/// <summary>
		/// If <see langword="true"/>, the tunnel can be configured remotely from the Zero Trust dashboard.
		/// If <see langword="false"/>, the tunnel must be configured locally on the origin machine.
		/// </summary>
		[JsonProperty("remote_config")]
		public bool? RemoteConfiguration { get; set; }

		/// <summary>
		/// The status of the tunnel.
		/// </summary>
		[JsonProperty("status")]
		public CloudflareTunnelStatus? Status { get; set; }

		/// <summary>
		/// The type of tunnel.
		/// </summary>
		[JsonProperty("tun_type")]
		public CloudflareTunnelType? TunType { get; set; }
	}

	/// <summary>
	/// A connection to Cloudflare's edge.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L204">Source</see>
	/// </summary>
	public class CloudflareTunnelConnection
	{
		/// <summary>
		/// UUID of the Cloudflare Tunnel connection.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// UUID of the Cloudflare Tunnel connector.
		/// </summary>
		[JsonProperty("client_id")]
		public string? ClientId { get; set; }

		/// <summary>
		/// The cloudflared version used to establish this connection.
		/// </summary>
		[JsonProperty("client_version")]
		public string? ClientVersion { get; set; }

		/// <summary>
		/// The Cloudflare data center used for this connection.
		/// </summary>
		[JsonProperty("colo_name")]
		public string? ColocationName { get; set; }

		/// <summary>
		/// Cloudflare continues to track connections for several minutes after they disconnect.
		/// This is an optimization to improve latency and reliability of reconnecting.
		/// <br/>
		/// If <see langword="true"/>, the connection has disconnected but is still being tracked.
		/// If <see langword="false"/>, the connection is actively serving traffic.
		/// </summary>
		[JsonProperty("is_pending_reconnect")]
		public bool? IsPendingReconnect { get; set; }

		/// <summary>
		/// Timestamp of when the connection was established.
		/// </summary>
		[JsonProperty("opened_at")]
		public DateTime? OpenedAt { get; set; }

		/// <summary>
		/// The public IP address of the host running cloudflared.
		/// </summary>
		[JsonProperty("origin_ip")]
		public string? OriginIp { get; set; }

		/// <summary>
		/// UUID of the Cloudflare Tunnel connection.
		/// </summary>
		[JsonProperty("uuid")]
		public string? UUID { get; set; }
	}

	/// <summary>
	/// The status of the tunnel.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L195">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum CloudflareTunnelStatus
	{
		/// <summary>
		/// The tunnel has never been run.
		/// </summary>
		[EnumMember(Value = "inactive")]
		Inactive = 1,

		/// <summary>
		/// The tunnel is active and able to serve traffic but in an unhealthy state.
		/// </summary>
		[EnumMember(Value = "degraded")]
		Degraded = 2,

		/// <summary>
		/// The tunnel is active and able to serve traffic.
		/// </summary>
		[EnumMember(Value = "healthy")]
		Healthy = 3,

		/// <summary>
		/// The tunnel can not serve traffic as it has no connections to the Cloudflare Edge.
		/// </summary>
		[EnumMember(Value = "down")]
		Down = 4
	}

	/// <summary>
	/// The type of tunnel.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L200">Source</see>
	/// </summary>
	public enum CloudflareTunnelType
	{
		/// <summary>
		/// Cloudflared.
		/// </summary>
		[EnumMember(Value = "cfd_tunnel")]
		Cloudflared = 1,

		/// <summary>
		/// WARP Connector.
		/// </summary>
		[EnumMember(Value = "warp_connector")]
		WarpConnector = 2,

		/// <summary>
		/// WARP.
		/// </summary>
		[EnumMember(Value = "warp")]
		Warp = 3,

		/// <summary>
		/// Magic WAN.
		/// </summary>
		[EnumMember(Value = "magic")]
		MagicWAN = 4,

		/// <summary>
		/// IPsec.
		/// </summary>
		[EnumMember(Value = "ip_sec")]
		IpSec = 5,

		/// <summary>
		/// GRE.
		/// </summary>
		[EnumMember(Value = "gre")]
		Gre = 6,

		/// <summary>
		/// CNI.
		/// </summary>
		[EnumMember(Value = "cni")]
		Cni = 7
	}
}
