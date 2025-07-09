namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Turn on or off
	/// <see href="https://developers.cloudflare.com/waf/reference/legacy/old-waf-managed-rules/">WAF managed rules (previous version, deprecated)</see>.
	/// You cannot enable or disable individual WAF managed rules via Page Rules.
	/// </summary>
	public class WAF : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WAF"/> class.
		/// </summary>
		public WAF()
		{
			Id = ZoneSettingId.WAF;
		}

		/// <summary>
		/// The status of WAF managed rules (previous version).
		/// </summary>
		[JsonProperty("value")]
		public OnOffState? Value { get; set; }
	}
}
