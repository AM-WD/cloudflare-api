namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to update an internal DNS view.
	/// </summary>
	public class UpdateInternalDnsViewRequest : CreateInternalDnsViewRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UpdateInternalDnsViewRequest"/> class.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="viewId">The view identifier.</param>
		/// <param name="name">The name of the view.</param>
		public UpdateInternalDnsViewRequest(string accountId, string viewId, string name)
			: base(accountId, name)
		{
			ViewId = viewId;
		}

		/// <summary>
		/// The view identifier.
		/// </summary>
		public string ViewId { get; set; }
	}
}
