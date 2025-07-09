namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// HTTP/2 enabled for this zone.
	/// </summary>
	public class HTTP2 : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HTTP2"/> class.
		/// </summary>
		public HTTP2()
		{
			Id = ZoneSettingId.HTTP2;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
