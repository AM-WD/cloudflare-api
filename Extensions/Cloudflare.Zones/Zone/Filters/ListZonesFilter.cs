using System;
using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Filter for listing zones.
	/// </summary>
	public class ListZonesFilter : IQueryParameterFilter
	{
		/// <summary>
		/// An account ID.
		/// </summary>
		/// <value>account.id</value>
		public string AccountId { get; set; }

		/// <summary>
		/// An account Name.
		/// </summary>
		/// <remarks>
		/// Optional filter operators can be provided to extend refine the search:
		/// <list type="bullet">
		/// 	<item><description>equal</description> (default)</item>
		/// 	<item><description>not_equal</description></item>
		/// 	<item><description>starts_with</description></item>
		/// 	<item><description>ends_with</description></item>
		/// 	<item><description>contains</description></item>
		/// 	<item><description>starts_with_case_sensitive</description></item>
		/// 	<item><description>ends_with_case_sensitive</description></item>
		/// 	<item><description>contains_case_sensitive</description></item>
		/// </list>
		/// </remarks>
		/// <example>Dev Account</example>
		/// <example>contains:Test</example>
		/// <value>account.name</value>
		public string AccountName { get; set; }

		/// <summary>
		/// Direction to order zones.
		/// </summary>
		/// <value>direction</value>
		public SortDirection? OrderDirection { get; set; }

		/// <summary>
		/// Whether to match all search requirements or at least one (any).
		/// </summary>
		/// <value>match</value>
		public FilterMatchType? MatchType { get; set; }

		/// <summary>
		/// A domain name.
		/// </summary>
		/// <remarks>
		/// Optional filter operators can be provided to extend refine the search:
		/// <list type="bullet">
		/// 	<item><description>equal</description> (default)</item>
		/// 	<item><description>not_equal</description></item>
		/// 	<item><description>starts_with</description></item>
		/// 	<item><description>ends_with</description></item>
		/// 	<item><description>contains</description></item>
		/// 	<item><description>starts_with_case_sensitive</description></item>
		/// 	<item><description>ends_with_case_sensitive</description></item>
		/// 	<item><description>contains_case_sensitive</description></item>
		/// </list>
		/// </remarks>
		/// <example>example.com</example>
		/// <example>contains:.org</example>
		/// <example>ends_with:arpa</example>
		/// <example>starts_with:dev</example>
		/// <value>name</value>
		public string Name { get; set; }

		/// <summary>
		/// Field to order zones by.
		/// </summary>
		/// <value>order</value>
		public ZonesOrderBy? OrderBy { get; set; }

		/// <summary>
		/// Page number of paginated results.
		/// </summary>
		/// <value>page</value>
		public int? Page { get; set; }

		/// <summary>
		/// Number of zones per page.
		/// </summary>
		/// <value>per_page</value>
		public int? PerPage { get; set; }

		/// <summary>
		/// A zone status.
		/// </summary>
		/// <value>status</value>
		public ZoneStatus? Status { get; set; }

		/// <inheritdoc />
		public IDictionary<string, string> GetQueryParameters()
		{
			var dict = new Dictionary<string, string>();

			if (!string.IsNullOrWhiteSpace(AccountId))
				dict.Add("account.id", AccountId);

			if (!string.IsNullOrWhiteSpace(AccountName))
				dict.Add("account.name", AccountName);

			if (OrderDirection.HasValue && Enum.IsDefined(typeof(SortDirection), OrderDirection.Value))
				dict.Add("direction", OrderDirection.Value.GetEnumMemberValue());

			if (MatchType.HasValue && Enum.IsDefined(typeof(FilterMatchType), MatchType.Value))
				dict.Add("match", MatchType.Value.GetEnumMemberValue());

			if (!string.IsNullOrWhiteSpace(Name))
				dict.Add("name", Name);

			if (OrderBy.HasValue && Enum.IsDefined(typeof(ZonesOrderBy), OrderBy.Value))
				dict.Add("order", OrderBy.Value.GetEnumMemberValue());

			if (Page.HasValue && Page.Value >= 1)
				dict.Add("page", Page.Value.ToString());

			if (PerPage.HasValue && PerPage.Value >= 5 && PerPage.Value <= 50)
				dict.Add("per_page", PerPage.Value.ToString());

			if (Status.HasValue && Enum.IsDefined(typeof(ZoneStatus), Status.Value))
				dict.Add("status", Status.Value.GetEnumMemberValue());

			return dict;
		}
	}
}
