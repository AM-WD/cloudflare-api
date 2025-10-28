namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Summarised aggregate metrics over a given time period as report.
	/// </summary>
	public class DnsAnalyticsReport
	{
		/// <summary>
		/// Array with one row per combination of dimension values.
		/// </summary>
		[JsonProperty("data")]
		public IReadOnlyCollection<DnsAnalyticsReportData>? Data { get; set; }

		/// <summary>
		/// Number of seconds between current time and last processed event, in another words how many seconds of data could be missing.
		/// </summary>
		[JsonProperty("data_lag")]
		public int? DataLag { get; set; }

		/// <summary>
		/// Maximum results for each metric (object mapping metric names to values).
		/// Currently always an empty object.
		/// </summary>
		[JsonProperty("max")]
		public object? Max { get; set; }

		/// <summary>
		/// Minimum results for each metric (object mapping metric names to values).
		/// Currently always an empty object.
		/// </summary>
		[JsonProperty("min")]
		public object? Min { get; set; }

		/// <summary>
		/// The query information.
		/// </summary>
		[JsonProperty("query")]
		public DnsAnalyticsReportQuery? Query { get; set; }

		/// <summary>
		/// Total number of rows in the result.
		/// </summary>
		[JsonProperty("rows")]
		public int? Rows { get; set; }

		/// <summary>
		/// Total results for metrics across all data (object mapping metric names to values).
		/// </summary>
		[JsonProperty("totals")]
		public object? Totals { get; set; }
	}

	/// <summary>
	/// A combination of dimension values.
	/// </summary>
	public class DnsAnalyticsReportData
	{
		/// <summary>
		/// Array of dimension values, representing the combination of dimension values corresponding to this row.
		/// </summary>
		[JsonProperty("dimensions")]
		public IReadOnlyCollection<string>? Dimensions { get; set; }

		/// <summary>
		/// Array with one item per requested metric. Each item is a single value.
		/// </summary>
		[JsonProperty("metrics")]
		public IReadOnlyCollection<int>? Metrics { get; set; }
	}

	/// <summary>
	/// The query information.
	/// </summary>
	public class DnsAnalyticsReportQuery
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
		/// Start date and time of requesting data period in ISO 8601 format.
		/// </summary>
		[JsonProperty("since")]
		public DateTime? Since { get; set; }

		/// <summary>
		/// End date and time of requesting data period in ISO 8601 format.
		/// </summary>
		[JsonProperty("until")]
		public DateTime? Until { get; set; }

		/// <summary>
		/// Segmentation filter in 'attribute operator value' format.
		/// </summary>
		[JsonProperty("filters")]
		public string? Filters { get; set; }

		/// <summary>
		/// Array of dimension names to sort by, where each dimension may be prefixed by - (descending) or + (ascending).
		/// </summary>
		[JsonProperty("sort")]
		public IReadOnlyCollection<string>? Sort { get; set; }
	}
}
