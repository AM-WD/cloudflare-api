namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to create an internal DNS view.
	/// </summary>
	public class CreateInternalDnsViewRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateInternalDnsViewRequest"/> class.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="name">The name of the view.</param>
		public CreateInternalDnsViewRequest(string accountId, string name)
		{
			AccountId = accountId;
			Name = name;
		}

		/// <summary>
		/// The account identifier.
		/// </summary>
		public string AccountId { get; set; }

		/// <summary>
		/// The name of the view.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The list of zones linked to this view.
		/// </summary>
		public IReadOnlyCollection<string> ZoneIds { get; set; } = [];
	}
}
