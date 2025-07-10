namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// A response info.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L600">Source</see>
	/// </summary>
	public class ResponseInfo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ResponseInfo"/> class.
		/// </summary>
		/// <param name="code">The response code.</param>
		/// <param name="message">The response message.</param>
		public ResponseInfo(int code, string message)
		{
			Code = code;
			Message = message;
		}

		/// <summary>
		/// The response code.
		/// </summary>
		[JsonProperty("code")]
		public int Code { get; set; }

		/// <summary>
		/// The response message.
		/// </summary>
		[JsonProperty("message")]
		public string Message { get; set; }

		/// <summary>
		/// The documentation URL.
		/// </summary>
		[JsonProperty("documentation_url")]
		public string? DocumentationUrl { get; set; }

		/// <summary>
		/// The response source.
		/// </summary>
		[JsonProperty("source")]
		public ResponseInfoSource? Source { get; set; }
	}

	/// <summary>
	/// A response info source.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L611">Source</see>
	/// </summary>
	public class ResponseInfoSource
	{
		/// <summary>
		/// The pointer.
		/// </summary>
		[JsonProperty("pointer")]
		public string? Pointer { get; set; }
	}
}
