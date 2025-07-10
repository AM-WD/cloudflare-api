namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Represents a request to create a zone.
	/// </summary>
	public class CreateZoneRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateZoneRequest"/> class.
		/// </summary>
		/// <param name="name">The domain name.</param>
		public CreateZoneRequest(string name)
		{
			Name = name;
		}

		/// <summary>
		/// The account identifier.
		/// </summary>
		public string? AccountId { get; set; }

		/// <summary>
		/// The domain name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// A full zone implies that DNS is hosted with Cloudflare.
		/// A partial zone is typically a partner-hosted zone or a CNAME setup.
		/// </summary>
		public ZoneType? Type { get; set; }
	}
}
