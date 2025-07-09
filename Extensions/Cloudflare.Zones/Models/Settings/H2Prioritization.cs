namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// HTTP/2 Edge Prioritization optimises the delivery of resources served through
	/// HTTP/2 to improve page load performance.It also supports fine control of
	/// content delivery when used in conjunction with Workers.
	/// </summary>
	public class H2Prioritization : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="H2Prioritization"/> class.
		/// </summary>
		public H2Prioritization()
		{
			Id = ZoneSettingId.H2Prioritization;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffCustomState Value { get; set; }
	}
}
