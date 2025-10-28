namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Filter for DNS analytics report.
	/// </summary>
	public class GetDnsAnalyticsReportFilter : IQueryParameterFilter
	{
		/// <summary>
		/// A (comma-separated) list of dimensions to group results by.
		/// </summary>
		/// <remarks>
		/// Further see: <see href="https://developers.cloudflare.com/dns/reference/analytics-api-properties/#dimensions">Cloudflare Docs</see>.
		/// </remarks>
		public IReadOnlyCollection<string>? Dimensions { get; set; }

		/// <summary>
		/// Segmentation filter in 'attribute operator value' format.
		/// </summary>
		/// <remarks>
		/// Further see: <see href="https://developers.cloudflare.com/dns/reference/analytics-api-properties/#filters">Cloudflare Docs</see>.
		/// </remarks>
		public string? Filters { get; set; }

		/// <summary>
		/// Limit number of returned metrics. (Default: 100.000)
		/// </summary>
		public int? Limit { get; set; }

		/// <summary>
		/// A (comma-separated) list of metrics to query.
		/// </summary>
		public IReadOnlyCollection<string>? Metrics { get; set; }

		/// <summary>
		/// Start date and time of requesting data period in ISO 8601 format.
		/// </summary>
		public DateTime? Since { get; set; }

		/// <summary>
		/// A (comma-separated) list of dimensions to sort by, where each dimension may be prefixed by - (descending) or + (ascending)
		/// </summary>
		public IReadOnlyCollection<string>? Sort { get; set; }

		/// <summary>
		/// End date and time of requesting data period in ISO 8601 format.
		/// </summary>
		public DateTime? Until { get; set; }

		/// <inheritdoc/>
		public virtual IReadOnlyDictionary<string, string> GetQueryParameters()
		{
			var dict = new Dictionary<string, string>();

			if (Dimensions?.Count > 0)
				dict.Add("dimensions", string.Join(",", Dimensions));

			if (!string.IsNullOrWhiteSpace(Filters))
				dict.Add("filters", Filters!.Trim());

			if (Limit.HasValue && Limit > 0)
				dict.Add("limit", Limit.Value.ToString());

			if (Metrics?.Count > 0)
				dict.Add("metrics", string.Join(",", Metrics));

			if (Since.HasValue)
				dict.Add("since", Since.Value.ToIso8601Format());

			if (Sort?.Count > 0)
				dict.Add("sort", string.Join(",", Sort));

			if (Until.HasValue)
				dict.Add("until", Until.Value.ToIso8601Format());

			return dict;
		}
	}
}
