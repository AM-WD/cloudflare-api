namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// 0-RTT session resumption enabled for this zone.
	/// </summary>
	public class ZeroRTT : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ZeroRTT"/> class.
		/// </summary>
		public ZeroRTT()
		{
			Id = ZoneSettingId.ZeroRTT;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
