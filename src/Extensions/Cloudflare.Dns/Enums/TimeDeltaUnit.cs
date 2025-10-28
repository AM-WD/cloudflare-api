using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Time delta units.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/dns/dns.ts#L103">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TimeDeltaUnit
	{
		/// <summary>
		/// All time.
		/// </summary>
		[EnumMember(Value = "all")]
		All = 1,

		/// <summary>
		/// Auto.
		/// </summary>
		[EnumMember(Value = "auto")]
		Auto = 2,

		/// <summary>
		/// Year.
		/// </summary>
		[EnumMember(Value = "year")]
		Year = 3,

		/// <summary>
		/// Quarter (3 months).
		/// </summary>
		[EnumMember(Value = "quarter")]
		Quarter = 4,

		/// <summary>
		/// Month.
		/// </summary>
		[EnumMember(Value = "month")]
		Month = 5,

		/// <summary>
		/// Week.
		/// </summary>
		[EnumMember(Value = "week")]
		Week = 6,

		/// <summary>
		/// Day.
		/// </summary>
		[EnumMember(Value = "day")]
		Day = 7,

		/// <summary>
		/// Hour.
		/// </summary>
		[EnumMember(Value = "hour")]
		Hour = 8,

		/// <summary>
		/// Dekaminute (10 minutes).
		/// </summary>
		[EnumMember(Value = "dekaminute")]
		DekaMinute = 9,

		/// <summary>
		/// Minute.
		/// </summary>
		[EnumMember(Value = "minute")]
		Minute = 10
	}
}
