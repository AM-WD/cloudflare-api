namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to update a DNS record.
	/// </summary>
	public class UpdateDnsRecordRequest : CreateDnsRecordRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UpdateDnsRecordRequest"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="recordId">The DNS record identifier.</param>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public UpdateDnsRecordRequest(string zoneId, string recordId, string name)
			: base(zoneId, name)
		{
			RecordId = recordId;
		}

		/// <summary>
		/// The DNS record identifier.
		/// </summary>
		public string RecordId { get; set; }
	}
}
