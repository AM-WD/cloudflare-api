using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Zone modes.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ZoneMode
	{
		/// <summary>
		/// Standard.
		/// </summary>
		[EnumMember(Value = "standard")]
		Standard = 1,

		/// <summary>
		/// Only as CDN.
		/// </summary>
		[EnumMember(Value = "cdn_only")]
		CdnOnly = 2,

		/// <summary>
		/// Only as DNS.
		/// </summary>
		[EnumMember(Value = "dns_only")]
		DnsOnly = 3,
	}
}
