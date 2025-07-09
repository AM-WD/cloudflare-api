namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Enhance your website's font delivery with Cloudflare Fonts. Deliver Google
	/// Hosted fonts from your own domain, boost performance, and enhance user privacy.
	/// Refer to the Cloudflare Fonts documentation for more information.
	/// </summary>
	public class FontSettings : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FontSettings"/> class.
		/// </summary>
		public FontSettings()
		{
			Id = ZoneSettingId.FontSettings;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState? Value { get; set; }
	}
}
