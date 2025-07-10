#if ! NET6_0_OR_GREATER

using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
	/// <summary>
	/// Extension methods for <see cref="HttpClient"/>s.
	/// </summary>
	/// <remarks>
	/// Copied from <see href="https://github.com/dotnet/runtime/blob/v6.0.0/src/libraries/System.Net.Http/src/System/Net/Http/HttpClient.cs">.NET 6 runtime / HttpClient</see>.
	/// </remarks>
	[Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	internal static class HttpClientExtensions
	{
		private static readonly HttpMethod _httpMethodPatch = new("PATCH");

		/// <summary>
		/// Sends a PATCH request with a cancellation token to a Uri represented as a string as an asynchronous operation.
		/// </summary>
		/// <param name="client">A <see cref="HttpClient"/> instance.</param>
		/// <param name="requestUri">The Uri the request is sent to.</param>
		/// <param name="content">The HTTP request content sent to the server.</param>
		/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, string? requestUri, HttpContent? content, CancellationToken cancellationToken) =>
			client.PatchAsync(CreateUri(requestUri), content, cancellationToken);

		/// <summary>
		/// Sends a PATCH request with a cancellation token as an asynchronous operation.
		/// </summary>
		/// <param name="client">A <see cref="HttpClient"/> instance.</param>
		/// <param name="requestUri">The Uri the request is sent to.</param>
		/// <param name="content">The HTTP request content sent to the server.</param>
		/// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
		/// <returns>The task object representing the asynchrnous operation.</returns>
		public static Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri? requestUri, HttpContent? content, CancellationToken cancellationToken)
		{
			var request = new HttpRequestMessage(_httpMethodPatch, requestUri)
			{
				Version = HttpVersion.Version11,
				Content = content,
			};
			return client.SendAsync(request, cancellationToken);
		}

		private static Uri? CreateUri(string? uri) =>
			string.IsNullOrEmpty(uri) ? null : new Uri(uri, UriKind.RelativeOrAbsolute);
	}
}

#endif
