using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using Moq;

namespace Cloudflare.Zones.Tests.DnsRecords
{
	[TestClass]
	public class ScanDnsRecordsTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<ImportDnsRecordsResult> _response;
		private List<string> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<ImportDnsRecordsResult>
			{
				Success = true,
				Messages = [
					new ResponseInfo
					{
						Code = 1000,
						Message = "Message 1",
					}
				],
				Errors = [
					new ResponseInfo
					{
						Code = 1000,
						Message = "Error 1",
					}
				],
				Result = new ImportDnsRecordsResult
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
			var response = await client.ScanDnsRecords(ZoneId);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);
			Assert.AreEqual($"zones/{ZoneId}/dns_records/scan", _callbacks.First());

			_clientMock.Verify(m => m.PostAsync<ImportDnsRecordsResult, object>($"zones/{ZoneId}/dns_records/scan", null, null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<ImportDnsRecordsResult, object>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, object, IQueryParameterFilter, CancellationToken>((requestPath, _, _, _) => _callbacks.Add(requestPath))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
