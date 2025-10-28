namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a response of a secondary zone configuration.
	/// </summary>
	public class OutgoingZoneConfiguration
	{
		/// <summary>
		/// The unique identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

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
		[JsonProperty("last_transferred_time")]
		public DateTime? LastTransferredTime { get; set; }

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
