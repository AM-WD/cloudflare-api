namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Certificate record.
	/// </summary>
	public class CERTRecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CERTRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public CERTRecord(string name)
			: base(name)
		{
			Type = DnsRecordType.CERT;
		}

		/// <summary>
		/// Components of a CERT record.
		/// </summary>
		[JsonProperty("data")]
		public CERTRecordData? Data { get; set; }
	}

	/// <summary>
	/// Components of a CERT record.
	/// </summary>
	public class CERTRecordData
	{
		/// <summary>
		/// Algorithm.
		/// </summary>
		[JsonProperty("algorithm")]
		public int? Algorithm { get; set; }

		/// <summary>
		/// Certificate.
		/// </summary>
		[JsonProperty("certificate")]
		public string? Certificate { get; set; }

		/// <summary>
		/// Key tag.
		/// </summary>
		[JsonProperty("key_tag")]
		public int? KeyTag { get; set; }

		/// <summary>
		/// Type.
		/// </summary>
		[JsonProperty("type")]
		public int? Type { get; set; }
	}
}
