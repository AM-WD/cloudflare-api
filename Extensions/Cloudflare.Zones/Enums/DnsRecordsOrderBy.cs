using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Possible fields to order DNS records by.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum DnsRecordsOrderBy
	{
		/// <summary>
		/// Order by record type.
		/// </summary>
		[EnumMember(Value = "type")]
		Type = 1,

		/// <summary>
		/// Order by record name.
		/// </summary>
		[EnumMember(Value = "name")]
		Name = 2,

		/// <summary>
		/// Order by record content.
		/// </summary>
		[EnumMember(Value = "content")]
		Content = 3,

		/// <summary>
		/// Order by record TTL.
		/// </summary>
		[EnumMember(Value = "ttl")]
		Ttl = 4,

		/// <summary>
		/// Order by record proxied.
		/// </summary>
		[EnumMember(Value = "proxied")]
		Proxied = 5
	}
}
