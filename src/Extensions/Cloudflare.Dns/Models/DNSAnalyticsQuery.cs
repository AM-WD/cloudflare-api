namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// The DNS Analytics query.
	/// </summary>
	public class DnsAnalyticsQuery
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

		/// <summary>
		/// End date and time of requesting data period.
		/// </summary>
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
}
