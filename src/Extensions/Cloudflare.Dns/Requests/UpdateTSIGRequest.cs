namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to update an existing TSIG (Transaction Signature) key.
	/// </summary>
	public class UpdateTSIGRequest : CreateTSIGRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UpdateTSIGRequest"/> class.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="tsigId">The TSIG identifier.</param>
		/// <param name="name">TSIG key name.</param>
		/// <param name="secret">TSIG secret.</param>
		public UpdateTSIGRequest(string accountId, string tsigId, string name, string secret)
			: base(accountId, name, secret)
		{
			TSigId = tsigId;
		}

		/// <summary>
		/// The TSIG identifier.
		/// </summary>
		public string TSigId { get; set; }
	}
}
