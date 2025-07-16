namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to create a DNS record.
	/// </summary>
	public class CreateDnsRecordRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateDnsRecordRequest"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public CreateDnsRecordRequest(string zoneId, string name)
		{
			ZoneId = zoneId;
			Name = name;
		}

		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; }

		/// <summary>
		/// DNS record name (or @ for the zone apex) in Punycode.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Time To Live (TTL) of the DNS record in seconds.
		/// </summary>
		/// <remarks>
		/// Setting to <c>1</c> means 'automatic'. Value must be between <c>60</c> and <c>86400</c>, with the
		/// minimum reduced to <c>30</c> for Enterprise zones.
		/// </remarks>
		public int? TimeToLive { get; set; }

		/// <summary>
		/// DNS record type.
		/// </summary>
		public DnsRecordType Type { get; set; }

		/// <summary>
		/// Comments or notes about the DNS record.
		/// This field has no effect on DNS responses.
		/// </summary>
		public string? Comment { get; set; }

		/// <summary>
		/// The content of the DNS record.
		/// </summary>
		public string? Content { get; set; }

		/// <summary>
		/// Components of a record.
		/// </summary>
		public object? Data { get; set; }

		/// <summary>
		/// Required for MX, SRV and URI records; unused by other record types.
		/// Records with lower priorities are preferred.
		/// </summary>
		public int? Priority { get; set; }

		/// <summary>
		/// Whether the record is receiving the performance and security benefits of
		/// Cloudflare.
		/// </summary>
		public bool? Proxied { get; set; }

		/// <summary>
		/// Settings for the DNS record.
		/// </summary>
		public DnsRecordSettings? Settings { get; set; }

		/// <summary>
		/// Custom tags for the DNS record.
		/// This field has no effect on DNS responses.
		/// </summary>
		public IReadOnlyCollection<string>? Tags { get; set; }
	}
}
