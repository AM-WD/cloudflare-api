using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsDnssecExtensions
{
	[TestClass]
	public class DnssecDetailsTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<DNSSEC> _response;

		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

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
					ModifiedOn = DateTime.Parse("2025-08-02 10:20:30"),
					PublicKey = "ABCDEF1234567890",
					Status = DNSSECStatus.Active
				}
			};
		}

		[TestMethod]
		public async Task ShouldGetDnssecDetails()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.DnssecDetails(ZoneId);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dnssec", callback.RequestPath);
			Assert.IsNull(callback.QueryFilter);

			_clientMock.Verify(m => m.GetAsync<DNSSEC>(
				$"/zones/{ZoneId}/dnssec",
				null,
				It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<DNSSEC>(
					It.IsAny<string>(),
					It.IsAny<IQueryParameterFilter>(),
					It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
