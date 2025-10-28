namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Summarised aggregate metrics over a given time period as report.
	/// </summary>
	public class DnsAnalyticsByTime
	{
		/// <summary>
		/// Array with one row per combination of dimension values.
		/// </summary>
		[JsonProperty("data")]
		public IReadOnlyCollection<DnsAnalyticsByTimeData>? Data { get; set; }

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
		public DnsAnalyticsQuery? Query { get; set; }

		/// <summary>
		/// Total number of rows in the result.
		/// </summary>
		[JsonProperty("rows")]
		public int? Rows { get; set; }

		/// <summary>
		/// Array of time intervals in the response data. Each interval is represented as an
		/// array containing two values: the start time, and the end time.
		/// </summary>
		[JsonProperty("time_intervals")]
		public IReadOnlyCollection<IReadOnlyCollection<string>>? TimeIntervals { get; set; }

		/// <summary>
		/// Total results for metrics across all data (object mapping metric names to values).
		/// </summary>
		[JsonProperty("totals")]
		public object? Totals { get; set; }
	}

	/// <summary>
	/// A combination of dimension values.
	/// </summary>
	public class DnsAnalyticsByTimeData
	{
		/// <summary>
		/// Array of dimension values, representing the combination of dimension values corresponding to this row.
		/// </summary>
		[JsonProperty("dimensions")]
		public IReadOnlyCollection<string>? Dimensions { get; set; }

		/// <summary>
		/// Array with one item per requested metric. Each item is an array of values, broken down by time interval.
		/// </summary>
		[JsonProperty("metrics")]
		public IReadOnlyCollection<IReadOnlyCollection<object>>? Metrics { get; set; }
	}
}
