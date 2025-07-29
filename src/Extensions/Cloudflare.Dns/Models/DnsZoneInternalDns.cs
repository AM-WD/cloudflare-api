namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Settings for this internal zone.
	/// </summary>
	public class DnsZoneInternalDns
	{
		/// <summary>
		/// The ID of the zone to fallback to.
		/// </summary>
		[JsonProperty("reference_zone_id")]
		public string? ReferenceZoneId { get; set; }
	}
}
