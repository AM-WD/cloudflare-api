namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Represents a request to update a zone subscription.
	/// </summary>
	public class UpdateZoneSubscriptionRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UpdateZoneSubscriptionRequest"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		public UpdateZoneSubscriptionRequest(string zoneId)
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
		public RenewFrequency? Frequency { get; set; }

		/// <summary>
		/// The rate plan applied to the subscription.
		/// </summary>
		public RatePlan? RatePlan { get; set; }
	}
}
