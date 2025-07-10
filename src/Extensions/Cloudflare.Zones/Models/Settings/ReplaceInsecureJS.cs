namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Automatically replace insecure JavaScript libraries with safer and faster
	/// alternatives provided under cdnjs and powered by Cloudflare. Currently supports
	/// the following libraries: Polyfill under polyfill.io.
	/// </summary>
	public class ReplaceInsecureJS : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ReplaceInsecureJS"/> class.
		/// </summary>
		public ReplaceInsecureJS()
		{
			Id = ZoneSettingId.ReplaceInsecureJS;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
