namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// When the client requesting an asset supports the Brotli compression algorithm,
	/// Cloudflare will serve a Brotli compressed version of the asset.
	/// </summary>
	public class Brotli : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Brotli"/> class.
		/// </summary>
		public Brotli()
		{
			Id = ZoneSettingId.Brotli;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
