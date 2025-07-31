namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// A Cloudflare internal DNS view.
	/// </summary>
	public class InternalDnsView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InternalDnsView"/> class.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="name">The name of the view.</param>
		public InternalDnsView(string id, string name)
		{
			Id = id;
			Name = name;
		}

		/// <summary>
		/// The identifier.
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// When the view was created.
		/// </summary>
		[JsonProperty("created_time")]
		public DateTime? CreatedTime { get; set; }

		/// <summary>
		/// When the view was last modified.
		/// </summary>
		[JsonProperty("modified_time")]
		public DateTime? ModifiedTime { get; set; }

		/// <summary>
		/// The name of the view.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// The list of zones linked to this view.
		/// </summary>
		[JsonProperty("zones")]
		public IReadOnlyCollection<string> ZoneIds { get; set; } = [];
	}
}
