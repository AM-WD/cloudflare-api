namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Represents a request to update a zone hold.
	/// </summary>
	public class UpdateZoneHoldRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UpdateZoneHoldRequest"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		public UpdateZoneHoldRequest(string zoneId)
		{
			ZoneId = zoneId;
		}

		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; }

		/// <summary>
		/// If the value is provided and future-dated, the hold will be temporarily disabled,
		/// then automatically re-enabled by the system at the time specified in this timestamp.
		/// A past-dated value will have no effect on an existing, enabled hold.
		/// Providing an empty string will set its value to the current time.
		/// </summary>
		public DateTime? HoldAfter { get; set; }

		/// <summary>
		/// If <see langword="true"/>, the zone hold will extend to block any subdomain of the given zone, as well as SSL4SaaS Custom Hostnames.
		/// For example, a zone hold on a zone with the hostname 'example.com' and <c><see cref="IncludeSubdomains"/>=<see langword="true"/></c>
		/// will block 'example.com', 'staging.example.com', 'api.staging.example.com', etc.
		/// </summary>
		public bool? IncludeSubdomains { get; set; }
	}
}
