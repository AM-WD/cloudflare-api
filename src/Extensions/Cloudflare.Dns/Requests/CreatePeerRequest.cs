namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to create a new peer within a specific account.
	/// </summary>
	public class CreatePeerRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreatePeerRequest"/> class.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="name">The name of the peer.</param>
		public CreatePeerRequest(string accountId, string name)
		{
			AccountId = accountId;
			Name = name;
		}

		/// <summary>
		/// The account identifier.
		/// </summary>
		public string AccountId { get; set; }

		/// <summary>
		/// The name of the peer.
		/// </summary>
		public string Name { get; set; }
	}
}
