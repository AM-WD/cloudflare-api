namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// Base implementation of an owner.
	/// </summary>
	public class OwnerBase
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		// <= 32 characters
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// Name of the owner.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// The type of owner.
		/// </summary>
		[JsonProperty("type")]
		public string Type { get; set; }
	}
}
