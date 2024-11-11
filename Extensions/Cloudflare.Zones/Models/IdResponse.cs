namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// The deleted id.
	/// </summary>
	public class IdResponse
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		// <= 32 characters
		[JsonProperty("id")]
		public string? Id { get; set; }
	}
}
