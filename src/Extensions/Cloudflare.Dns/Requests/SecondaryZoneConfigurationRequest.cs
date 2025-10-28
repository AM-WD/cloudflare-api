namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to create a secondary zone configuration.
	/// </summary>
	public class SecondaryZoneConfigurationRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SecondaryZoneConfigurationRequest"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="name">The zone name.</param>
		public SecondaryZoneConfigurationRequest(string zoneId, string name)
		{
			ZoneId = zoneId;
			Name = name;

			Peers = [];
		}

		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; }

		/// <summary>
		/// The Zone name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// How often should a secondary zone auto refresh regardless of DNS NOTIFY. Not applicable for primary zones.
		/// </summary>
		public int AutoRefreshSeconds { get; set; }

		/// <summary>
		/// A list of peer tags.
		/// </summary>
		public IReadOnlyCollection<string> Peers { get; set; }
	}
}
