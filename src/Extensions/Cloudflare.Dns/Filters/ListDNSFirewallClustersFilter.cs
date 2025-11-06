namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a filter for querying DNS firewall clusters with optional pagination parameters.
	/// </summary>
	public class ListDNSFirewallClustersFilter : IQueryParameterFilter
	{
		/// <summary>
		/// Page number of paginated results.
		/// </summary>
		public int? Page { get; set; }

		/// <summary>
		/// Number of clusters per page.
		/// </summary>
		public int? PerPage { get; set; }

		/// <inheritdoc/>
		public IReadOnlyDictionary<string, string> GetQueryParameters()
		{
			var dict = new Dictionary<string, string>();

			if (Page.HasValue && 1 <= Page.Value)
				dict.Add("page", Page.Value.ToString());

			if (PerPage.HasValue && 1 <= PerPage.Value && PerPage.Value <= 100)
				dict.Add("per_page", PerPage.Value.ToString());

			return dict;
		}
	}
}
