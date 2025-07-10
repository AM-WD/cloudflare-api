namespace AMWD.Net.Api.Cloudflare.Zones.Internals
{
	internal class InternalUpdateZoneHoldRequest
	{
		[JsonProperty("hold_after")]
		public DateTime? HoldAfter { get; set; }

		[JsonProperty("include_subdomains")]
		public bool? IncludeSubdomains { get; set; }
	}
}
