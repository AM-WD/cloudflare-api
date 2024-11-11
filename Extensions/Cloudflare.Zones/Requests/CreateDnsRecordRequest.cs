using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Request to create a new DNS record.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the <see cref="CreateDnsRecordRequest"/> class.
	/// </remarks>
	/// <param name="zoneId">The zone identifier.</param>
	/// <param name="name">DNS record name (or @ for the zone apex).</param>
	/// <param name="type">Record type.</param>
	public class CreateDnsRecordRequest(string zoneId, string name, DnsRecordType type)
	{
		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; } = zoneId;

		/// <summary>
		/// Comments or notes about the DNS record.
		/// </summary>
		/// <remarks>
		/// This field has no effect on DNS responses.
		/// </remarks>
		public string? Comment { get; set; }

		/// <summary>
		/// DNS record name (or @ for the zone apex) in Punycode.
		/// </summary>
		public string Name { get; set; } = name;

		/// <summary>
		/// Whether the record is receiving the performance and security benefits of Cloudflare.
		/// </summary>
		public bool Proxied { get; set; }

		/// <summary>
		/// Settings for the DNS record.
		/// </summary>
		public object? Settings { get; set; }

		/// <summary>
		/// Custom tags for the DNS record.
		/// </summary>
		/// <remarks>
		/// This field has no effect on DNS responses.
		/// </remarks>
		public IList<string>? Tags { get; set; }

		/// <summary>
		/// Time To Live (TTL) of the DNS record in seconds.
		/// </summary>
		/// <remarks>
		/// Setting to <c>1</c> means 'automatic'.
		/// Value must be between <c>60</c> and <c>86400</c>, <em>with the minimum reduced to 30 for Enterprise zones</em>.
		/// </remarks>
		public int? Ttl { get; set; }

		/// <summary>
		/// Components of a record.
		/// </summary>
		/// <remarks>
		/// <strong>Caution:</strong> This field has priority over the <see cref="Content"/> field.
		/// </remarks>
		public object? Data { get; set; }

		/// <summary>
		/// A valid content.
		/// </summary>
		/// <remarks>
		/// <strong>Caution:</strong> This field has no effect for record types with specific <see cref="Data"/> values.
		/// </remarks>
		public string? Content { get; set; }

		/// <summary>
		/// A priority.
		/// </summary>
		/// <remarks>
		/// Required for <see cref="DnsRecordType.Mx"/>, <see cref="DnsRecordType.Srv"/> and <see cref="DnsRecordType.Uri"/> records; unused by other record types.
		/// Records with lower priorities are preferred.
		/// </remarks>
		public ushort? Priority { get; set; }

		/// <summary>
		/// Record type.
		/// </summary>
		public DnsRecordType Type { get; set; } = type;
	}
}
