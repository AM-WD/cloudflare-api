using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Value states on/off/custom.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/settings.ts#L591">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum OnOffCustomState
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
		Off = 2,

		/// <summary>
		/// Custom state.
		/// </summary>
		[EnumMember(Value = "custom")]
		Custom = 3
	}
}
