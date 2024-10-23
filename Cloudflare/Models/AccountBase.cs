namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// Base implementation of an account.
	/// </summary>
	public class AccountBase
	{
		/// <summary>
		/// Identifier
		/// </summary>
		// <= 32 characters
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// The name of the account.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }
	}
}
