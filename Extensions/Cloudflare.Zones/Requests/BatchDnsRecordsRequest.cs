using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Batch of DNS Record API calls to be executed together.
	/// </summary>
	/// <remarks>
	/// Execution order: Delete, Update, Overwrite, Create
	/// </remarks>
	public class BatchDnsRecordsRequest(string zoneId)
	{
		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; } = zoneId;

		/// <summary>
		/// The DNS record identifiers to delete.
		/// </summary>
		public IList<string> DnsRecordIdsToDelete { get; set; } = [];

		/// <summary>
		/// The DNS records to update.
		/// </summary>
		public IList<UpdateDnsRecordRequest> DnsRecordsToUpdate { get; set; } = [];

		/// <summary>
		/// The DNS records to overwrite.
		/// </summary>
		public IList<OverwriteDnsRecordRequest> DnsRecordsToOverwrite { get; set; } = [];

		/// <summary>
		/// The DNS records to create.
		/// </summary>
		public IList<CreateDnsRecordRequest> DnsRecordsToCreate { get; set; } = [];
	}
}
