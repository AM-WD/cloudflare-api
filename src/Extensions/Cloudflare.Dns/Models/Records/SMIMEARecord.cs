namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// S/MIME cert association record.
	/// </summary>
	public class SMIMEARecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SMIMEARecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public SMIMEARecord(string name)
			: base(name)
		{
			Type = DnsRecordType.SMIMEA;
		}

		/// <summary>
		/// Components of a SMIMEA record.
		/// </summary>
		[JsonProperty("data")]
		public SMIMEARecordData? Data { get; set; }

		/// <inheritdoc/>
		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public override string ToString()
		{
			return $"{Name}  {TimeToLive}  IN  SMIMEA  {Data?.Usage} {Data?.Selector} {Data?.MatchingType} {Data?.Certificate}";
		}
	}

	/// <summary>
	/// Components of a SMIMEA record.
	/// </summary>
	public class SMIMEARecordData
	{
		/// <summary>
		/// Certificate.
		/// </summary>
		[JsonProperty("certificate")]
		public string? Certificate { get; set; }

		/// <summary>
		/// Matching type.
		/// </summary>
		[JsonProperty("matching_type")]
		public int? MatchingType { get; set; }

		/// <summary>
		/// Selector.
		/// </summary>
		[JsonProperty("selector")]
		public int? Selector { get; set; }

		/// <summary>
		/// Usage.
		/// </summary>
		[JsonProperty("usage")]
		public int? Usage { get; set; }
	}
}
