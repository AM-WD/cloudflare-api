namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to manipulate DNS records.
	/// </summary>
	public class BatchDnsRecordsRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BatchDnsRecordsRequest"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		public BatchDnsRecordsRequest(string zoneId)
		{
			ZoneId = zoneId;
		}

		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; }

		/// <summary>
		/// The DNS records to delete.
		/// </summary>
		public IReadOnlyCollection<string> Deletes { get; set; } = [];

		/// <summary>
		/// The DNS records to update.
		/// </summary>
		public IReadOnlyCollection<Patch> Updates { get; set; } = [];

		/// <summary>
		/// The DNS records to create.
		/// </summary>
		public IReadOnlyCollection<Post> Creates { get; set; } = [];

		/// <summary>
		/// The DNS records to overwrite.
		/// </summary>
		public IReadOnlyCollection<Put> Overwrites { get; set; } = [];

		/// <summary>
		/// Represents a request to update a DNS record.
		/// </summary>
		public class Patch : Post
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="Patch"/> class.
			/// </summary>
			/// <param name="id">The DNS record identifier.</param>
			/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
			public Patch(string id, string name)
				: base(name)
			{
				Id = id;
			}

			/// <summary>
			/// The DNS record identifier.
			/// </summary>
			public string Id { get; set; }
		}

		/// <summary>
		/// Represents a request to create a DNS record.
		/// </summary>
		public class Post
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="Post"/> class.
			/// </summary>
			/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
			public Post(string name)
			{
				Name = name;
			}

			/// <summary>
			/// DNS record name (or @ for the zone apex) in Punycode.
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Time To Live (TTL) of the DNS record in seconds.
			/// </summary>
			/// <remarks>
			/// Setting to <c>1</c> means 'automatic'. Value must be between <c>60</c> and <c>86400</c>, with the
			/// minimum reduced to <c>30</c> for Enterprise zones.
			/// </remarks>
			public int? TimeToLive { get; set; }

			/// <summary>
			/// DNS record type.
			/// </summary>
			public DnsRecordType Type { get; set; }

			/// <summary>
			/// Comments or notes about the DNS record.
			/// This field has no effect on DNS responses.
			/// </summary>
			public string? Comment { get; set; }

			/// <summary>
			/// The content of the DNS record.
			/// </summary>
			public string? Content { get; set; }

			/// <summary>
			/// Components of a record.
			/// </summary>
			public object? Data { get; set; }

			/// <summary>
			/// Required for MX, SRV and URI records; unused by other record types.
			/// Records with lower priorities are preferred.
			/// </summary>
			public int? Priority { get; set; }

			/// <summary>
			/// Whether the record is receiving the performance and security benefits of
			/// Cloudflare.
			/// </summary>
			public bool? Proxied { get; set; }

			/// <summary>
			/// Settings for the DNS record.
			/// </summary>
			public DnsRecordSettings? Settings { get; set; }

			/// <summary>
			/// Custom tags for the DNS record.
			/// This field has no effect on DNS responses.
			/// </summary>
			public IReadOnlyCollection<string>? Tags { get; set; }
		}

		/// <summary>
		/// Represents a request to overwrite a DNS record.
		/// </summary>
		public class Put : Post
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="Put"/> class.
			/// </summary>
			/// <param name="id">The DNS record identifier.</param>
			/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
			public Put(string id, string name)
				: base(name)
			{
				Id = id;
			}

			/// <summary>
			/// The DNS record identifier.
			/// </summary>
			public string Id { get; set; }
		}
	}
}
