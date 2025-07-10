using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Control options for the <strong>Security Level</strong> feature from the <strong>Security</strong> app.
	/// </summary>
	public class SecurityLevel : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SecurityLevel"/> class.
		/// </summary>
		public SecurityLevel()
		{
			Id = ZoneSettingId.SecurityLevel;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public SecurityLevelValue? Value { get; set; }
	}

	/// <summary>
	/// Security levels.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/settings.ts#L1223">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum SecurityLevelValue
	{
		/// <summary>
		/// Off.
		/// </summary>
		[EnumMember(Value = "off")]
		Off = 1,

		/// <summary>
		/// Essentially off.
		/// </summary>
		[EnumMember(Value = "essentially_off")]
		EssentiallyOff = 2,

		/// <summary>
		/// Low.
		/// </summary>
		[EnumMember(Value = "low")]
		Low = 3,

		/// <summary>
		/// Medium.
		/// </summary>
		[EnumMember(Value = "medium")]
		Medium = 4,

		/// <summary>
		/// High.
		/// </summary>
		[EnumMember(Value = "high")]
		High = 5,

		/// <summary>
		/// Under Attack.
		/// </summary>
		[EnumMember(Value = "under_attack")]
		UnderAttack = 6
	}
}
