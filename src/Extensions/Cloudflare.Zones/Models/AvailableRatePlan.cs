namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// A Cloudflare available plan.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/plans.ts#L60">Source</see>
	/// </summary>
	public class AvailableRatePlan
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// Indicates whether you can subscribe to this plan.
		/// </summary>
		[JsonProperty("can_subscribe")]
		public bool? CanSubscribe { get; set; }

		/// <summary>
		/// The monetary unit in which pricing information is displayed.
		/// </summary>
		[JsonProperty("currency")]
		public string? Currency { get; set; }

		/// <summary>
		/// Indicates whether this plan is managed externally.
		/// </summary>
		[JsonProperty("externally_managed")]
		public bool? ExternallyManaged { get; set; }

		/// <summary>
		/// The frequency at which you will be billed for this plan.
		/// </summary>
		[JsonProperty("frequency")]
		public Frequency? Frequency { get; set; }

		/// <summary>
		/// Indicates whether you are currently subscribed to this plan.
		/// </summary>
		[JsonProperty("is_subscribed")]
		public bool? IsSubscribed { get; set; }

		/// <summary>
		/// Indicates whether this plan has a legacy discount applied.
		/// </summary>
		[JsonProperty("legacy_discount")]
		public bool? LegacyDiscount { get; set; }

		/// <summary>
		/// The legacy identifier for this rate plan, if any.
		/// </summary>
		[JsonProperty("legacy_id")]
		public string? LegacyId { get; set; }

		/// <summary>
		/// The plan name.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }

		/// <summary>
		/// The amount you will be billed for this plan.
		/// </summary>
		[JsonProperty("price")]
		public decimal? Price { get; set; }
	}
}
