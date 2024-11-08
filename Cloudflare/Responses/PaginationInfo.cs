namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// Cloudflare pagination information.
	/// </summary>
	public class PaginationInfo
	{
		/// <summary>
		/// Total number of results for the requested service.
		/// </summary>
		[JsonProperty("count")]
		public int Count { get; set; }

		/// <summary>
		/// Current page within paginated list of results.
		/// </summary>
		[JsonProperty("page")]
		public int Page { get; set; }

		/// <summary>
		/// Number of results per page of results.
		/// </summary>
		[JsonProperty("per_page")]
		public int PerPage { get; set; }

		/// <summary>
		/// Total results available without any search parameters.
		/// </summary>
		[JsonProperty("total_count")]
		public int TotalCount { get; set; }

		/// <summary>
		/// Total number of pages of results.
		/// </summary>
		[JsonProperty("total_pages")]
		public int TotalPages { get; set; }
	}
}
