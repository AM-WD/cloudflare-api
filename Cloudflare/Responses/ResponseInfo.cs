namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// A Cloudflare response information.
	/// </summary>
	public class ResponseInfo
	{
		/// <summary>
		/// The message code.
		/// </summary>
		[JsonProperty("code")]
		public int Code { get; set; }

		/// <summary>
		/// The message.
		/// </summary>
		[JsonProperty("message")]
		public string Message { get; set; }
	}
}
