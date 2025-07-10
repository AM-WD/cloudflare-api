namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Represents a request to update a domain.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/registrar/domains.ts#L256">Source</see>
	/// </summary>
	public class UpdateDomainRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UpdateDomainRequest"/> class.
		/// </summary>
		/// <param name="accountId">Identifier.</param>
		/// <param name="domainName">Domain name.</param>
		public UpdateDomainRequest(string accountId, string domainName)
		{
			AccountId = accountId;
			DomainName = domainName;
		}

		/// <summary>
		/// Identifier.
		/// </summary>
		public string AccountId { get; set; }

		/// <summary>
		/// Domain name.
		/// </summary>
		public string DomainName { get; set; }

		/// <summary>
		/// Auto-renew controls whether subscription is automatically renewed upon domain expiration.
		/// </summary>
		public bool? AutoRenew { get; set; }

		/// <summary>
		/// Shows whether a registrar lock is in place for a domain.
		/// </summary>
		public bool? Locked { get; set; }

		/// <summary>
		/// Privacy option controls redacting WHOIS information.
		/// </summary>
		public bool? Privacy { get; set; }
	}
}
