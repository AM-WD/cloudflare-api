using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using AMWD.Net.Api.Cloudflare.Dns.Internals;
using Moq;
using Newtonsoft.Json.Linq;

namespace Cloudflare.Dns.Tests.DnsRecordsExtensions
{
	[TestClass]
	public class BatchDnsRecordsTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string RecordId = "023e105f4ecef8ad9ca31a8372d0c355";

		private const string DomainName = "example.com";
		private const string IpContent = "96.7.128.175";

		private Mock<ICloudflareClient> _clientMock;

		private List<(string RequestPath, InternalBatchRequest Request, IQueryParameterFilter QueryFilter)> _callbacks;
		private CloudflareResponse<JObject> _response;
		private BatchDnsRecordsRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<JObject>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = new JObject
				{
					["deletes"] = JArray.FromObject(new[] { JObject.FromObject(new ARecord(DomainName) { Id = RecordId, Content = IpContent }) }),
					["patches"] = JArray.FromObject(new[] { JObject.FromObject(new ARecord(DomainName) { Id = RecordId, Content = IpContent }) }),
					["posts"] = JArray.FromObject(new[] { JObject.FromObject(new ARecord(DomainName) { Id = RecordId, Content = IpContent }) }),
					["puts"] = JArray.FromObject(new[] { JObject.FromObject(new ARecord(DomainName) { Id = RecordId, Content = IpContent }) })
				}
			};

			_request = new BatchDnsRecordsRequest(ZoneId)
			{
				Deletes = [RecordId],
				Creates = [new BatchDnsRecordsRequest.Post(DomainName) { Type = DnsRecordType.A, Content = IpContent }],
				Updates = [new BatchDnsRecordsRequest.Patch(RecordId, DomainName) { Type = DnsRecordType.A, Content = IpContent }],
				Overwrites = [new BatchDnsRecordsRequest.Put(RecordId, DomainName) { Type = DnsRecordType.A, Content = IpContent }]
			};
		}

		[TestMethod]
		public async Task ShouldExecuteBatchDnsRecords()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.BatchDnsRecords(_request, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);

			Assert.AreEqual(RecordId, response.Result.Deletes.Single().Id);
			Assert.AreEqual(IpContent, response.Result.Deletes.Single().Content);
			Assert.AreEqual(RecordId, response.Result.Creates.Single().Id);
			Assert.AreEqual(IpContent, response.Result.Creates.Single().Content);
			Assert.AreEqual(RecordId, response.Result.Updates.Single().Id);
			Assert.AreEqual(IpContent, response.Result.Updates.Single().Content);
			Assert.AreEqual(RecordId, response.Result.Overwrites.Single().Id);
			Assert.AreEqual(IpContent, response.Result.Overwrites.Single().Content);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/batch", requestPath);
			Assert.IsNull(queryFilter);
			Assert.IsNotNull(request);

			Assert.HasCount(1, request.Deletes);
			Assert.AreEqual(RecordId, request.Deletes.First().Id);

			Assert.HasCount(1, request.Patches);
			var patch = request.Patches.First();
			Assert.AreEqual(RecordId, patch.Id);
			Assert.AreEqual(DomainName, patch.Name);
			Assert.AreEqual(DnsRecordType.A, patch.Type);
			Assert.AreEqual(IpContent, patch.Content);

			Assert.HasCount(1, request.Puts);
			var put = request.Puts.First();
			Assert.AreEqual(RecordId, put.Id);
			Assert.AreEqual(DomainName, put.Name);
			Assert.AreEqual(DnsRecordType.A, put.Type);
			Assert.AreEqual(IpContent, put.Content);

			Assert.HasCount(1, request.Posts);
			var post = request.Posts.First();
			Assert.AreEqual(DomainName, post.Name);
			Assert.AreEqual(DnsRecordType.A, post.Type);
			Assert.AreEqual(IpContent, post.Content);

			_clientMock.Verify(m => m.PostAsync<JObject, InternalBatchRequest>($"/zones/{ZoneId}/dns_records/batch", It.IsAny<InternalBatchRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<JObject, InternalBatchRequest>(It.IsAny<string>(), It.IsAny<InternalBatchRequest>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalBatchRequest, IQueryParameterFilter, CancellationToken>((requestPath, request, queryFilter, _) => _callbacks.Add((requestPath, request, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
