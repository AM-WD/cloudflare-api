namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Certification Authority Authorization record.
	/// </summary>
	public class CAARecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CAARecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public CAARecord(string name)
			: base(name)
		{
			Type = DnsRecordType.CAA;
		}

		/// <summary>
		/// Components of a CAA record.
		/// </summary>
		[JsonProperty("data")]
		public CAARecordData? Data { get; set; }

		/// <inheritdoc/>
		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public override string ToString()
		{
			return $"{Name}  {TimeToLive}  IN  CAA  {Data?.Flags} {Data?.Tag} \"{Data?.Value}\"";
		}
	}

	/// <summary>
	/// Components of a CAA record.
	/// </summary>
	public class CAARecordData
	{
		/// <summary>
		/// Flags for the CAA record.
		/// </summary>
		[JsonProperty("flags")]
		public int? Flags { get; set; }

		/// <summary>
		/// Name of the property controlled by this record (e.g.: issue, issuewild, iodef).
		/// </summary>
		[JsonProperty("tag")]
		public string? Tag { get; set; }

		/// <summary>
		/// Value of the record. This field's semantics depend on the chosen tag.
		/// </summary>
		[JsonProperty("value")]
		public string? Value { get; set; }
	}
}
