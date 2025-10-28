using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using AMWD.Net.Api.Cloudflare.Dns.Internals;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.TSIGs
{
	[TestClass]
	public class CreateTSIGTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<TSIG> _response;
		private List<(string RequestPath, InternalTSIGRequest Request)> _callbacks;
		private CreateTSIGRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<TSIG>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = new TSIG
				{
					Id = "tsig-1",
					Name = "tsig-key-a",
					Secret = "very-secret",
					Algorithm = TSigAlgorithm.HMAC_SHA256
				}
			};

			_request = new CreateTSIGRequest(
				accountId: AccountId,
				name: "tsig-key-a",
				secret: "very-secret"
			)
			{
				Algorithm = TSigAlgorithm.HMAC_SHA512
			};
		}

		[TestMethod]
		public async Task ShouldCreateTsig()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.CreateTSIG(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/secondary_dns/tsigs", requestPath);

			Assert.IsNotNull(request);
			Assert.AreEqual(_request.Name, request.Name);
			Assert.AreEqual(_request.Secret, request.Secret);
			Assert.AreEqual(_request.Algorithm, request.Algorithm);

			_clientMock.Verify(m => m.PostAsync<TSIG, InternalTSIGRequest>($"/accounts/{AccountId}/secondary_dns/tsigs", It.IsAny<InternalTSIGRequest>(), null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public async Task ShouldThrowArgumentNullExceptionForName(string name)
		{
			// Arrange
			_request.Name = name;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () =>
			{
				await client.CreateTSIG(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForAlgorithm()
		{
			// Arrange
			_request.Algorithm = 0;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.CreateTSIG(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public async Task ShouldThrowArgumentNullExceptionForSecret(string secret)
		{
			// Arrange
			_request.Secret = secret;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () =>
			{
				await client.CreateTSIG(_request, TestContext.CancellationToken);
			});
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<TSIG, InternalTSIGRequest>(It.IsAny<string>(), It.IsAny<InternalTSIGRequest>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalTSIGRequest, IQueryParameterFilter, CancellationToken>((requestPath, request, _, __) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
