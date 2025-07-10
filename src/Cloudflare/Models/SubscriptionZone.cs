namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// A simple zone object. May have null properties if not a zone subscription.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L776">Source</see>
	/// </summary>
	public class SubscriptionZone
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// The domain name.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }
	}
}
