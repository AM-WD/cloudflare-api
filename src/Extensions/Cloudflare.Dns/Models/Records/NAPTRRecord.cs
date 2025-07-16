namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Naming authority pointer record.
	/// </summary>
	public class NAPTRRecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NAPTRRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public NAPTRRecord(string name)
			: base(name)
		{
			Type = DnsRecordType.NAPTR;
		}

		/// <summary>
		/// Components of a NAPTR record.
		/// </summary>
		[JsonProperty("data")]
		public NAPTRRecordData? Data { get; set; }
	}

	/// <summary>
	/// Components of a NAPTR record.
	/// </summary>
	public class NAPTRRecordData
	{
		/// <summary>
		/// Flags.
		/// </summary>
		[JsonProperty("flags")]
		public string? Flags { get; set; }

		/// <summary>
		/// Order.
		/// </summary>
		[JsonProperty("order")]
		public int? Order { get; set; }

		/// <summary>
		/// Preference.
		/// </summary>
		[JsonProperty("preference")]
		public int? Preference { get; set; }

		/// <summary>
		/// Regular expression.
		/// </summary>
		[JsonProperty("regex")]
		public string? Regex { get; set; }

		/// <summary>
		/// Replacement.
		/// </summary>
		[JsonProperty("replacement")]
		public string? Replacement { get; set; }

		/// <summary>
		/// Service.
		/// </summary>
		[JsonProperty("service")]
		public string? Service { get; set; }
	}
}
