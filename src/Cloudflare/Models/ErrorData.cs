namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// An error message.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L250">Source</see>
	/// </summary>
	public class ErrorData
	{
		/// <summary>
		/// The error code.
		/// </summary>
		[JsonProperty("code")]
		public int? Code { get; set; }

		/// <summary>
		/// A link to documentation about the error.
		/// </summary>
		[JsonProperty("documentation_url")]
		public string? DocumentationUrl { get; set; }

		/// <summary>
		/// The error message.
		/// </summary>
		[JsonProperty("message")]
		public string? Message { get; set; }

		/// <summary>
		/// The source of the error.
		/// </summary>
		[JsonProperty("source")]
		public ErrorDataSource? Source { get; set; }
	}

	/// <summary>
	/// The source of the error.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L261">Source</see>
	/// </summary>
	public class ErrorDataSource
	{
		/// <summary>
		/// The pointer to the source of the error.
		/// </summary>
		[JsonProperty("pointer")]
		public string? Pointer { get; set; }
	}
}
