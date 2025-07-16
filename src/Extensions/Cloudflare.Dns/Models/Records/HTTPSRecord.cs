namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// HTTPS binding record.
	/// </summary>
	public class HTTPSRecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HTTPSRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public HTTPSRecord(string name)
			: base(name)
		{
			Type = DnsRecordType.HTTPS;
		}

		/// <summary>
		/// Components of a HTTPS record.
		/// </summary>
		[JsonProperty("data")]
		public HTTPSRecordData? Data { get; set; }
	}

	/// <summary>
	/// Components of a HTTPS record.
	/// </summary>
	public class HTTPSRecordData
	{
		/// <summary>
		/// Priority.
		/// </summary>
		[JsonProperty("priority")]
		public int? Priority { get; set; }

		/// <summary>
		/// Target.
		/// </summary>
		[JsonProperty("target")]
		public string? Target { get; set; }

		/// <summary>
		/// Value.
		/// </summary>
		[JsonProperty("value")]
		public string? Value { get; set; }
	}
}
