namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Service locator record.
	/// </summary>
	public class SRVRecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SRVRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public SRVRecord(string name)
			: base(name)
		{
			Type = DnsRecordType.SRV;
		}

		/// <summary>
		/// Components of a SRV record.
		/// </summary>
		[JsonProperty("data")]
		public SRVRecordData? Data { get; set; }
	}

	/// <summary>
	/// Components of a SRV record.
	/// </summary>
	public class SRVRecordData
	{
		/// <summary>
		/// The port of the service.
		/// </summary>
		[JsonProperty("port")]
		public int? Port { get; set; }

		/// <summary>
		/// Required for MX, SRV and URI records; unused by other record types.
		/// Records with lower priorities are preferred.
		/// </summary>
		[JsonProperty("priority")]
		public int? Priority { get; set; }

		/// <summary>
		/// A valid hostname.
		/// </summary>
		[JsonProperty("target")]
		public string? Target { get; set; }

		/// <summary>
		/// The record weight.
		/// </summary>
		[JsonProperty("weight")]
		public int? Weight { get; set; }
	}
}
