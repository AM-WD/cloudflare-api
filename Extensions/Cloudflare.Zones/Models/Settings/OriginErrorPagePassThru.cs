namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Turn on or off Cloudflare error pages generated from issues sent from the origin
	/// server. If enabled, this setting triggers error pages issued by the origin.
	/// </summary>
	public class OriginErrorPagePassThru : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OriginErrorPagePassThru"/> class.
		/// </summary>
		public OriginErrorPagePassThru()
		{
			Id = ZoneSettingId.OriginErrorPagePassThru;
		}

		/// <summary>
		/// The status of Origin Error Page Passthru.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState? Value { get; set; }
	}
}
