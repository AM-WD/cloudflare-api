namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to delete a DNS record.
	/// </summary>
	public class DeleteDnsRecordRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DeleteDnsRecordRequest"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="recordId">The DNS record identifier.</param>
		public DeleteDnsRecordRequest(string zoneId, string recordId)
		{
			ZoneId = zoneId;
			RecordId = recordId;
		}

		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; }

		/// <summary>
		/// The DNS record identifier.
		/// </summary>
		public string RecordId { get; set; }
	}
}
