namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// IPv6 address record.
	/// </summary>
	public class AAAARecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ARecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public AAAARecord(string name)
			: base(name)
		{
			Type = DnsRecordType.AAAA;
		}
	}
}
