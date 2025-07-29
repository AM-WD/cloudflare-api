namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Settings for a Cloudflare DNS zone on account level.
	/// </summary>
	public class DnsAccountSettings
	{
		/// <summary>
		/// Settings zone defaults.
		/// </summary>
		[JsonProperty("zone_defaults")]
		public DnsAccountZoneDefaults? ZoneDefaults { get; set; }
	}
}
