namespace AMWD.Net.Api.Cloudflare.Zones.Internals
{
	internal class InternalUpdateZoneSubscriptionRequest
	{
		[JsonProperty("frequency")]
		public RenewFrequency? Frequency { get; set; }

		[JsonProperty("rate_plan")]
		public RatePlan? RatePlan { get; set; }
	}
}
