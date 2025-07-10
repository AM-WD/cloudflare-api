namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// When the client requesting the image supports the WebP image codec, and WebP
	/// offers a performance advantage over the original image format, Cloudflare will
	/// serve a WebP version of the original image.
	/// </summary>
	public class WebP : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WebP"/> class.
		/// </summary>
		public WebP()
		{
			Id = ZoneSettingId.WebP;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
