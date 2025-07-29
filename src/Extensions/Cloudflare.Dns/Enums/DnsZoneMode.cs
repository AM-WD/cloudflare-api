using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// The mode of a DNS zone.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/dns/settings/zone.ts#L94">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum DnsZoneMode
	{
		/// <summary>
		/// The standard mode.
		/// </summary>
		[EnumMember(Value = "standard")]
		Standard = 1,

		/// <summary>
		/// The CDN-only mode.
		/// </summary>
		[EnumMember(Value = "cdn_only")]
		CdnOnly = 2,

		/// <summary>
		/// The DNS-only mode.
		/// </summary>
		[EnumMember(Value = "dns_only")]
		DnsOnly = 3
	}
}
