namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Cloudflare Mirage reduces bandwidth used by images in mobile browsers. It can
	/// accelerate loading of image-heavy websites on very slow mobile connections and
	/// HTTP/1.
	/// </summary>
	public class Mirage : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Mirage"/> class.
		/// </summary>
		public Mirage()
		{
			Id = ZoneSettingId.Mirage;
		}

		/// <summary>
		/// The status of Mirage.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState? Value { get; set; }
	}
}
