namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Name Server record.
	/// </summary>
	public class NSRecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NSRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public NSRecord(string name)
			: base(name)
		{
			Type = DnsRecordType.NS;
		}
	}
}
