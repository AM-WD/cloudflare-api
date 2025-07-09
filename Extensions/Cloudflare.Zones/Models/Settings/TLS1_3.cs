using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Enables Crypto TLS 1.3 feature for a zone.
	/// </summary>
	public class TLS1_3 : ZoneSettingBase
	{
		/// <summary>
		/// Enables Crypto TLS 1.3 feature for a zone.
		/// </summary>
		public TLS1_3()
		{
			Id = ZoneSettingId.TLS1_3;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public TlsOption Value { get; set; }
	}

	/// <summary>
	/// Available TLS options.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/settings.ts#L1352">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TlsOption
	{
		/// <summary>
		/// Off.
		/// </summary>
		[EnumMember(Value = "off")]
		Off = 1,

		/// <summary>
		/// On.
		/// </summary>
		[EnumMember(Value = "on")]
		On = 2,

		/// <summary>
		/// ZRT refers to <see href="https://blog.cloudflare.com/introducing-0-rtt/">Zero Round Trip Time Resumption (0-RTT)</see>.
		/// </summary>
		[EnumMember(Value = "zrt")]
		ZeroRoundTrip = 3
	}
}
