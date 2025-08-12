using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using Moq;

namespace Cloudflare.Zones.Tests.RegistrarExtensions
{
	[TestClass]
	public class ListDomainsTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<IReadOnlyCollection<Domain>> _response;

		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<IReadOnlyCollection<Domain>>();
		}

		[TestMethod]
		public async Task ShouldListRegistrarDomains()
		{
			// Arrange
			var client = GetClient();

			// Act
			var result = await client.ListDomains(AccountId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.AreEqual(_response, result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/registrar/domains", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<IReadOnlyCollection<Domain>>($"/accounts/{AccountId}/registrar/domains", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<IReadOnlyCollection<Domain>>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
