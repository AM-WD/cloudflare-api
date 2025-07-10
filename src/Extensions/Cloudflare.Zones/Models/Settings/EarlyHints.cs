namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// When enabled, Cloudflare will attempt to speed up overall page loads by serving
	/// <c>103</c> responses with <c>Link</c> headers from the final response. Refer to
	/// <see href="https://developers.cloudflare.com/cache/about/early-hints">Early Hints</see>
	/// for more information.
	/// </summary>
	public class EarlyHints : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EarlyHints"/> class.
		/// </summary>
		public EarlyHints()
		{
			Id = ZoneSettingId.EarlyHints;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
