namespace AMWD.Net.Api.Cloudflare.Dns.Internals
{
	internal class InternalUpdateDnsAccountSettingsRequest
	{
		[JsonProperty("zone_defaults")]
		public InternalDnsAccountZoneDefaults? ZoneDefaults { get; set; }
	}
}
