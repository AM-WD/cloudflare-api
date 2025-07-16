using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsRecordsExtensions
{
	[TestClass]
	public class DnsRecordDetailsTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string RecordId = "023e105f4ecef8ad9ca31a8372d0c355";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<DnsRecord> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<DnsRecord>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = new ARecord("example.com")
				{
					Id = RecordId,
					Content = "96.7.128.175",
					Proxiable = true,
					Proxied = true,
					TimeToLive = 1,
					Settings = new(),
					Comment = "Domain verification record",
					Tags = [],
					CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
					ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
					CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
					TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
				}
			};
		}

		[TestMethod]
		public async Task ShouldDeleteDnsRecord()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", callback.RequestPath);
			Assert.IsNull(callback.QueryFilter);

			_clientMock.Verify(m => m.GetAsync<DnsRecord>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<DnsRecord>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
