namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a response of a secondary zone configuration.
	/// </summary>
	public class IncomingZoneConfiguration
	{
		/// <summary>
		/// The unique identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// How often should a secondary zone auto refresh regardless of DNS NOTIFY.
		/// Not applicable for primary zones.
		/// </summary>
		[JsonProperty("auto_refresh_seconds")]
		public int? AutoRefreshSeconds { get; set; }

		/// <summary>
		/// The time for a specific event.
		/// </summary>
		[JsonProperty("checked_time")]
		public DateTime? CheckedTime { get; set; }

		/// <summary>
		/// The time for a specific event.
		/// </summary>
		[JsonProperty("created_time")]
		public DateTime? CreatedTime { get; set; }

		/// <summary>
		/// The time for a specific event.
		/// </summary>
		[JsonProperty("modified_time")]
		public DateTime? ModifiedTime { get; set; }

		/// <summary>
		/// The zone name.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }

		/// <summary>
		/// A list of peer tags.
		/// </summary>
		[JsonProperty("peers")]
		public IReadOnlyCollection<string>? Peers { get; set; }

		/// <summary>
		/// The serial number of the SOA for the given zone.
		/// </summary>
		[JsonProperty("soa_serial")]
		public int? SoaSerial { get; set; }
	}
}
