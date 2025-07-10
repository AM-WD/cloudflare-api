namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Inspect the visitor's browser for headers commonly associated with spammers and
	/// certain bots.
	/// </summary>
	public class BrowserCheck : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BrowserCheck"/> class.
		/// </summary>
		public BrowserCheck()
		{
			Id = ZoneSettingId.BrowserCheck;
		}

		/// <summary>
		/// The status of Browser Integrity Check.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState? Value { get; set; }
	}
}
