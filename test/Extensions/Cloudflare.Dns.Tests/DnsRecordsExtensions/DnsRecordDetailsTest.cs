using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;
using Newtonsoft.Json.Linq;

namespace Cloudflare.Dns.Tests.DnsRecordsExtensions
{
	[TestClass]
	public class DnsRecordDetailsTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string RecordId = "023e105f4ecef8ad9ca31a8372d0c355";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<JObject> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

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
				]
			};
		}

		[TestMethod]
		public async Task ShouldGetARecord()
		{
			// Arrange
			var result = new ARecord("example.com")
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
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetAAAARecord()
		{
			// Arrange
			var result = new AAAARecord("example.com")
			{
				Id = RecordId,
				Content = "::1",
				Proxiable = true,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Comment = "Domain verification record",
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetCAARecord()
		{
			// Arrange
			var result = new CAARecord("example.com")
			{
				Id = RecordId,
				Content = "0 issue \"letsencrypt.org\"",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetCERTRecord()
		{
			// Arrange
			var result = new CERTRecord("example.com")
			{
				Id = RecordId,
				Content = "2 77 2 TUlJQ1l6Q0NBY3lnQXdJQkFnSUJBREFOQmdrcWh",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 120,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetCNAMERecord()
		{
			// Arrange
			var result = new CNAMERecord("example.com")
			{
				Id = RecordId,
				Content = "alias.example.org",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetDNSKEYRecord()
		{
			// Arrange
			var result = new DNSKEYRecord("example.com")
			{
				Id = RecordId,
				Content = "256 3 13 OtuN/SL9sE+SDQ0tOLeezr1KzUNi77FflTjxQylUhm3V7m13Vz9tYQuc SGK0pyxISo9CQsszubAwJSypq3li3g==",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetDSRecord()
		{
			// Arrange
			var result = new DSRecord("example.com")
			{
				Id = RecordId,
				Content = "370 13 2 BE74359954660069D5C63D200C39F5603827D7DD02B56F120EE9F3A8 6764247C",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetHTTPSRecord()
		{
			// Arrange
			var result = new HTTPSRecord("example.com")
			{
				Id = RecordId,
				Content = "1 svc.example.com. alpn=h2",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetLOCRecord()
		{
			// Arrange
			var result = new LOCRecord("example.com")
			{
				Id = RecordId,
				Content = "51 30 12.748 N 0 7 39.611 W 0.00m 0.00m 0.00m 0.00m",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetMXRecord()
		{
			// Arrange
			var result = new MXRecord("example.com")
			{
				Id = RecordId,
				Content = "10 mail.example.com.",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetNAPTRRecord()
		{
			// Arrange
			var result = new NAPTRRecord("example.com")
			{
				Id = RecordId,
				Content = "100 10 \"S\" \"SIP+D2T\" \"\" _sip._tcp.example.com.",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetNSRecord()
		{
			// Arrange
			var result = new NSRecord("example.com")
			{
				Id = RecordId,
				Content = "a.iana-servers.net.",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetOPENPGPKEYRecord()
		{
			// Arrange
			var result = new OPENPGPKEYRecord("00d8d3f11739d2f3537099982b4674c29fc59a8fda350fca1379613a._openpgpkey.example.com")
			{
				Id = RecordId,
				Content = "a2V5S0VZMTIzNGtleUtFWQ==",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 3600,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetPTRRecord()
		{
			// Arrange
			var result = new PTRRecord("14.215.184.93.in-addr.arpa")
			{
				Id = RecordId,
				Content = "example.com",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetSMIMEARecord()
		{
			// Arrange
			var result = new SMIMEARecord("example.com")
			{
				Id = RecordId,
				Content = "0 0 0 keyKEY1234keyKEY",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetSRVRecord()
		{
			// Arrange
			var result = new SRVRecord("example.com")
			{
				Id = RecordId,
				Content = "1 0 443 mail.example.com.",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetSSHFPRecord()
		{
			// Arrange
			var result = new SSHFPRecord("example.com")
			{
				Id = RecordId,
				Content = "2 1 123456789abcdef67890123456789abcdef67890",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetSVCBRecord()
		{
			// Arrange
			var result = new SVCBRecord("example.com")
			{
				Id = RecordId,
				Content = "1 . alpn=\"h2,http/1.1\"",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetTLSARecord()
		{
			// Arrange
			var result = new TLSARecord("_443._tcp.example.com")
			{
				Id = RecordId,
				Content = "3 0 18cb0fc6c527506a053f4f14c8464bebbd6dede2738d11468dd953d7d6a3021f1",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetTXTRecord()
		{
			// Arrange
			var result = new TXTRecord("example.com")
			{
				Id = RecordId,
				Content = "\"v=spf1 -all\"",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetURIRecord()
		{
			// Arrange
			var result = new URIRecord("_ftp._tcp.example.com")
			{
				Id = RecordId,
				Content = "10 1 \"ftp://ftp.example.com/public\"",
				Proxiable = false,
				Proxied = false,
				TimeToLive = 1,
				Settings = new(),
				Tags = [],
				CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
				CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
				TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
			};

			_response.Result = JObject.FromObject(result);
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			AssertRecord(result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldReturnNullForNullResponse()
		{
			// Arrange
			_response.Result = null;
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNull(response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldReturnNullForUnknownType()
		{
			// Arrange
			var result = new ARecord("example.com");
			_response.Result = JObject.FromObject(result);
			_response.Result.Remove("type");
			var client = GetClient();

			// Act
			var response = await client.DnsRecordDetails(ZoneId, RecordId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNull(response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/{RecordId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<JObject>($"/zones/{ZoneId}/dns_records/{RecordId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private static void AssertRecord(DnsRecord expected, DnsRecord actual)
		{
			Assert.AreEqual(expected.Id, actual.Id);
			Assert.AreEqual(expected.Name, actual.Name);
			Assert.AreEqual(expected.Type, actual.Type);
			Assert.AreEqual(expected.Content, actual.Content);
			Assert.AreEqual(expected.Proxied, actual.Proxied);
			Assert.AreEqual(expected.TimeToLive, actual.TimeToLive);
			Assert.AreEqual(expected.Comment, actual.Comment);
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<JObject>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
