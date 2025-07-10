namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Orange to Orange (O2O) allows zones on Cloudflare to CNAME to other zones also
	/// on Cloudflare.
	/// </summary>
	public class OrangeToOrange : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OrangeToOrange"/> class.
		/// </summary>
		public OrangeToOrange()
		{
			Id = ZoneSettingId.OrangeToOrange;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
