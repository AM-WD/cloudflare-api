namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// When enabled, the Hotlink Protection option ensures that other sites cannot suck
	/// up your bandwidth by building pages that use images hosted on your site.Anytime
	/// a request for an image on your site hits Cloudflare, we check to ensure that
	/// it's not another site requesting them. People will still be able to download and
	/// view images from your page, but other sites won't be able to steal them for use
	/// on their own pages.
	/// (<see href="https://support.cloudflare.com/hc/en-us/articles/200170026"/>).
	/// </summary>
	public class HotlinkProtection : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HotlinkProtection"/> class.
		/// </summary>
		public HotlinkProtection()
		{
			Id = ZoneSettingId.HotlinkProtection;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
