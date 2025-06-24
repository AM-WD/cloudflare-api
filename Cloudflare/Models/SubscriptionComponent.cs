namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// A component value for a subscription.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L751">Source</see>
	/// </summary>
	public class SubscriptionComponent
	{
		/// <summary>
		/// The default amount assigned.
		/// </summary>
		[JsonProperty("default")]
		public int? Default { get; set; }

		/// <summary>
		/// The name of the component value.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }

		/// <summary>
		/// The unit price for the component value.
		/// </summary>
		[JsonProperty("price")]
		public decimal? Price { get; set; }

		/// <summary>
		/// The amount of the component value assigned.
		/// </summary>
		[JsonProperty("value")]
		public int? Value { get; set; }
	}
}
