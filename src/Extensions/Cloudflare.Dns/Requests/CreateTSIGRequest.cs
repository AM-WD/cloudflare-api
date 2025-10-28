namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to create a TSIG (Transaction Signature) key for DNS operations.
	/// </summary>
	public class CreateTSIGRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateTSIGRequest"/> class.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="name">TSIG key name.</param>
		/// <param name="secret">TSIG secret.</param>
		public CreateTSIGRequest(string accountId, string name, string secret)
		{
			Algorithm = TSigAlgorithm.HMAC_SHA256;

			AccountId = accountId;
			Name = name;
			Secret = secret;
		}

		/// <summary>
		/// The account identifier.
		/// </summary>
		public string AccountId { get; set; }

		/// <summary>
		/// TSIG algorithm.
		/// </summary>
		public TSigAlgorithm Algorithm { get; set; }

		/// <summary>
		/// TSIG key name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// TSIG secret.
		/// </summary>
		public string Secret { get; set; }
	}
}
