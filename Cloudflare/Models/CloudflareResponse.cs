namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// The base Cloudflare response.
	/// Derived from <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L616">Source: Result</see>
	/// </summary>
	public class CloudflareResponse
	{
		/// <summary>
		/// Errors returned by the API call.
		/// </summary>
		[JsonProperty("errors")]
		public IReadOnlyCollection<ResponseInfo>? Errors { get; set; }

		/// <summary>
		/// Messages returned by the API call.
		/// </summary>
		[JsonProperty("messages")]
		public IReadOnlyCollection<ResponseInfo>? Messages { get; set; }

		/// <summary>
		/// Whether the API call was successful.
		/// </summary>
		[JsonProperty("success")]
		public bool Success { get; set; }
	}

	/// <inheritdoc/>
	/// <typeparam name="T">The result type.</typeparam>
	public class CloudflareResponse<T> : CloudflareResponse
	{
		/// <summary>
		/// The result of the API call.
		/// </summary>
		[JsonProperty("result")]
		public T? Result { get; set; }

		/// <summary>
		/// Information about the result of the request.
		/// </summary>
		[JsonProperty("result_info")]
		public PaginationInfo? ResultInfo { get; set; }
	}
}
