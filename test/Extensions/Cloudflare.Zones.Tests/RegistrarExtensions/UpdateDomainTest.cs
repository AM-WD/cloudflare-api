using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using AMWD.Net.Api.Cloudflare.Zones.Internals;
using Moq;
using Newtonsoft.Json.Linq;

namespace Cloudflare.Zones.Tests.RegistrarExtensions
{
	[TestClass]
	public class UpdateDomainTest
	{
		public TestContext TestContext { get; set; }

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
			var result = await client.UpdateDomain(_request, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.AreEqual(_response, result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/registrar/domains/{DomainName}", requestPath);

			Assert.IsNotNull(request);
			Assert.AreEqual(_request.AutoRenew, request.AutoRenew);
			Assert.AreEqual(_request.Locked, request.Locked);
			Assert.AreEqual(_request.Privacy, request.Privacy);

			_clientMock.Verify(m => m.PutAsync<JToken, InternalUpdateDomainRequest>($"/accounts/{AccountId}/registrar/domains/{DomainName}", It.IsAny<InternalUpdateDomainRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("  ")]
		public async Task ShouldThrowArgumentNullExceptionOnDomainName(string domainName)
		{
			// Arrange
			_request.DomainName = domainName;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => await client.UpdateDomain(_request, TestContext.CancellationTokenSource.Token));
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
