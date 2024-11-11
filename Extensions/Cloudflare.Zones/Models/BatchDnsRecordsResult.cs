using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// The DNS batch execution response.
	/// </summary>
	public class BatchDnsRecordsResult
	{
		/// <summary>
		/// The deleted records.
		/// </summary>
		[JsonProperty("deletes")]
		public IList<DnsRecord>? DeletedRecords { get; set; }

		/// <summary>
		/// The updated records.
		/// </summary>
		[JsonProperty("patches")]
		public IList<DnsRecord>? UpdatedRecords { get; set; }

		/// <summary>
		/// The overwritten records.
		/// </summary>
		[JsonProperty("puts")]
		public IList<DnsRecord>? OverwrittenRecords { get; set; }

		/// <summary>
		/// The created records.
		/// </summary>
		[JsonProperty("posts")]
		public IList<DnsRecord>? CreatedRecords { get; set; }
	}
}
