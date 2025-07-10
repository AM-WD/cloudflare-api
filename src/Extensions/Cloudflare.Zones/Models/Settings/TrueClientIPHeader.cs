namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Turn on or off the True-Client-IP Header feature of the Cloudflare Network app.
	/// </summary>
	public class TrueClientIPHeader : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TrueClientIPHeader"/> class.
		/// </summary>
		public TrueClientIPHeader()
		{
			Id = ZoneSettingId.TrueClientIPHeader;
		}

		/// <summary>
		/// The status of True Client IP Header.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState? Value { get; set; }
	}
}
