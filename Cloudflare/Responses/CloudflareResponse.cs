using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// The base Cloudflare response.
	/// </summary>
	public class CloudflareResponse
	{
		/// <summary>
		/// Information about the result of the request.
		/// </summary>
		[JsonProperty("result_info")]
		public PaginationInfo ResultInfo { get; set; }

		/// <summary>
		/// Whether the API call was successful.
		/// </summary>
		[JsonProperty("success")]
		public bool Success { get; set; }

		/// <summary>
		/// Errors returned by the API call.
		/// </summary>
		[JsonProperty("errors")]
		public IReadOnlyList<ResponseInfo> Errors { get; set; } = [];

		/// <summary>
		/// Messages returned by the API call.
		/// </summary>
		[JsonProperty("messages")]
		public IReadOnlyList<ResponseInfo> Messages { get; set; } = [];
	}

	/// <summary>
	/// The base Cloudflare response with a result.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CloudflareResponse<T> : CloudflareResponse
	{
		/// <summary>
		/// The result of the API call.
		/// </summary>
		[JsonProperty("result")]
		public T Result { get; set; }
	}
}
