namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Enable IPv6 on all subdomains that are Cloudflare enabled.
	/// (<see href="https://support.cloudflare.com/hc/en-us/articles/200168586"/>).
	/// </summary>
	public class IPV6 : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IPV6"/> class.
		/// </summary>
		public IPV6()
		{
			Id = ZoneSettingId.IPV6;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
