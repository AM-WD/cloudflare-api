namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to update DNS zone defaults on account level.
	/// </summary>
	public class UpdateDnsAccountSettingsRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UpdateDnsAccountSettingsRequest"/> class.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		public UpdateDnsAccountSettingsRequest(string accountId)
		{
			AccountId = accountId;
		}

		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string AccountId { get; set; }

		/// <summary>
		/// The DNS zone defaults.
		/// </summary>
		public DnsAccountZoneDefaults? ZoneDefaults { get; set; }
	}
}
