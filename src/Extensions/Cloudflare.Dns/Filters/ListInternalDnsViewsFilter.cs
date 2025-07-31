using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Filter for listing internal DNS views.
	/// </summary>
	public class ListInternalDnsViewsFilter : IQueryParameterFilter
	{
		/// <summary>
		/// Direction to order DNS views in.
		/// </summary>
		public SortDirection? Direction { get; set; }

		/// <summary>
		/// Whether to match all search requirements or at least one (any).
		/// </summary>
		/// <remarks>
		/// <para>
		/// If set to <see cref="FilterMatchType.All"/>, acts like a logical AND between filters.
		/// <br/>
		/// If set to <see cref="FilterMatchType.Any"/>, acts like a logical OR instead.
		/// </para>
		/// </remarks>
		public FilterMatchType? Match { get; set; }

		#region Name

		/// <summary>
		/// Substring of the DNS record Name.
		/// Name filters are case-insensitive.
		/// </summary>
		public string? NameContains { get; set; }

		/// <summary>
		/// Suffix of the DNS record Name.
		/// Name filters are case-insensitive.
		/// </summary>
		public string? NameEndsWith { get; set; }

		/// <summary>
		/// Exact value of the DNS record Name.
		/// Name filters are case-insensitive.
		/// </summary>
		public string? NameExact { get; set; }

		/// <summary>
		/// Prefix of the DNS record Name.
		/// Name filters are case-insensitive.
		/// </summary>
		public string? NameStartsWith { get; set; }

		#endregion Name

		/// <summary>
		/// Field to order DNS views by.
		/// </summary>
		public InternalDnsViewsOrderBy? OrderBy { get; set; }

		/// <summary>
		/// Page number of paginated results.
		/// </summary>
		/// <value>1 &lt;= X</value>
		public int? Page { get; set; }

		/// <summary>
		/// Number of DNS records per page.
		/// </summary>
		/// <value>1 &lt;= X &lt;= 5,000,000</value>
		public int? PerPage { get; set; }

		/// <summary>
		/// A zone ID that exists in the zones list for the view.
		/// </summary>
		public string? ZoneId { get; set; }

		/// <summary>
		/// A zone name that exists in the zones list for the view.
		/// </summary>
		public string? ZoneName { get; set; }

		/// <inheritdoc />
		public IReadOnlyDictionary<string, string> GetQueryParameters()
		{
			var dict = new Dictionary<string, string>();

#pragma warning disable CS8602, CS8604 // There will be no null value below.

			if (Direction.HasValue && Enum.IsDefined(typeof(SortDirection), Direction.Value))
				dict.Add("direction", Direction.Value.GetEnumMemberValue());

			if (Match.HasValue && Enum.IsDefined(typeof(FilterMatchType), Match.Value))
				dict.Add("match", Match.Value.GetEnumMemberValue());

			if (!string.IsNullOrWhiteSpace(NameContains))
				dict.Add("name.contains", NameContains.Trim());

			if (!string.IsNullOrWhiteSpace(NameEndsWith))
				dict.Add("name.endswith", NameEndsWith.Trim());

			if (!string.IsNullOrWhiteSpace(NameExact))
				dict.Add("name.exact", NameExact.Trim());

			if (!string.IsNullOrWhiteSpace(NameStartsWith))
				dict.Add("name.startswith", NameStartsWith.Trim());

			if (OrderBy.HasValue && Enum.IsDefined(typeof(InternalDnsViewsOrderBy), OrderBy.Value))
				dict.Add("order", OrderBy.Value.GetEnumMemberValue());

			if (Page.HasValue && Page.Value >= 1)
				dict.Add("page", Page.Value.ToString());

			if (PerPage.HasValue && PerPage.Value >= 1 && PerPage.Value <= 5_000_000)
				dict.Add("per_page", PerPage.Value.ToString());

			if (!string.IsNullOrWhiteSpace(ZoneId))
				dict.Add("zone_id", ZoneId.Trim());

			if (!string.IsNullOrWhiteSpace(ZoneName))
				dict.Add("zone_name", ZoneName.Trim());

#pragma warning restore CS8602, CS8604

			return dict;
		}
	}

	/// <summary>
	/// Possible fields to order internal DNS views by.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum InternalDnsViewsOrderBy
	{
		/// <summary>
		/// Order by name.
		/// </summary>
		[EnumMember(Value = "name")]
		Name = 1,

		/// <summary>
		/// Order by creation date.
		/// </summary>
		[EnumMember(Value = "created_on")]
		CreatedOn = 2,

		/// <summary>
		/// Order by last modified date.
		/// </summary>
		[EnumMember(Value = "modified_on")]
		ModifiedOn = 3
	}
}
