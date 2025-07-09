namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// HTTP/3 enabled for this zone.
	/// </summary>
	public class HTTP3 : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HTTP3"/> class.
		/// </summary>
		public HTTP3()
		{
			Id = ZoneSettingId.HTTP3;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
