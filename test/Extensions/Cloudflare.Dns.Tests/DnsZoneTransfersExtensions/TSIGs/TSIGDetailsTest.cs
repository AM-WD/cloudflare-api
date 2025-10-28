using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.TSIGs
{
	[TestClass]
	public class TSIGDetailsTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string TsigId = "023e105f4ecef8ad9ca31a8372d0c351";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<TSIG> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = new List<(string, IQueryParameterFilter)>();

			_response = new CloudflareResponse<TSIG>
			{
				Success = true,
				Messages = [new ResponseInfo(1000, "Message 1")],
				Errors = [new ResponseInfo(1000, "Error 1")],
				Result = new TSIG
				{
					Id = TsigId,
					Name = "tsig-key-a",
					Secret = "very-secret",
					Algorithm = TSigAlgorithm.HMAC_SHA256
				}
			};
		}

		[TestMethod]
		public async Task ShouldGetTSIGDetails()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.TSIGDetails(AccountId, TsigId, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/secondary_dns/tsigs/{TsigId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<TSIG>($"/accounts/{AccountId}/secondary_dns/tsigs/{TsigId}", null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<TSIG>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
