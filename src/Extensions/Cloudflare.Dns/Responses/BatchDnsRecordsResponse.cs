namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// The response for a batch update request.
	/// </summary>
	public class BatchDnsRecordsResponse
	{
		/// <summary>
		/// The records that were deleted (DELETE).
		/// </summary>
		[JsonProperty("deletes")]
		public IReadOnlyCollection<DnsRecord>? Deletes { get; set; }

		/// <summary>
		/// The records that were updated (PATCH).
		/// </summary>
		[JsonProperty("patches")]
		public IReadOnlyCollection<DnsRecord>? Updates { get; set; }

		/// <summary>
		/// The records that were created (POST).
		/// </summary>
		[JsonProperty("posts")]
		public IReadOnlyCollection<DnsRecord>? Creates { get; set; }

		/// <summary>
		/// The records that were overwritten (PUT).
		/// </summary>
		[JsonProperty("puts")]
		public IReadOnlyCollection<DnsRecord>? Overwrites { get; set; }
	}
}
