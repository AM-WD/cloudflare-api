namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to add a custom nameserver.
	/// </summary>
	public class AddCustomNameserverRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AddCustomNameserverRequest"/> class.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="nameserverName">The FQDN of the name server.</param>
		public AddCustomNameserverRequest(string accountId, string nameserverName)
		{
			AccountId = accountId;
			NameserverName = nameserverName;
		}

		/// <summary>
		/// The account identifier.
		/// </summary>
		public string AccountId { get; set; }

		/// <summary>
		/// The FQDN of the name server.
		/// </summary>
		public string NameserverName { get; set; }

		/// <summary>
		/// The number of the set that this name server belongs to.
		/// </summary>
		public int? NameserverSet { get; set; }
	}
}
