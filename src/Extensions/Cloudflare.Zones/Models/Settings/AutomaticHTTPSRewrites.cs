namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Turn on or off Automatic HTTPS Rewrites.
	/// </summary>
	public class AutomaticHTTPSRewrites : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AutomaticHTTPSRewrites"/> class.
		/// </summary>
		public AutomaticHTTPSRewrites()
		{
			Id = ZoneSettingId.AutomaticHTTPSRewrites;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
