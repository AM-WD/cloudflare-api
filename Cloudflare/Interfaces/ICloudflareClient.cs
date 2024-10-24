using System.Threading;
using System.Threading.Tasks;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// Represents a client for the Cloudflare API.
	/// </summary>
	public interface ICloudflareClient
	{
		/// <summary>
		/// Makes a GET request to the Cloudflare API.
		/// </summary>
		/// <remarks>
		/// The GET method requests a representation of the specified resource.
		/// Requests using GET should only retrieve data and should not contain a request content.
		/// </remarks>
		/// <typeparam name="TResponse">The response type.</typeparam>
		/// <param name="requestPath">The request path (extending the base URL).</param>
		/// <param name="queryFilter">The query parameters.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		Task<CloudflareResponse<TResponse>> GetAsync<TResponse>(string requestPath, IQueryParameterFilter queryFilter = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Makes a POST request to the Cloudflare API.
		/// </summary>
		/// <remarks>
		/// The POST method submits an entity to the specified resource, often causing a change in state or side effects on the server.
		/// </remarks>
		/// <typeparam name="TResponse">The response type.</typeparam>
		/// <typeparam name="TRequest">The request type.</typeparam>
		/// <param name="requestPath">The request path (extending the base URL).</param>
		/// <param name="request">The request content.</param>
		/// <param name="queryFilter">The query parameters.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		Task<CloudflareResponse<TResponse>> PostAsync<TResponse, TRequest>(string requestPath, TRequest request, IQueryParameterFilter queryFilter = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Makes a PUT request to the Cloudflare API.
		/// </summary>
		/// <remarks>
		/// The PUT method replaces all current representations of the target resource with the request content.
		/// </remarks>
		/// <typeparam name="TResponse">The response type.</typeparam>
		/// <typeparam name="TRequest">The request type.</typeparam>
		/// <param name="requestPath">The request path (extending the base URL).</param>
		/// <param name="request">The request content.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		Task<CloudflareResponse<TResponse>> PutAsync<TResponse, TRequest>(string requestPath, TRequest request, CancellationToken cancellationToken = default);

		/// <summary>
		/// Makes a DELETE request to the Cloudflare API.
		/// </summary>
		/// <remarks>
		/// The DELETE method deletes the specified resource.
		/// </remarks>
		/// <typeparam name="TResponse">The response type.</typeparam>
		/// <param name="requestPath">The request path (extending the base URL).</param>
		/// <param name="queryFilter">The query parameters.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		/// <returns></returns>
		Task<CloudflareResponse<TResponse>> DeleteAsync<TResponse>(string requestPath, IQueryParameterFilter queryFilter = null, CancellationToken cancellationToken = default);

		/// <summary>
		/// Makes a PATCH request to the Cloudflare API.
		/// </summary>
		/// <remarks>
		/// The PATCH method applies partial modifications to a resource.
		/// </remarks>
		/// <typeparam name="TResponse">The response type.</typeparam>
		/// <typeparam name="TRequest">The request type.</typeparam>
		/// <param name="requestPath">The request path (extending the base URL).</param>
		/// <param name="request">The request content.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		Task<CloudflareResponse<TResponse>> PatchAsync<TResponse, TRequest>(string requestPath, TRequest request, CancellationToken cancellationToken = default);
	}
}
