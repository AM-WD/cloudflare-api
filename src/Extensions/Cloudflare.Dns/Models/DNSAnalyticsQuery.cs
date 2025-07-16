using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	internal class DNSAnalyticsQuery
	{
		/// <summary>
		/// Array of dimension names.
		/// </summary>
		[JsonProperty("dimensions")]
		public IReadOnlyCollection<string>? Dimensions { get; set; }

		/// <summary>
		/// Limit number of returned metrics.
		/// </summary>
		[JsonProperty("limit")]
		public int? Limit { get; set; }

		/// <summary>
		/// Array of metric names.
		/// </summary>
		[JsonProperty("metrics")]
		public IReadOnlyCollection<string>? Metrics { get; set; }

		/// <summary>
		/// Start date and time of requesting data period.
		/// </summary>
		[JsonProperty("since")]
		public DateTime? Since { get; set; }

		/// <summary>
		/// Unit of time to group data by.
		/// </summary>
		[JsonProperty("time_delta")]
		public TimeDeltaUnit? TimeDelta { get; set; }

		[JsonProperty("until")]
		public DateTime? Until { get; set; }

		/// <summary>
		/// Segmentation filter in 'attribute operator value' format.
		/// </summary>
		[JsonProperty("filters")]
		public string? Filters { get; set; }

		/// <summary>
		/// Array of dimensions to sort by, where each dimension may be prefixed
		/// by <c>-</c> (descending) or <c>+</c> (ascending).
		/// </summary>
		[JsonProperty("sort")]
		public IReadOnlyCollection<string>? Sort { get; set; }
	}

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
		/// Quarter.
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
		/// Dekaminute.
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
