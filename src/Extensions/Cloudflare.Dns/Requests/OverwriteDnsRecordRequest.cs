namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to overwrite a DNS record.
	/// </summary>
	public class OverwriteDnsRecordRequest : CreateDnsRecordRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OverwriteDnsRecordRequest"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="recordId">The DNS record identifier.</param>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public OverwriteDnsRecordRequest(string zoneId, string recordId, string name)
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
