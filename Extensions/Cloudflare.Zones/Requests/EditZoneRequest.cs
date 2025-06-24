namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Represents a request to edit a zone.
	/// </summary>
	public class EditZoneRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EditZoneRequest"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		public EditZoneRequest(string zoneId)
		{
			ZoneId = zoneId;
		}

		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; }

		/// <summary>
		/// Indicates whether the zone is only using Cloudflare DNS services.
		/// </summary>
		/// <remarks>
		/// A <see langword="true"/> value means the zone will not receive security or performance benefits.
		/// </remarks>
		public bool? Paused { get; set; }

		/// <summary>
		/// A full zone implies that DNS is hosted with Cloudflare.
		/// A partial zone is typically a partner-hosted zone or a CNAME setup.
		/// </summary>
		/// <remarks>
		/// This parameter is only available to Enterprise customers or if it has been explicitly enabled on a zone.
		/// </remarks>
		public ZoneType? Type { get; set; }

		/// <summary>
		/// A list of domains used for custom name servers.
		/// </summary>
		/// <remarks>
		/// This is only available for Business and Enterprise plans.
		/// </remarks>
		public IReadOnlyCollection<string>? VanityNameServers { get; set; }
	}
}
