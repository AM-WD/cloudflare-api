namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Uniform Resource Identifier record.
	/// </summary>
	public class URIRecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="URIRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public URIRecord(string name)
			: base(name)
		{
			Type = DnsRecordType.URI;
		}

		/// <summary>
		/// Required for MX, SRV and URI records; unused by other record types.
		/// Records with lower priorities are preferred.
		/// </summary>
		[JsonProperty("priority")]
		public int? Priority { get; set; }

		/// <summary>
		/// Components of a URI record.
		/// </summary>
		[JsonProperty("data")]
		public URIRecordData? Data { get; set; }
	}

	/// <summary>
	/// Components of a URI record.
	/// </summary>
	public class URIRecordData
	{
		/// <summary>
		/// The record content.
		/// </summary>
		[JsonProperty("target")]
		public string? Target { get; set; }

		/// <summary>
		/// The record weight.
		/// </summary>
		[JsonProperty("weight")]
		public int? Weight { get; set; }
	}
}
