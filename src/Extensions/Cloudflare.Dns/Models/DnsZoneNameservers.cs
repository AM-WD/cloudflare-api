using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Settings determining the nameservers through which the zone should be available.
	/// </summary>
	public class DnsZoneNameservers
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DnsZoneNameservers"/> class.
		/// </summary>
		/// <param name="type">Nameserver type.</param>
		public DnsZoneNameservers(DnsZoneNameserversType type)
		{
			Type = type;
		}

		/// <summary>
		/// Nameserver type.
		/// </summary>
		[JsonProperty("type")]
		public DnsZoneNameserversType Type { get; set; }

		/// <summary>
		/// Configured nameserver set to be used for this zone.
		/// </summary>
		[JsonProperty("ns_set")]
		public int? NameserverSet { get; set; }
	}

	/// <summary>
	/// The type of a DNS zone nameserver.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/dns/settings/zone.ts#L115">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum DnsZoneNameserversType
	{
		/// <summary>
		/// The Cloudflare standard nameservers.
		/// </summary>
		[EnumMember(Value = "cloudflare.standard")]
		Standard = 1,

		/// <summary>
		/// The account specific nameservers.
		/// </summary>
		[EnumMember(Value = "custom.account")]
		Account = 2,

		/// <summary>
		/// The tenant specific nameservers.
		/// </summary>
		[EnumMember(Value = "custom.tenant")]
		Tenant = 3,

		/// <summary>
		/// The zone specific nameservers.
		/// </summary>
		[EnumMember(Value = "custom.zone")]
		Zone = 4
	}
}
