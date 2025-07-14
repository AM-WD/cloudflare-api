namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Represents a request to create a zone subscription.
	/// </summary>
	public class CreateZoneSubscriptionRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateZoneSubscriptionRequest"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		public CreateZoneSubscriptionRequest(string zoneId)
		{
			ZoneId = zoneId;
		}

		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; }

		/// <summary>
		/// How often the subscription is renewed automatically.
		/// </summary>
		public Frequency? Frequency { get; set; }

		/// <summary>
		/// The rate plan applied to the subscription.
		/// </summary>
		public RatePlan? RatePlan { get; set; }
	}
}
