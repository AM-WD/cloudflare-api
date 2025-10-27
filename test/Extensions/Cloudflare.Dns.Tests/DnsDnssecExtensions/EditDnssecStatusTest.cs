using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using AMWD.Net.Api.Cloudflare.Dns.Internals;
using Moq;

namespace Cloudflare.Dns.Tests.DnsDnssecExtensions
{
	[TestClass]
	public class EditDnssecStatusTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<DNSSEC> _response;

		private List<(string RequestPath, InternalEditDnssecStatusRequest Request)> _callbacks;

		private EditDnssecStatusRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<DNSSEC>
			{
				Success = true,
				Messages =
				[
					new ResponseInfo(1000, "Message 1")
				],
				Errors =
				[
					new ResponseInfo(1000, "Error 1")
				],
				Result = new DNSSEC
				{
					Algorithm = "ECDSAP256SHA256",
					Digest = "1234567890ABCDEF",
					DigestAlgorithm = "SHA256",
					DigestType = "2",
					DnssecMultiSigner = true,
					DnssecPresigned = false,
					DnssecUseNsec3 = true,
					Ds = "12345 13 2 1234567890ABCDEF",
					Flags = 257,
					KeyTag = 12345,
					KeyType = "ECDSAP256SHA256",
					ModifiedOn = DateTime.UtcNow,
					PublicKey = "ABCDEF1234567890",
					Status = DNSSECStatus.Active
				}
			};

			_request = new EditDnssecStatusRequest(ZoneId)
			{
				DnssecMultiSigner = true,
				DnssecPresigned = false,
				DnssecUseNsec3 = true,
				Status = DnssecEditStatus.Active
			};
		}

		[TestMethod]
		public async Task ShouldEditDnssecStatus()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.EditDnssecStatus(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dnssec", requestPath);

			Assert.IsNotNull(request);
			Assert.AreEqual(_request.DnssecMultiSigner, request.DnssecMultiSigner);
			Assert.AreEqual(_request.DnssecPresigned, request.DnssecPresigned);
			Assert.AreEqual(_request.DnssecUseNsec3, request.DnssecUseNsec3);
			Assert.AreEqual(_request.Status, request.Status);

			_clientMock.Verify(m => m.PatchAsync<DNSSEC, InternalEditDnssecStatusRequest>(
				$"/zones/{ZoneId}/dnssec",
				It.IsAny<InternalEditDnssecStatusRequest>(),
				It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PatchAsync<DNSSEC, InternalEditDnssecStatusRequest>(
					It.IsAny<string>(),
					It.IsAny<InternalEditDnssecStatusRequest>(),
					It.IsAny<CancellationToken>()))
				.Callback<string, InternalEditDnssecStatusRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
