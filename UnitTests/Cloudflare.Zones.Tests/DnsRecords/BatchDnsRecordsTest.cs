using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using AMWD.Net.Api.Cloudflare.Zones.Internals.Requests;
using Moq;

namespace Cloudflare.Zones.Tests.DnsRecords
{
	[TestClass]
	public class BatchDnsRecordsTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string RecordId = "023e105f4ecef8ad9ca31a8372d0c355";

		private Mock<ICloudflareClient> _clientMock;

		private List<(string RequestPath, InternalBatchRequest Request, IQueryParameterFilter QueryFilter)> _callbacks;
		private CloudflareResponse<BatchDnsRecordsResult> _response;
		private BatchDnsRecordsRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<BatchDnsRecordsResult>
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
				Result = new BatchDnsRecordsResult
				{
					DeletedRecords = [new DnsRecord { Id = RecordId, ZoneId = ZoneId, ZoneName = "example.com", Name = "example.com", Type = DnsRecordType.A, Content = "198.51.100.4" }],
					UpdatedRecords = [new DnsRecord { Id = RecordId, ZoneId = ZoneId, ZoneName = "example.com", Name = "example.com", Type = DnsRecordType.A, Content = "198.51.100.4" }],
					OverwrittenRecords = [new DnsRecord { Id = RecordId, ZoneId = ZoneId, ZoneName = "example.com", Name = "example.com", Type = DnsRecordType.A, Content = "198.51.100.4" }],
					CreatedRecords = [new DnsRecord { Id = RecordId, ZoneId = ZoneId, ZoneName = "example.com", Name = "example.com", Type = DnsRecordType.A, Content = "198.51.100.4" }],
				}
			};

			_request = new BatchDnsRecordsRequest(ZoneId)
			{
				DnsRecordIdsToDelete = [RecordId],
				DnsRecordsToUpdate = [new UpdateDnsRecordRequest(RecordId, ZoneId, "example.com", DnsRecordType.A) { Content = "198.51.100.4" }],
				DnsRecordsToOverwrite = [new OverwriteDnsRecordRequest(RecordId, ZoneId, "example.com", DnsRecordType.A) { Content = "198.51.100.4" }],
				DnsRecordsToCreate = [new CreateDnsRecordRequest(ZoneId, "example.com", DnsRecordType.A) { Content = "198.51.100.4" }],
			};
		}

		[TestMethod]
		public async Task ShouldExecuteBatchDnsRecords()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.BatchDnsRecords(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"zones/{ZoneId}/dns_records/batch", callback.RequestPath);
			Assert.IsNull(callback.QueryFilter);
			Assert.IsNotNull(callback.Request);

			Assert.AreEqual(1, callback.Request.Deletes.Count);
			Assert.AreEqual(RecordId, callback.Request.Deletes.First().Id);

			Assert.AreEqual(1, callback.Request.Patches.Count);
			var patch = callback.Request.Patches.First();
			Assert.AreEqual(RecordId, patch.Id);
			Assert.AreEqual("example.com", patch.Name);
			Assert.AreEqual(DnsRecordType.A, patch.Type);
			Assert.AreEqual("198.51.100.4", patch.Content);

			Assert.AreEqual(1, callback.Request.Puts.Count);
			var put = callback.Request.Puts.First();
			Assert.AreEqual(RecordId, put.Id);
			Assert.AreEqual("example.com", put.Name);
			Assert.AreEqual(DnsRecordType.A, put.Type);
			Assert.AreEqual("198.51.100.4", put.Content);

			Assert.AreEqual(1, callback.Request.Posts.Count);
			var post = callback.Request.Posts.First();
			Assert.AreEqual("example.com", post.Name);
			Assert.AreEqual(DnsRecordType.A, post.Type);
			Assert.AreEqual("198.51.100.4", post.Content);

			_clientMock.Verify(m => m.PostAsync<BatchDnsRecordsResult, InternalBatchRequest>(It.IsAny<string>(), It.IsAny<InternalBatchRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[DataRow("023e105f4ecef8ad9ca31a8372d0c354")]
		[ExpectedException(typeof(ArgumentException))]
		public async Task ShouldThrowArgumentExceptionForDifferentZoneIdOnUpdateRecord(string str)
		{
			// Arrange
			_request.DnsRecordsToUpdate.First().ZoneId = str;
			var client = GetClient();

			// Act
			await client.BatchDnsRecords(_request);

			// Assert - ArgumentException
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[DataRow("023e105f4ecef8ad9ca31a8372d0c354")]
		[ExpectedException(typeof(ArgumentException))]
		public async Task ShouldThrowArgumentExceptionForDifferentZoneIdOnOverwriteRecord(string str)
		{
			// Arrange
			_request.DnsRecordsToOverwrite.First().ZoneId = str;
			var client = GetClient();

			// Act
			await client.BatchDnsRecords(_request);

			// Assert - ArgumentException
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<BatchDnsRecordsResult, InternalBatchRequest>(It.IsAny<string>(), It.IsAny<InternalBatchRequest>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalBatchRequest, IQueryParameterFilter, CancellationToken>((requestPath, request, queryFilter, _) => _callbacks.Add((requestPath, request, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
