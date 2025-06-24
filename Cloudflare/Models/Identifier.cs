namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// A Cloudflare identifier.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L266">Source</see>
	/// </summary>
	public class Identifier
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }
	}
}
