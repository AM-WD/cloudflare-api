namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// The DNS records import result.
	/// </summary>
	public class ImportDnsRecordsResult
	{
		/// <summary>
		/// Number of DNS records added.
		/// </summary>
		[JsonProperty("recs_added")]
		public int? RecordsAdded { get; set; }

		/// <summary>
		/// Total number of DNS records parsed.
		/// </summary>
		[JsonProperty("total_records_parsed")]
		public int? TotalRecordsParsed { get; set; }
	}
}
