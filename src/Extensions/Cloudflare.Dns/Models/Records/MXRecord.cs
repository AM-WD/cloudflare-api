namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Mail Exchange record.
	/// </summary>
	public class MXRecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MXRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public MXRecord(string name)
			: base(name)
		{
			Type = DnsRecordType.MX;
		}

		/// <summary>
		/// Required for MX, SRV and URI records; unused by other record types.
		/// Records with lower priorities are preferred.
		/// </summary>
		[JsonProperty("priority")]
		public int? Priority { get; set; }

		/// <inheritdoc/>
		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public override string ToString()
		{
			return $"{Name}  {TimeToLive}  IN  MX  {Priority} {Content}";
		}
	}
}
