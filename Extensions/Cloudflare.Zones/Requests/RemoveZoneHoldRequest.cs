namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Represents a request to remove a zone hold.
	/// </summary>
	public class RemoveZoneHoldRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RemoveZoneHoldRequest"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		public RemoveZoneHoldRequest(string zoneId)
		{
			ZoneId = zoneId;
		}

		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; }

		/// <summary>
		/// If it is provided, the hold will be temporarily disabled,
		/// then automatically re-enabled by the system at the time specified in this timestamp.
		/// Otherwise, the hold will be disabled indefinitely.
		/// </summary>
		public DateTime? HoldAfter { get; set; }
	}
}
