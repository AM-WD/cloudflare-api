using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsRecordsExtensions
{
	[TestClass]
	public class ScanDnsRecordsTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<RecordScanResponse> _response;
		private List<string> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<RecordScanResponse>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = new RecordScanResponse
				{
					RecordsAdded = 5,
					TotalRecordsParsed = 6,
				}
			};
		}

		[TestMethod]
		public async Task ShouldScanDnsRecords()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ScanDnsRecords(ZoneId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/scan", _callbacks.First());

			_clientMock.Verify(m => m.PostAsync<RecordScanResponse, object>($"/zones/{ZoneId}/dns_records/scan", null, null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<RecordScanResponse, object>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, object, IQueryParameterFilter, CancellationToken>((requestPath, _, _, _) => _callbacks.Add(requestPath))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
