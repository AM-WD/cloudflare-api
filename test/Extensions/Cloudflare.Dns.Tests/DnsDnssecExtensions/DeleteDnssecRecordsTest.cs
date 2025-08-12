using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsDnssecExtensions
{
	[TestClass]
	public class DeleteDnssecRecordsTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<string> _response;

		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<string>
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
				Result = "023e105f4ecef8ad9ca31a8372d0c353"
			};
		}

		[TestMethod]
		public async Task ShouldDeleteDnssecRecords()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.DeleteDnssecRecords(ZoneId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dnssec", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.DeleteAsync<string>(
				$"/zones/{ZoneId}/dnssec",
				null,
				It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.DeleteAsync<string>(
					It.IsAny<string>(),
					It.IsAny<IQueryParameterFilter>(),
					It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
