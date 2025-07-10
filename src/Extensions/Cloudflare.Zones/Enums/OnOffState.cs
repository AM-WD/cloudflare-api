using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Value states on/off.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/settings.ts#L71">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum OnOffState
	{
		/// <summary>
		/// On state.
		/// </summary>
		[EnumMember(Value = "on")]
		On = 1,

		/// <summary>
		/// Off state.
		/// </summary>
		[EnumMember(Value = "off")]
		Off = 2
	}
}
