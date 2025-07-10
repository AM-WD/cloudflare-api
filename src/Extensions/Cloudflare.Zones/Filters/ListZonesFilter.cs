using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

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
		public string? AccountId { get; set; }

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
		public string? AccountName { get; set; }

		/// <summary>
		/// Direction to order zones.
		/// </summary>
		public SortDirection? Direction { get; set; }

		/// <summary>
		/// Whether to match all search requirements or at least one (any).
		/// </summary>
		public ListZonesMatch? Match { get; set; }

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
		public string? Name { get; set; }

		/// <summary>
		/// Field to order zones by.
		/// </summary>
		public ListZonesOrderBy? OrderBy { get; set; }

		/// <summary>
		/// Page number of paginated results.
		/// </summary>
		/// <value>1 &lt;= X</value>
		public int? Page { get; set; }

		/// <summary>
		/// Number of zones per page.
		/// </summary>
		/// <value>5 &lt;= X &lt;= 50</value>
		public int? PerPage { get; set; }

		/// <summary>
		/// A zone status.
		/// </summary>
		public ZoneStatus? Status { get; set; }

		/// <inheritdoc />
		public IDictionary<string, string> GetQueryParameters()
		{
			var dict = new Dictionary<string, string>();

#pragma warning disable CS8602, CS8604 // There will be no null value below.

			if (!string.IsNullOrWhiteSpace(AccountId))
				dict.Add("account.id", AccountId.Trim());

			if (!string.IsNullOrWhiteSpace(AccountName))
				dict.Add("account.name", AccountName.Trim());

			if (Direction.HasValue && Enum.IsDefined(typeof(SortDirection), Direction.Value))
				dict.Add("direction", Direction.Value.GetEnumMemberValue());

			if (Match.HasValue && Enum.IsDefined(typeof(ListZonesMatch), Match.Value))
				dict.Add("match", Match.Value.GetEnumMemberValue());

			if (!string.IsNullOrWhiteSpace(Name))
				dict.Add("name", Name);

			if (OrderBy.HasValue && Enum.IsDefined(typeof(ListZonesOrderBy), OrderBy.Value))
				dict.Add("order", OrderBy.Value.GetEnumMemberValue());

			if (Page.HasValue && Page.Value >= 1)
				dict.Add("page", Page.Value.ToString());

			if (PerPage.HasValue && PerPage.Value >= 5 && PerPage.Value <= 50)
				dict.Add("per_page", PerPage.Value.ToString());

			if (Status.HasValue && Enum.IsDefined(typeof(ZoneStatus), Status.Value))
				dict.Add("status", Status.Value.GetEnumMemberValue());

#pragma warning restore CS8602, CS8604

			return dict;
		}
	}

	/// <summary>
	/// Match type for listing zones.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/zones.ts#L553">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ListZonesMatch
	{
		/// <summary>
		/// Match all search requirements.
		/// </summary>
		[EnumMember(Value = "all")]
		All = 1,

		/// <summary>
		/// Match at least one search requirement.
		/// </summary>
		[EnumMember(Value = "any")]
		Any = 2
	}

	/// <summary>
	/// Field to order zones by.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/zones.ts#L573">Source</see>
	/// </summary>
	public enum ListZonesOrderBy
	{
		/// <summary>
		/// Order by zone name.
		/// </summary>
		[EnumMember(Value = "name")]
		Name = 1,

		/// <summary>
		/// Order by zone status.
		/// </summary>
		[EnumMember(Value = "status")]
		Status = 2,

		/// <summary>
		/// Order by account ID.
		/// </summary>
		[EnumMember(Value = "account.id")]
		AccountId = 3,

		/// <summary>
		/// Order by account name.
		/// </summary>
		[EnumMember(Value = "account.name")]
		AccountName = 4,
	}
}
