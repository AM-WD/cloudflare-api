using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Auth;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// Implements the Core of the Cloudflare API client.
	/// </summary>
	public class CloudflareClient : ICloudflareClient, IDisposable
	{
		private static readonly JsonSerializerSettings _jsonSerializerSettings = new()
		{
			Culture = CultureInfo.InvariantCulture,
			Formatting = Formatting.None,
			NullValueHandling = NullValueHandling.Ignore,
		};

		private readonly ClientOptions _clientOptions;
		private readonly HttpClient _httpClient;

		private bool _isDisposed = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="CloudflareClient"/> class.
		/// </summary>
		/// <param name="emailAddress">The email address of the Cloudflare account.</param>
		/// <param name="apiKey">The API key of the Cloudflare account.</param>
		/// <param name="clientOptions">The client options (optional).</param>
		public CloudflareClient(string emailAddress, string apiKey, ClientOptions clientOptions = null)
			: this(new ApiKeyAuthentication(emailAddress, apiKey), clientOptions)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CloudflareClient"/> class.
		/// </summary>
		/// <param name="apiToken">The API token.</param>
		/// <param name="clientOptions">The client options (optional).</param>
		public CloudflareClient(string apiToken, ClientOptions clientOptions = null)
			: this(new ApiTokenAuthentication(apiToken), clientOptions)
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CloudflareClient"/> class.
		/// </summary>
		/// <param name="authentication">The authentication information.</param>
		/// <param name="clientOptions">The client options (optional).</param>
		public CloudflareClient(IAuthentication authentication, ClientOptions clientOptions = null)
		{
			if (authentication == null)
				throw new ArgumentNullException(nameof(authentication));

			_clientOptions = clientOptions ?? new ClientOptions();
			ValidateClientOptions();

			_httpClient = CreateHttpClient();
			authentication.AddHeader(_httpClient);
		}

		/// <summary>
		/// Disposes of the resources used by the <see cref="CloudflareClient"/> object.
		/// </summary>
		public void Dispose()
		{
			if (_isDisposed)
				return;

			_isDisposed = true;

			_httpClient.Dispose();
			GC.SuppressFinalize(this);
		}

		/// <inheritdoc/>
		public async Task<CloudflareResponse<TResponse>> GetAsync<TResponse>(string requestPath, IQueryParameterFilter queryFilter = null, CancellationToken cancellationToken = default)
		{
			ThrowIfDisposed();
			ValidateRequestPath(requestPath);

			string requestUrl = BuildRequestUrl(requestPath, queryFilter);

			var response = await _httpClient.GetAsync(requestUrl, cancellationToken).ConfigureAwait(false);
			return await GetCloudflareResponse<TResponse>(response, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc/>
		public async Task<CloudflareResponse<TResponse>> PostAsync<TResponse, TRequest>(string requestPath, TRequest request, IQueryParameterFilter queryFilter = null, CancellationToken cancellationToken = default)
		{
			ThrowIfDisposed();
			ValidateRequestPath(requestPath);

			string requestUrl = BuildRequestUrl(requestPath, queryFilter);
			var httpContent = ConvertRequest(request);

			var response = await _httpClient.PostAsync(requestUrl, httpContent, cancellationToken).ConfigureAwait(false);
			return await GetCloudflareResponse<TResponse>(response, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc/>
		public async Task<CloudflareResponse<TResponse>> PutAsync<TResponse, TRequest>(string requestPath, TRequest request, CancellationToken cancellationToken = default)
		{
			ThrowIfDisposed();
			ValidateRequestPath(requestPath);

			string requestUrl = BuildRequestUrl(requestPath);
			var httpContent = ConvertRequest(request);

			var response = await _httpClient.PutAsync(requestUrl, httpContent, cancellationToken).ConfigureAwait(false);
			return await GetCloudflareResponse<TResponse>(response, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc/>
		public async Task<CloudflareResponse<TResponse>> DeleteAsync<TResponse>(string requestPath, IQueryParameterFilter queryFilter = null, CancellationToken cancellationToken = default)
		{
			ThrowIfDisposed();
			ValidateRequestPath(requestPath);

			string requestUrl = BuildRequestUrl(requestPath, queryFilter);

			var response = await _httpClient.DeleteAsync(requestUrl, cancellationToken).ConfigureAwait(false);
			return await GetCloudflareResponse<TResponse>(response, cancellationToken).ConfigureAwait(false);
		}

		/// <inheritdoc/>
		public async Task<CloudflareResponse<TResponse>> PatchAsync<TResponse, TRequest>(string requestPath, TRequest request, CancellationToken cancellationToken = default)
		{
			ThrowIfDisposed();
			ValidateRequestPath(requestPath);

			string requestUrl = BuildRequestUrl(requestPath);
			var httpContent = ConvertRequest(request);

			var response = await _httpClient.PatchAsync(requestUrl, httpContent, cancellationToken).ConfigureAwait(false);
			return await GetCloudflareResponse<TResponse>(response, cancellationToken).ConfigureAwait(false);
		}

		private void ThrowIfDisposed()
		{
			if (_isDisposed)
				throw new ObjectDisposedException(GetType().FullName);
		}

		private void ValidateClientOptions()
		{
			if (string.IsNullOrWhiteSpace(_clientOptions.BaseUrl))
				throw new ArgumentNullException(nameof(_clientOptions.BaseUrl));

			if (_clientOptions.Timeout <= TimeSpan.Zero)
				throw new ArgumentOutOfRangeException(nameof(_clientOptions.Timeout), "Timeout must be positive.");

			if (_clientOptions.MaxRetries < 0 || 10 < _clientOptions.MaxRetries)
				throw new ArgumentOutOfRangeException(nameof(_clientOptions.MaxRetries), "MaxRetries should be between 0 and 10.");

			if (_clientOptions.UseProxy && _clientOptions.Proxy == null)
				throw new ArgumentNullException(nameof(_clientOptions.Proxy));
		}

		private void ValidateRequestPath(string requestPath)
		{
			if (string.IsNullOrWhiteSpace(requestPath))
				throw new ArgumentNullException(nameof(requestPath));

			if (requestPath.Contains("?"))
				throw new ArgumentException("Query parameters are not allowed", nameof(requestPath));
		}

		private HttpClient CreateHttpClient()
		{
			string version = typeof(CloudflareClient).Assembly
				.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
				.InformationalVersion;

			HttpMessageHandler handler;
			try
			{
				handler = new HttpClientHandler
				{
					AllowAutoRedirect = _clientOptions.AllowRedirects,
					UseProxy = _clientOptions.UseProxy,
					Proxy = _clientOptions.Proxy,
				};
			}
			catch (PlatformNotSupportedException)
			{
				handler = new HttpClientHandler
				{
					AllowAutoRedirect = _clientOptions.AllowRedirects,
				};
			}

			var client = new HttpClient(handler, true)
			{
				BaseAddress = new Uri(_clientOptions.BaseUrl),
				Timeout = _clientOptions.Timeout,
			};

			client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AMWD.CloudflareClient", version));
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			if (_clientOptions.DefaultHeaders.Count > 0)
			{
				foreach (var headerKvp in _clientOptions.DefaultHeaders)
					client.DefaultRequestHeaders.Add(headerKvp.Key, headerKvp.Value);
			}

			return client;
		}

		private static async Task<CloudflareResponse<TRes>> GetCloudflareResponse<TRes>(HttpResponseMessage httpResponse, CancellationToken cancellationToken)
		{
#if NET6_0_OR_GREATER
			string content = await httpResponse.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
			string content = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif
			switch (httpResponse.StatusCode)
			{
				case HttpStatusCode.Forbidden:
				case HttpStatusCode.Unauthorized:
					var errorResponse = JsonConvert.DeserializeObject<CloudflareResponse<object>>(content, _jsonSerializerSettings)
						?? throw new CloudflareException("Response is not a valid Cloudflare API response.");

					throw new AuthenticationException(string.Join(Environment.NewLine, errorResponse.Errors.Select(e => $"{e.Code}: {e.Message}")));

				default:
					try
					{
						var response = JsonConvert.DeserializeObject<CloudflareResponse<TRes>>(content)
							?? throw new CloudflareException("Response is not a valid Cloudflare API response.");

						return response;
					}
					catch
					{
						if (typeof(TRes) == typeof(string))
						{
							object cObj = content.Replace("\\n", Environment.NewLine);
							return new CloudflareResponse<TRes>
							{
								Success = true,
								ResultInfo = new PaginationInfo(),
								Result = (TRes)cObj,
							};
						}

						throw;
					}
			}
		}

		private string BuildRequestUrl(string requestPath, IQueryParameterFilter queryFilter = null)
		{
			var dict = new Dictionary<string, string>();

			if (_clientOptions.DefaultQueryParams.Count > 0)
			{
				foreach (var paramKvp in _clientOptions.DefaultQueryParams)
					dict[paramKvp.Key] = paramKvp.Value;
			}

			var queryParams = queryFilter?.GetQueryParameters();
			if (queryParams?.Count > 0)
			{
				foreach (var kvp in queryParams)
					dict[kvp.Key] = kvp.Value;
			}

			if (dict.Count == 0)
				return requestPath;

			string[] param = dict.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}").ToArray();
			string query = string.Join("&", param);

			return $"{requestPath}?{query}";
		}

		private static HttpContent ConvertRequest<T>(T request)
		{
			if (request == null)
				return null;

			if (request is HttpContent httpContent)
				return httpContent;

			string json = JsonConvert.SerializeObject(request, _jsonSerializerSettings);
			return new StringContent(json, Encoding.UTF8, "application/json");
		}
	}
}
