namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Delegate Signer record.
	/// </summary>
	public class DSRecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DSRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public DSRecord(string name)
			: base(name)
		{
			Type = DnsRecordType.DS;
		}

		/// <summary>
		/// Components of a DS record.
		/// </summary>
		[JsonProperty("data")]
		public DSRecordData? Data { get; set; }
	}

	/// <summary>
	/// Components of a DS record.
	/// </summary>
	public class DSRecordData
	{
		/// <summary>
		/// Algorithm.
		/// </summary>
		[JsonProperty("algorithm")]
		public int? Algorithm { get; set; }

		/// <summary>
		/// Digest.
		/// </summary>
		[JsonProperty("digest")]
		public string? Digest { get; set; }

		/// <summary>
		/// Digest type.
		/// </summary>
		[JsonProperty("digest_type")]
		public int? DigestType { get; set; }

		/// <summary>
		/// Key tag.
		/// </summary>
		[JsonProperty("key_tag")]
		public int? KeyTag { get; set; }
	}
}
