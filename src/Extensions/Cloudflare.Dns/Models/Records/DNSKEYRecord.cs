namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// DNS Key record.
	/// </summary>
	public class DNSKEYRecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DNSKEYRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public DNSKEYRecord(string name)
			: base(name)
		{
			Type = DnsRecordType.DNSKEY;
		}

		/// <summary>
		/// Components of a DNSKEY record.
		/// </summary>
		[JsonProperty("data")]
		public DNSKEYRecordData? Data { get; set; }
	}

	/// <summary>
	/// Components of a DNSKEY record.
	/// </summary>
	public class DNSKEYRecordData
	{
		/// <summary>
		/// Algorithm.
		/// </summary>
		[JsonProperty("algorithm")]
		public int? Algorithm { get; set; }

		/// <summary>
		/// Flags.
		/// </summary>
		[JsonProperty("flags")]
		public int? Flags { get; set; }

		/// <summary>
		/// Protocol.
		/// </summary>
		[JsonProperty("protocol")]
		public int? Protocol { get; set; }

		/// <summary>
		/// Public key.
		/// </summary>
		[JsonProperty("public_key")]
		public string? PublicKey { get; set; }
	}
}
