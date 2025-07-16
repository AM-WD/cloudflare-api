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
	}
}
