using Newtonsoft.Json.Linq;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// A DNS record.
	/// </summary>
	public abstract class DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DnsRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		protected DnsRecord(string name)
		{
			Name = name;
		}

		/// <summary>
		/// DNS record name (or @ for the zone apex) in Punycode.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// Record type.
		/// </summary>
		[JsonProperty("type")]
		public DnsRecordType Type { get; protected set; }

		/// <summary>
		/// Comments or notes about the DNS record.
		/// This field has no effect on DNS responses.
		/// </summary>
		[JsonProperty("comment")]
		public string? Comment { get; set; }

		/// <summary>
		/// DNS record content.
		/// </summary>
		[JsonProperty("content")]
		public string? Content { get; set; }

		/// <summary>
		/// Whether the record is receiving the performance and security benefits of
		/// Cloudflare.
		/// </summary>
		[JsonProperty("proxied")]
		public bool? Proxied { get; set; }

		/// <summary>
		/// Settings for the DNS record.
		/// </summary>
		[JsonProperty("settings")]
		public DnsRecordSettings? Settings { get; set; }

		/// <summary>
		/// Custom tags for the DNS record.
		/// This field has no effect on DNS responses.
		/// </summary>
		[JsonProperty("tags")]
		public IReadOnlyCollection<string>? Tags { get; set; }

		/// <summary>
		/// Time To Live (TTL) of the DNS record in seconds.
		/// </summary>
		/// <remarks>
		/// Setting to <c>1</c> means 'automatic'. Value must be between <c>60</c> and <c>86400</c>, with the
		/// minimum reduced to <c>30</c> for Enterprise zones.
		/// </remarks>
		[JsonProperty("ttl")]
		public int? TimeToLive { get; set; }

		#region Response

		/// <summary>
		/// Identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// When the record was created.
		/// </summary>
		[JsonProperty("created_on")]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Extra Cloudflare-specific information about the record.
		/// </summary>
		[JsonProperty("meta")]
		public JToken? Meta { get; set; }

		/// <summary>
		/// When the record was last modified.
		/// </summary>
		[JsonProperty("modified_on")]
		public DateTime ModifiedOn { get; set; }

		/// <summary>
		/// Whether the record can be proxied by Cloudflare or not.
		/// </summary>
		[JsonProperty("proxiable")]
		public bool Proxiable { get; set; }

		/// <summary>
		/// When the record comment was last modified. Omitted if there is no comment.
		/// </summary>
		[JsonProperty("comment_modified_on")]
		public DateTime? CommentModifiedOn { get; set; }

		/// <summary>
		/// When the record tags were last modified. Omitted if there are no tags.
		/// </summary>
		[JsonProperty("tags_modified_on")]
		public DateTime? TagsModifiedOn { get; set; }

		#endregion Response
	}
}
