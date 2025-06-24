using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using AMWD.Net.Api.Cloudflare.Zones.Internals;
using Moq;
using Newtonsoft.Json.Linq;

namespace Cloudflare.Zones.Tests.ZoneRegistrar
{
	[TestClass]
	public class UpdateDomainTest
	{
		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";

		private const string DomainName = "example.com";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<JToken> _response;

		private List<(string RequestPath, InternalUpdateDomainRequest Request)> _callbacks;

		private UpdateDomainRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<JToken>();

			_request = new UpdateDomainRequest(AccountId, DomainName)
			{
				AutoRenew = true,
				Privacy = false
			};
		}

		[TestMethod]
		public async Task ShouldUpdateRegistrarDomain()
		{
			// Arrange
			var client = GetClient();

			// Act
			var result = await client.UpdateDomain(_request);

			// Assert
			Assert.AreEqual(_response, result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/registrar/domains/{DomainName}", callback.RequestPath);

			Assert.IsNotNull(callback.Request);
			Assert.AreEqual(_request.AutoRenew, callback.Request.AutoRenew);
			Assert.AreEqual(_request.Locked, callback.Request.Locked);
			Assert.AreEqual(_request.Privacy, callback.Request.Privacy);

			_clientMock.Verify(m => m.PutAsync<JToken, InternalUpdateDomainRequest>($"/accounts/{AccountId}/registrar/domains/{DomainName}", It.IsAny<InternalUpdateDomainRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("  ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionOnDomainName(string domainName)
		{
			// Arrange
			_request.DomainName = domainName;
			var client = GetClient();

			// Act
			var result = await client.UpdateDomain(_request);

			// Assert - ArgumentNullException
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PutAsync<JToken, InternalUpdateDomainRequest>(It.IsAny<string>(), It.IsAny<InternalUpdateDomainRequest>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalUpdateDomainRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
