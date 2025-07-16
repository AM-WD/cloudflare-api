namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Text record.
	/// </summary>
	public class TXTRecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TXTRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public TXTRecord(string name)
			: base(name)
		{
			Type = DnsRecordType.TXT;
		}

		/// <summary>
		/// Text content for the record. The content must consist of quoted "character
		/// strings" (RFC 1035), each with a length of up to 255 bytes. Strings exceeding
		/// this allowed maximum length are automatically split.
		/// </summary>
		/// <remarks>
		/// Learn more at <see href="https://www.cloudflare.com/learning/dns/dns-records/dns-txt-record/"/>.
		/// </remarks>
		[JsonProperty("content")]
		public new string? Content { get; set; }
	}
}
