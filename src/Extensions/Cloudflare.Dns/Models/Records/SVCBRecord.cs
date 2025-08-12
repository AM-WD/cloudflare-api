namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Service Binding record.
	/// </summary>
	public class SVCBRecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SVCBRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public SVCBRecord(string name)
			: base(name)
		{
			Type = DnsRecordType.SVCB;
		}

		/// <summary>
		/// Components of a SVCB record.
		/// </summary>
		[JsonProperty("data")]
		public SVCBRecordData? Data { get; set; }

		/// <inheritdoc/>
		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public override string ToString()
		{
			return $"{Name}  {TimeToLive}  IN  SVCB  {Data?.Priority} {Data?.Target} {Data?.Value}";
		}
	}

	/// <summary>
	/// Components of a SVCB record.
	/// </summary>
	public class SVCBRecordData
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
