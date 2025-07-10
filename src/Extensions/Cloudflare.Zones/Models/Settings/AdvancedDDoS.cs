namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Advanced protection from Distributed Denial of Service (DDoS) attacks on your
	/// website. This is an uneditable value that is <see cref="OnOffState.On"/> in the
	/// case of Business and Enterprise zones.
	/// </summary>
	public class AdvancedDDoS : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AdvancedDDoS"/> class.
		/// </summary>
		public AdvancedDDoS()
		{
			Id = ZoneSettingId.AdvancedDDoS;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
