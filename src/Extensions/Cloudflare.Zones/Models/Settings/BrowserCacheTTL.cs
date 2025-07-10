namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Control how long resources cached by client browsers remain valid.
	/// </summary>
	public class BrowserCacheTTL : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BrowserCacheTTL"/> class.
		/// </summary>
		public BrowserCacheTTL()
		{
			Id = ZoneSettingId.BrowserCacheTTL;
		}

		/// <summary>
		/// The number of seconds to cache resources for.
		/// The API prohibits setting this to 0 for non-Enterprise domains.
		/// </summary>
		[JsonProperty("value")]
		public int? Value { get; set; }
	}
}
