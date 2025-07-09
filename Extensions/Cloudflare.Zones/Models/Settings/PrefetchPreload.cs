namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Cloudflare will prefetch any URLs that are included in the response headers.
	/// This is limited to Enterprise Zones.
	/// </summary>
	public class PrefetchPreload : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PrefetchPreload"/> class.
		/// </summary>
		public PrefetchPreload()
		{
			Id = ZoneSettingId.PrefetchPreload;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
