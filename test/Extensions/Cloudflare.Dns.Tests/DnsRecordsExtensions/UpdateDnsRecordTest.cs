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
	public class UpdateDnsRecordTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string RecordId = "023e105f4ecef8ad9ca31a8372d0c355";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<JObject> _response;
		private List<(string RequestPath, InternalDnsRecordRequest Request)> _callbacks;
		private UpdateDnsRecordRequest _request;

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
				Result = JObject.FromObject(new ARecord("example.com")
				{
					Id = RecordId,
					Name = "*.example.com",
					Content = "96.7.128.175",
					Proxiable = true,
					Proxied = true,
					TimeToLive = 1,
					Settings = new(),
					Meta = new JObject(),
					Comment = "Domain verification record",
					Tags = [],
					CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
					ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
					CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
					TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
				})
			};

			_request = new UpdateDnsRecordRequest(ZoneId, RecordId, "example.com")
			{
				Type = DnsRecordType.A,
				Content = "127.0.1.22"
			};
		}

		[TestMethod]
		public async Task ShouldUpdateDnsRecord()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.UpdateDnsRecord(_request, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			Assert.IsNotNull(response.Result);
			Assert.IsInstanceOfType<ARecord>(response.Result);

			Assert.AreEqual(RecordId, response.Result.Id);
			Assert.AreEqual("*.example.com", response.Result.Name);
			Assert.AreEqual("96.7.128.175", response.Result.Content);
			Assert.AreEqual("Domain verification record", response.Result.Comment);
			Assert.AreEqual(1, response.Result.TimeToLive);
			Assert.IsTrue(response.Result.Proxied);
			Assert.IsTrue(response.Result.Proxiable);
			Assert.IsNotNull(response.Result.Settings);
			Assert.IsNotNull(response.Result.Meta);
			Assert.IsNotNull(response.Result.CommentModifiedOn);
			Assert.IsNotNull(response.Result.TagsModifiedOn);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNotNull(request);

			Assert.AreEqual("example.com", request.Name);
			Assert.AreEqual(DnsRecordType.A, request.Type);
			Assert.AreEqual("127.0.1.22", request.Content);

			_clientMock.Verify(m => m.PatchAsync<JObject, InternalDnsRecordRequest>($"/zones/{ZoneId}/dns_records/{RecordId}", It.IsAny<InternalDnsRecordRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PatchAsync<JObject, InternalDnsRecordRequest>(It.IsAny<string>(), It.IsAny<InternalDnsRecordRequest>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalDnsRecordRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
