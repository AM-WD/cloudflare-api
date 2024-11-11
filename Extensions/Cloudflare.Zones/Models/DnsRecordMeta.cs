namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Extra Cloudflare-specific information about the record.
	/// </summary>
	public class DnsRecordMeta
	{
		/// <summary>
		/// Whether the record was automatically added.
		/// </summary>
		[JsonProperty("auto_added")]
		public bool AutoAdded { get; set; }

		/// <summary>
		/// Whether the record is managed by apps.
		/// </summary>
		[JsonProperty("managed_by_apps")]
		public bool ManagedByApps { get; set; }

		/// <summary>
		/// Whether the record is managed by argo tunnel.
		/// </summary>
		[JsonProperty("managed_by_argo_tunnel")]
		public bool ManagedByArgoTunnel { get; set; }
	}
}
