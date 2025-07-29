using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Settings determining the nameservers through which the zone should be available.
	/// </summary>
	public class DnsAccountNameservers
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DnsAccountNameservers"/> class.
		/// </summary>
		/// <param name="type">Nameserver type.</param>
		public DnsAccountNameservers(DnsAccountNameserversType type)
		{
			Type = type;
		}

		/// <summary>
		/// Nameserver type.
		/// </summary>
		[JsonProperty("type")]
		public DnsAccountNameserversType Type { get; set; }
	}

	/// <summary>
	/// The type of a DNS zone nameserver.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/dns/settings/account/account.ts#L137">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum DnsAccountNameserversType
	{
		/// <summary>
		/// The Cloudflare standard nameservers.
		/// </summary>
		[EnumMember(Value = "cloudflare.standard")]
		Standard = 1,

		/// <summary>
		/// The zone specific nameservers.
		/// </summary>
		[EnumMember(Value = "cloudflare.standard.random")]
		Random = 2,

		/// <summary>
		/// The account specific nameservers.
		/// </summary>
		[EnumMember(Value = "custom.account")]
		Account = 3,

		/// <summary>
		/// The tenant specific nameservers.
		/// </summary>
		[EnumMember(Value = "custom.tenant")]
		Tenant = 4,
	}
}
