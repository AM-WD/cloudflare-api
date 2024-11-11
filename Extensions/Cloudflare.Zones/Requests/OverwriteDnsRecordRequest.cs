namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Request to overwrite a DNS record.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the <see cref="OverwriteDnsRecordRequest"/> class.
	/// </remarks>
	/// <param name="id">The DNS record identifier.</param>
	/// <param name="zoneId">The zone identifier.</param>
	/// <param name="name">DNS record name (or @ for the zone apex).</param>
	/// <param name="type">Record type.</param>
	public class OverwriteDnsRecordRequest(string id, string zoneId, string name, DnsRecordType type) : CreateDnsRecordRequest(zoneId, name, type)
	{
		/// <summary>
		/// The DNS record identifier.
		/// </summary>
		public string Id { get; set; } = id;
	}
}
