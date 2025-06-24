using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// A Cloudflare subscription.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L705">Source</see>
	/// </summary>
	public class Subscription
	{
		/// <summary>
		/// Subscription identifier tag.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// The monetary unit in which pricing information is displayed.
		/// </summary>
		[JsonProperty("currency")]
		public string? Currency { get; set; }

		/// <summary>
		/// The end of the current period and also when the next billing is due.
		/// </summary>
		[JsonProperty("current_period_end")]
		public DateTime? CurrentPeriodEnd { get; set; }

		/// <summary>
		/// When the current billing period started.
		/// </summary>
		[JsonProperty("current_period_start")]
		public DateTime? CurrentPeriodStart { get; set; }

		/// <summary>
		/// How often the subscription is renewed automatically.
		/// </summary>
		[JsonProperty("frequency")]
		public RenewFrequency? Frequency { get; set; }

		/// <summary>
		/// The price of the subscription that will be billed, in US dollars.
		/// </summary>
		[JsonProperty("price")]
		public decimal? Price { get; set; }

		/// <summary>
		/// The rate plan applied to the subscription.
		/// </summary>
		[JsonProperty("rate_plan")]
		public RatePlan? RatePlan { get; set; }

		/// <summary>
		/// The state that the subscription is in.
		/// </summary>
		[JsonProperty("state")]
		public SubscriptionState? State { get; set; }
	}

	/// <summary>
	/// The state that the subscription is in.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L745">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum SubscriptionState
	{
		/// <summary>
		/// The subscription is in the trial period.
		/// </summary>
		[EnumMember(Value = "Trial")]
		Trial = 1,

		/// <summary>
		/// The subscription is provisioned.
		/// </summary>
		[EnumMember(Value = "Provisioned")]
		Provisioned = 2,

		/// <summary>
		/// The subscription is paid.
		/// </summary>
		[EnumMember(Value = "Paid")]
		Paid = 3,

		/// <summary>
		/// The subscription is awaiting payment.
		/// </summary>
		[EnumMember(Value = "AwaitingPayment")]
		AwaitingPayment = 4,

		/// <summary>
		/// The subscription is cancelled.
		/// </summary>
		[EnumMember(Value = "Cancelled")]
		Cancelled = 5,

		/// <summary>
		/// The subscription has failed.
		/// </summary>
		[EnumMember(Value = "Failed")]
		Failed = 6,

		/// <summary>
		/// The subscription has expired.
		/// </summary>
		[EnumMember(Value = "Expired")]
		Expired = 7
	}
}
