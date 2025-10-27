using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using Moq;
using Newtonsoft.Json.Linq;

namespace Cloudflare.Zones.Tests.RegistrarExtensions
{
	[TestClass]
	public class GetDomainTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string DomainName = "example.com";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<JToken> _response;

		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<JToken>();
		}

		[TestMethod]
		public async Task ShouldGetRegistrarDomain()
		{
			// Arrange
			var client = GetClient();

			// Act
			var result = await client.GetDomain(AccountId, DomainName, TestContext.CancellationToken);

			// Assert
			Assert.AreEqual(_response, result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/registrar/domains/{DomainName}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JToken>($"/accounts/{AccountId}/registrar/domains/{DomainName}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("  ")]
		public async Task ShouldThrowArgumentNullExceptionOnDomainName(string domainName)
		{
			// Arrange
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => await client.GetDomain(AccountId, domainName, TestContext.CancellationToken));
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<JToken>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
