namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Address record.
	/// </summary>
	public class ARecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ARecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public ARecord(string name)
			: base(name)
		{
			Type = DnsRecordType.A;
		}
	}
}
