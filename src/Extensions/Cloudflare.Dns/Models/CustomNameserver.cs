using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// A Cloudflare custom nameserver.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/custom-nameservers.ts#L88">Source</see>
	/// </summary>
	public class CustomNameserver
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CustomNameserver"/> class.
		/// </summary>
		/// <param name="nameserverName">The FQDN of the name server.</param>
		public CustomNameserver(string nameserverName)
		{
			NameserverName = nameserverName;
		}

		/// <summary>
		/// A and AAAA records associated with the nameserver.
		/// </summary>
		[JsonProperty("dns_records")]
		public IReadOnlyCollection<CustomNameserverDnsRecord>? DnsRecords { get; set; }

		/// <summary>
		/// The full qualified domain name (FQDN) of the name server.
		/// </summary>
		[JsonProperty("ns_name")]
		public string NameserverName { get; set; }

		/// <summary>
		/// Verification status of the nameserver.
		/// </summary>
		[Obsolete]
		[JsonProperty("status")]
		public CustomNameserverStatus? Status { get; set; }

		/// <summary>
		/// Identifier.
		/// </summary>
		[JsonProperty("zone_tag")]
		public string? ZoneTag { get; set; }

		/// <summary>
		/// The number of the set that this name server belongs to.
		/// </summary>
		/// <remarks>
		/// <c>1 &lt;= X &lt;= 5</c>
		/// <br/>
		/// Default: 1
		/// </remarks>
		[JsonProperty("ns_set")]
		public int? NameserverSet { get; set; }
	}

	/// <summary>
	/// Records associated with the nameserver.
	/// </summary>
	public class CustomNameserverDnsRecord
	{
		/// <summary>
		/// DNS record type.
		/// </summary>
		[JsonProperty("type")]
		public CustomNameserverDnsRecordType? Type { get; set; }

		/// <summary>
		/// DNS record contents (an IPv4 or IPv6 address).
		/// </summary>
		[JsonProperty("value")]
		public string? Value { get; set; }
	}

	/// <summary>
	/// Record types.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/custom-nameservers.ts#L120">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum CustomNameserverDnsRecordType
	{
		/// <summary>
		/// IPv4 record.
		/// </summary>
		[EnumMember(Value = "A")]
		A = 1,

		/// <summary>
		/// IPv6 record.
		/// </summary>
		[EnumMember(Value = "AAAA")]
		AAAA = 2
	}

	/// <summary>
	/// Custom nameserver states.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/custom-nameservers.ts#L102">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum CustomNameserverStatus
	{
		/// <summary>
		/// The nameserver has been moved.
		/// </summary>
		[EnumMember(Value = "moved")]
		Moved = 1,

		/// <summary>
		/// The nameserver is pending verification.
		/// </summary>
		[EnumMember(Value = "pending")]
		Pending = 2,

		/// <summary>
		/// The nameserver has been verified.
		/// </summary>
		[EnumMember(Value = "verified")]
		Verified = 3
	}
}
