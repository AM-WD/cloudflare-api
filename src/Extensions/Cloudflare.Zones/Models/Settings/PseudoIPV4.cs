using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// The value set for the Pseudo IPv4 setting.
	/// </summary>
	public class PseudoIPV4 : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PseudoIPV4"/> class.
		/// </summary>
		public PseudoIPV4()
		{
			Id = ZoneSettingId.PseudoIPV4;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public PseudoIPV4Value Value { get; set; }
	}

	/// <summary>
	/// Pseudo IPv4 values.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/settings.ts#L1081">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum PseudoIPV4Value
	{
		/// <summary>
		/// Off.
		/// </summary>
		[EnumMember(Value = "off")]
		Off = 1,

		/// <summary>
		/// Add a header.
		/// </summary>
		[EnumMember(Value = "add_header")]
		AddHeader = 2,

		/// <summary>
		/// Add header or overwrite if exists.
		/// </summary>
		[EnumMember(Value = "overwrite_header")]
		OverwriteHeader = 3
	}
}
