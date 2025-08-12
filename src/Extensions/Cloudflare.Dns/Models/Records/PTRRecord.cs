namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// PTR resource record.
	/// </summary>
	public class PTRRecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PTRRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public PTRRecord(string name)
			: base(name)
		{
			Type = DnsRecordType.PTR;
		}

		/// <inheritdoc/>
		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public override string ToString()
		{
			return $"{Name}  {TimeToLive}  IN  PTR  {Content}";
		}
	}
}
