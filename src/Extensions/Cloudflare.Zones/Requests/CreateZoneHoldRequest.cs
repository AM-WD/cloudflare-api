namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Represents a request to create a zone hold.
	/// </summary>
	public class CreateZoneHoldRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateZoneHoldRequest"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		public CreateZoneHoldRequest(string zoneId)
		{
			ZoneId = zoneId;
		}

		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; }

		/// <summary>
		/// If provided, the zone hold will extend to block any subdomain of the given zone, as well as SSL4SaaS Custom Hostnames.
		/// </summary>
		/// <remarks>
		/// For example, a zone hold on a zone with the hostname 'example.com' and
		/// <c><see cref="IncludeSubdomains"/>=<see langword="true"/></c> will block
		/// 'example.com', 'staging.example.com', 'api.staging.example.com', etc.
		/// </remarks>
		public bool? IncludeSubdomains { get; set; }
	}
}
