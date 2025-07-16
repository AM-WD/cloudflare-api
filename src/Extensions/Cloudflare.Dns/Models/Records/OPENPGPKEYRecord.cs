namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// OpenPGP Publi Key record.
	/// </summary>
	public class OPENPGPKEYRecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OPENPGPKEYRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public OPENPGPKEYRecord(string name)
			: base(name)
		{
			Type = DnsRecordType.OPENPGPKEY;
		}

		/// <summary>
		/// A single Base64-encoded OpenPGP Transferable Public Key (RFC 4880 Section 11.1).
		/// </summary>
		[JsonProperty("content")]
		public new string? Content { get; set; }
	}
}
