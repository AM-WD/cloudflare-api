using System;
using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Filter for listing DNS records.
	/// </summary>
	public class ListDnsRecordsFilter : IQueryParameterFilter
	{
		#region Comment

		/// <summary>
		/// Exact value of the DNS record comment.
		/// This is a convenience alias for <see cref="CommentExact"/>.
		/// </summary>
		public string? Comment { get; set; }

		/// <summary>
		/// If this parameter is present, only records <em>without</em> a comment are returned.
		/// </summary>
		public bool? CommentAbsent { get; set; }

		/// <summary>
		/// Substring of the DNS record comment.
		/// Comment filters are case-insensitive.
		/// </summary>
		public string? CommentContains { get; set; }

		/// <summary>
		/// Suffix of the DNS record comment.
		/// Comment filters are case-insensitive.
		/// </summary>
		public string? CommentEndsWith { get; set; }

		/// <summary>
		/// Exact value of the DNS record comment.
		/// Comment filters are case-insensitive.
		/// </summary>
		public string? CommentExact { get; set; }

		/// <summary>
		/// If this parameter is present, only records <em>with</em> a comment are returned.
		/// </summary>
		public bool? CommentPresent { get; set; }

		/// <summary>
		/// Prefix of the DNS record comment.
		/// Comment filters are case-insensitive.
		/// </summary>
		public string? CommentStartsWith { get; set; }

		#endregion Comment

		#region Content

		/// <summary>
		/// Exact value of the DNS record Content.
		/// This is a convenience alias for <see cref="ContentExact"/>.
		/// </summary>
		public string? Content { get; set; }

		/// <summary>
		/// Substring of the DNS record Content.
		/// Content filters are case-insensitive.
		/// </summary>
		public string? ContentContains { get; set; }

		/// <summary>
		/// Suffix of the DNS record Content.
		/// Content filters are case-insensitive.
		/// </summary>
		public string? ContentEndsWith { get; set; }

		/// <summary>
		/// Exact value of the DNS record Content.
		/// Content filters are case-insensitive.
		/// </summary>
		public string? ContentExact { get; set; }

		/// <summary>
		/// Prefix of the DNS record Content.
		/// Content filters are case-insensitive.
		/// </summary>
		public string? ContentStartsWith { get; set; }

		#endregion Content

		/// <summary>
		/// Direction to order DNS records in.
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
		/// <para>
		/// Note that the interaction between tag filters is controlled by the <see cref="TagMatch"/> parameter instead.
		/// </para>
		/// </remarks>
		public FilterMatchType? Match { get; set; }

		#region Name

		/// <summary>
		/// Exact value of the DNS record Name.
		/// This is a convenience alias for <see cref="NameExact"/>.
		/// </summary>
		public string? Name { get; set; }

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
		/// Field to order DNS records by.
		/// </summary>
		public DnsRecordsOrderBy? OrderBy { get; set; }

		/// <summary>
		/// Page number of paginated results.
		/// </summary>
		// >= 1
		public int? Page { get; set; }

		/// <summary>
		/// Number of DNS records per page.
		/// </summary>
		// >= 1 <= 5_000_000
		public int? PerPage { get; set; }

		/// <summary>
		/// Whether the record is receiving the performance and security benefits of Cloudflare.
		/// </summary>
		public bool? Proxied { get; set; }

		/// <summary>
		/// Allows searching in multiple properties of a DNS record simultaneously.
		/// </summary>
		/// <remarks>
		/// <para>
		/// This parameter is intended for human users, not automation.
		/// Its exact behavior is intentionally left unspecified and is subject to change in the future.
		/// </para>
		/// <para>
		/// <em>
		/// This parameter works independently of the <see cref="Match"/> setting.
		/// <br/>
		/// For automated searches, please use the other available parameters.
		/// </em>
		/// </para>
		/// </remarks>
		public string? Search { get; set; }

		#region Tag

		/// <summary>
		/// Condition on the DNS record tag.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Parameter values can be of the form <c>&lt;tag-name&gt;:&lt;tag-value&gt;</c> to search for an exact name:value pair,
		/// or just <c>&lt;tag-name&gt;</c> to search for records with a specific tag name regardless of its value.
		/// </para>
		/// <para>
		/// This is a convenience shorthand for the more powerful <c>tag.&lt;predicate&gt;</c> parameters.
		/// <br/>
		/// Examples:
		/// <list type="bullet">
		/// <item><c>tag=important</c> is equivalent to <c>tag.present=important</c></item>
		/// <item><c>tag=team:DNS</c> is equivalent to <c>tag.exact=team:DNS</c></item>
		/// </list>
		/// </para>
		/// </remarks>
		public string? Tag { get; set; }

		/// <summary>
		/// Name of a tag which must <em>not</em> be present on the DNS record.
		/// Tag filters are case-insensitive.
		/// </summary>
		public string? TagAbsent { get; set; }

		/// <summary>
		/// A tag and value, of the form <c>&lt;tag-name&gt;:&lt;tag-value&gt;</c>.
		/// Tag filters are case-insensitive.
		/// </summary>
		/// <remarks>
		/// The API will only return DNS records that have a tag named <c>&lt;tag-name&gt;</c> whose value contains <c>&lt;tag-value&gt;</c>.
		/// </remarks>
		public string? TagContains { get; set; }

		/// <summary>
		/// A tag and value, of the form <c>&lt;tag-name&gt;:&lt;tag-value&gt;</c>.
		/// Tag filters are case-insensitive.
		/// </summary>
		/// <remarks>
		/// The API will only return DNS records that have a tag named <c>&lt;tag-name&gt;</c> whose value ends with <c>&lt;tag-value&gt;</c>.
		/// </remarks>
		public string? TagEndsWith { get; set; }

		/// <summary>
		/// A tag and value, of the form <c>&lt;tag-name&gt;:&lt;tag-value&gt;</c>.
		/// Tag filters are case-insensitive.
		/// </summary>
		/// <remarks>
		/// The API will only return DNS records that have a tag named <c>&lt;tag-name&gt;</c> whose value is <c>&lt;tag-value&gt;</c>.
		/// </remarks>
		public string? TagExact { get; set; }

		/// <summary>
		/// Name of a tag which must be present on the DNS record.
		/// Tag filters are case-insensitive.
		/// </summary>
		public string? TagPresent { get; set; }

		/// <summary>
		/// A tag and value, of the form <c>&lt;tag-name&gt;:&lt;tag-value&gt;</c>.
		/// Tag filters are case-insensitive.
		/// </summary>
		/// <remarks>
		/// The API will only return DNS records that have a tag named <c>&lt;tag-name&gt;</c> whose value starts with <c>&lt;tag-value&gt;</c>.
		/// </remarks>
		public string? TagStartsWith { get; set; }

		#endregion Tag

		/// <summary>
		/// Whether to match all tag search requirements or at least one (any).
		/// </summary>
		/// <remarks>
		/// <para>
		/// If set to <see cref="FilterMatchType.All"/>, acts like a logical AND between filters.
		/// <br/>
		/// If set to <see cref="FilterMatchType.Any"/>, acts like a logical OR instead.
		/// </para>
		/// <para>
		/// Note that the regular <see cref="Match"/> parameter is still used to combine the resulting condition with other filters that aren't related to tags.
		/// </para>
		/// </remarks>
		public FilterMatchType? TagMatch { get; set; }

		/// <summary>
		/// Record type.
		/// </summary>
		public DnsRecordType? Type { get; set; }

		/// <inheritdoc />
		public IDictionary<string, string> GetQueryParameters()
		{
			var dict = new Dictionary<string, string>();

#pragma warning disable CS8602, CS8604 // There will be no null value below.

			#region Comment

			if (!string.IsNullOrWhiteSpace(Comment))
				dict.Add("comment", Comment.Trim());

			if (CommentAbsent.HasValue && CommentAbsent.Value)
				dict.Add("comment.absent", "true");

			if (!string.IsNullOrWhiteSpace(CommentContains))
				dict.Add("comment.contains", CommentContains.Trim());

			if (!string.IsNullOrWhiteSpace(CommentEndsWith))
				dict.Add("comment.endswith", CommentEndsWith.Trim());

			if (!string.IsNullOrWhiteSpace(CommentExact))
				dict.Add("comment.exact", CommentExact.Trim());

			if (CommentPresent.HasValue && CommentPresent.Value)
				dict.Add("comment.present", "true");

			if (!string.IsNullOrWhiteSpace(CommentStartsWith))
				dict.Add("comment.startswith", CommentStartsWith.Trim());

			#endregion Comment

			#region Content

			if (!string.IsNullOrWhiteSpace(Content))
				dict.Add("content", Content.Trim());

			if (!string.IsNullOrWhiteSpace(ContentContains))
				dict.Add("content.contains", ContentContains.Trim());

			if (!string.IsNullOrWhiteSpace(ContentEndsWith))
				dict.Add("content.endswith", ContentEndsWith.Trim());

			if (!string.IsNullOrWhiteSpace(ContentExact))
				dict.Add("content.exact", ContentExact.Trim());

			if (!string.IsNullOrWhiteSpace(ContentStartsWith))
				dict.Add("content.startswith", ContentStartsWith.Trim());

			#endregion Content

			if (Direction.HasValue && Enum.IsDefined(typeof(SortDirection), Direction.Value))
				dict.Add("direction", Direction.Value.GetEnumMemberValue());

			if (Match.HasValue && Enum.IsDefined(typeof(FilterMatchType), Match.Value))
				dict.Add("match", Match.Value.GetEnumMemberValue());

			#region Name

			if (!string.IsNullOrWhiteSpace(Name))
				dict.Add("name", Name.Trim());

			if (!string.IsNullOrWhiteSpace(NameContains))
				dict.Add("name.contains", NameContains.Trim());

			if (!string.IsNullOrWhiteSpace(NameEndsWith))
				dict.Add("name.endswith", NameEndsWith.Trim());

			if (!string.IsNullOrWhiteSpace(NameExact))
				dict.Add("name.exact", NameExact.Trim());

			if (!string.IsNullOrWhiteSpace(NameStartsWith))
				dict.Add("name.startswith", NameStartsWith.Trim());

			#endregion Name

			if (OrderBy.HasValue && Enum.IsDefined(typeof(DnsRecordsOrderBy), OrderBy.Value))
				dict.Add("order", OrderBy.Value.GetEnumMemberValue());

			if (Page.HasValue && Page.Value >= 1)
				dict.Add("page", Page.Value.ToString());

			if (PerPage.HasValue && PerPage.Value >= 1 && PerPage.Value <= 5_000_000)
				dict.Add("per_page", PerPage.Value.ToString());

			if (Proxied.HasValue)
				dict.Add("proxied", Proxied.Value.ToString().ToLowerInvariant());

			if (!string.IsNullOrWhiteSpace(Search))
				dict.Add("search", Search.Trim());

			#region Tag

			if (!string.IsNullOrWhiteSpace(Tag))
				dict.Add("tag", Tag.Trim());

			if (!string.IsNullOrWhiteSpace(TagAbsent))
				dict.Add("tag.absent", TagAbsent.Trim());

			if (!string.IsNullOrWhiteSpace(TagContains))
				dict.Add("tag.contains", TagContains.Trim());

			if (!string.IsNullOrWhiteSpace(TagEndsWith))
				dict.Add("tag.endswith", TagEndsWith.Trim());

			if (!string.IsNullOrWhiteSpace(TagExact))
				dict.Add("tag.exact", TagExact.Trim());

			if (!string.IsNullOrWhiteSpace(TagPresent))
				dict.Add("tag.present", TagPresent.Trim());

			if (!string.IsNullOrWhiteSpace(TagStartsWith))
				dict.Add("tag.startswith", TagStartsWith.Trim());

			#endregion Tag

			if (TagMatch.HasValue && Enum.IsDefined(typeof(FilterMatchType), TagMatch.Value))
				dict.Add("tag_match", TagMatch.Value.GetEnumMemberValue());

			if (Type.HasValue && Enum.IsDefined(typeof(DnsRecordType), Type.Value))
				dict.Add("type", Type.Value.GetEnumMemberValue());

#pragma warning restore CS8602, CS8604

			return dict;
		}
	}
}
