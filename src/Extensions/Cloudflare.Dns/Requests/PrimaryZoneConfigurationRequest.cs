namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to create a primary zone configuration.
	/// </summary>
	public class PrimaryZoneConfigurationRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PrimaryZoneConfigurationRequest"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="name">The zone name.</param>
		public PrimaryZoneConfigurationRequest(string zoneId, string name)
		{
			ZoneId = zoneId;
			Name = name;
		}

		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; }

		/// <summary>
		/// The zone name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// A list of peer tags.
		/// </summary>
		public IReadOnlyCollection<string> Peers { get; set; } = [];
	}
}
