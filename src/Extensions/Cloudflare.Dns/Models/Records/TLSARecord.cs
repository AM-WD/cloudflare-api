namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// TLSA certificate association record.
	/// </summary>
	public class TLSARecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TLSARecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public TLSARecord(string name)
			: base(name)
		{
			Type = DnsRecordType.TLSA;
		}

		/// <summary>
		/// Components of a TLSA record.
		/// </summary>
		[JsonProperty("data")]
		public TLSARecordData? Data { get; set; }
	}

	/// <summary>
	/// Components of a TLSA record.
	/// </summary>
	public class TLSARecordData
	{
		/// <summary>
		/// Certificate.
		/// </summary>
		[JsonProperty("certificate")]
		public string? Certificate { get; set; }

		/// <summary>
		/// Matching type.
		/// </summary>
		[JsonProperty("matching_type")]
		public int? MatchingType { get; set; }

		/// <summary>
		/// Selector.
		/// </summary>
		[JsonProperty("selector")]
		public int? Selector { get; set; }

		/// <summary>
		/// Usage.
		/// </summary>
		[JsonProperty("usage")]
		public int? Usage { get; set; }
	}
}
