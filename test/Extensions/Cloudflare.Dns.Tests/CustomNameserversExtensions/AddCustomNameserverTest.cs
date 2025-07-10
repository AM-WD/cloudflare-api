using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using AMWD.Net.Api.Cloudflare.Dns.Internals;
using Moq;

namespace Cloudflare.Dns.Tests.CustomNameserversExtensions
{
	[TestClass]
	public class AddCustomNameserverTest
	{
		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";

		private const string Nameserver = "ns1.example.com";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<CustomNameserver> _response;

		private List<(string RequestPath, InternalAddCustomNameserverRequest Request)> _callbacks;

		private AddCustomNameserverRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<CustomNameserver>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = new CustomNameserver(Nameserver)
			};

			_request = new AddCustomNameserverRequest(AccountId, Nameserver);
		}

		[TestMethod]
		public async Task ShouldAddCustomNameserver()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.AddCustomNameserver(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/custom_ns", callback.RequestPath);
			Assert.IsNotNull(callback.Request);

			Assert.AreEqual(_request.NameserverName, callback.Request.NameserverName);
			Assert.IsNull(callback.Request.NameserverSet);

			_clientMock.Verify(m => m.PostAsync<CustomNameserver, InternalAddCustomNameserverRequest>($"/accounts/{AccountId}/custom_ns", It.IsAny<InternalAddCustomNameserverRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<CustomNameserver, InternalAddCustomNameserverRequest>(It.IsAny<string>(), It.IsAny<InternalAddCustomNameserverRequest>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalAddCustomNameserverRequest, IQueryParameterFilter, CancellationToken>((requestPath, request, _, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
