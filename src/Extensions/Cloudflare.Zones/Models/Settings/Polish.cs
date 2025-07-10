using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Apply options from the Polish feature of the Cloudflare Speed app.
	/// </summary>
	public class Polish : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Polish"/> class.
		/// </summary>
		public Polish()
		{
			Id = ZoneSettingId.Polish;
		}

		/// <summary>
		/// The level of Polish you want applied to your origin.
		/// </summary>
		[JsonProperty("value")]
		public PolishLevel? Value { get; set; }
	}

	/// <summary>
	/// The level of Polish.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/settings.ts#L1001">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum PolishLevel
	{
		/// <summary>
		/// No Polish.
		/// </summary>
		[EnumMember(Value = "off")]
		Off = 1,

		/// <summary>
		/// Basic Polish.
		/// </summary>
		[EnumMember(Value = "lossless")]
		LossLess = 2,

		/// <summary>
		/// Full Polish.
		/// </summary>
		[EnumMember(Value = "lossy")]
		Lossy = 3
	}
}
