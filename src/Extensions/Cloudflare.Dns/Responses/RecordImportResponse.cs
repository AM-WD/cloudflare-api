namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// The response for a DNS record import.
	/// </summary>
	public class RecordImportResponse
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
