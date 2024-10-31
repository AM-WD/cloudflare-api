namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// The deleted zone.
	/// </summary>
	public class ZoneIdResponse
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		// <= 32 characters
		[JsonProperty("id")]
		public string? Id { get; set; }
	}
}
