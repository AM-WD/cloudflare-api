using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Zone types.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/zones.ts#L223">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ZoneType
	{
		/// <summary>
		/// Full Setup (most common).
		/// </summary>
		/// <remarks>
		/// Use Cloudflare as your primary DNS provider and manage your DNS records on Cloudflare.
		/// </remarks>
		[EnumMember(Value = "full")]
		Full = 1,

		/// <summary>
		/// Partial (CNAME) setup.
		/// </summary>
		/// <remarks>
		/// Keep your primary DNS provider and only use Cloudflare's reverse proxy for individual subdomains.
		/// </remarks>
		[EnumMember(Value = "partial")]
		Partial = 2,

		/// <summary>
		/// Zone transfers.
		/// </summary>
		/// <remarks>
		/// Use Cloudflare and another DNS provider together across your entire zone to increase availability and fault tolerance.
		/// <br/>
		/// DNS records will be transferred between providers using <see href="https://datatracker.ietf.org/doc/html/rfc5936">AXFR</see> or <see href="https://datatracker.ietf.org/doc/html/rfc1995">IXFR</see>.
		/// </remarks>
		[EnumMember(Value = "secondary")]
		Secondary = 3,

		/// <summary>
		/// An internal zone.
		/// </summary>
		[EnumMember(Value = "internal")]
		Internal = 4
	}
}
