using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.TSIGs
{
	[TestClass]
	public class ListTSIGsTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<IReadOnlyCollection<TSIG>> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<IReadOnlyCollection<TSIG>>
			{
				Success = true,
				Messages = [new ResponseInfo(1000, "Message 1")],
				Errors = [],
				Result =
				[
					new TSIG
					{
						Id = "tsig-1",
						Name = "tsig-key-a",
						Secret = "secretA",
						Algorithm = TSigAlgorithm.HMAC_SHA256
					},
					new TSIG
					{
						Id = "tsig-2",
						Name = "tsig-key-b",
						Secret = "secretB",
						Algorithm = TSigAlgorithm.HMAC_SHA1
					}
				]
			};
		}

		[TestMethod]
		public async Task ShouldListTSIGs()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ListTSIGs(AccountId, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);
			Assert.HasCount(2, response.Result);

			var tsigs = response.Result.ToList();
			Assert.AreEqual("tsig-key-a", tsigs[0].Name);
			Assert.AreEqual("secretA", tsigs[0].Secret);
			Assert.AreEqual(TSigAlgorithm.HMAC_SHA256, tsigs[0].Algorithm);

			Assert.AreEqual("tsig-key-b", tsigs[1].Name);
			Assert.AreEqual("secretB", tsigs[1].Secret);
			Assert.AreEqual(TSigAlgorithm.HMAC_SHA1, tsigs[1].Algorithm);

			Assert.HasCount(1, _callbacks);
			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/secondary_dns/tsigs", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<IReadOnlyCollection<TSIG>>($"/accounts/{AccountId}/secondary_dns/tsigs", null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<IReadOnlyCollection<TSIG>>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
