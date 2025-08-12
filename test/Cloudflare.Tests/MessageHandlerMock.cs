using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace Cloudflare.Tests
{
	internal class HttpMessageHandlerMock
	{
		public HttpMessageHandlerMock()
		{
			Mock = new();
			Mock.Protected()
				.Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
				.Callback<HttpRequestMessage, CancellationToken>(async (request, ct) =>
				{
					var callback = new HttpMessageRequestCallback
					{
						Method = request.Method,
						Url = request.RequestUri.ToString(),
						Headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.First()),
					};

					if (request.Content != null)
						callback.Content = await request.Content.ReadAsStringAsync(ct);

					Callbacks.Add(callback);
				})
				.ReturnsAsync(Responses.Dequeue);
		}

		public List<HttpMessageRequestCallback> Callbacks { get; } = [];

		public Queue<HttpResponseMessage> Responses { get; } = new();

		public Mock<HttpClientHandler> Mock { get; }
	}

	internal class HttpMessageRequestCallback
	{
		public HttpMethod Method { get; set; }

		public string Url { get; set; }

		public Dictionary<string, string> Headers { get; set; }

		public string Content { get; set; }
	}
}
