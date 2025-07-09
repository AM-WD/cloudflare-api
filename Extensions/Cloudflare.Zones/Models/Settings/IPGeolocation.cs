namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Cloudflare adds a CF-IPCountry HTTP header containing the country code that
	/// corresponds to the visitor.
	/// </summary>
	public class IPGeolocation : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IPGeolocation"/> class.
		/// </summary>
		public IPGeolocation()
		{
			Id = ZoneSettingId.IPGeolocation;
		}

		/// <summary>
		/// The status of adding the IP Geolocation Header.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState? Value { get; set; }
	}
}
