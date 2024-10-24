namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Request to create a new zone.
	/// </summary>
	public class CreateZoneRequest
	{
		/// <summary>
		/// The account identifier.
		/// </summary>
		public string AccountId { get; set; }

		/// <summary>
		/// The domain name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The zone type.
		/// </summary>
		/// <remarks>
		/// A full zone implies that DNS is hosted with Cloudflare.
		/// A partial zone is typically a partner-hosted zone or a CNAME setup.
		/// </remarks>
		public ZoneType Type { get; set; }
	}
}
