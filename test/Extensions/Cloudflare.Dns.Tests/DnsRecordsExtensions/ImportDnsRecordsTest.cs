using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsRecordsExtensions
{
	[TestClass]
	public class ImportDnsRecordsTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string BindConfigContent = "www.example.com.		300	IN	A	127.0.0.1";

		private Mock<ICloudflareClient> _clientMock;

		private List<(string RequestPath, MultipartFormDataContent Request, IQueryParameterFilter QueryFilter)> _callbacks;
		private CloudflareResponse<RecordImportResponse> _response;
		private ImportDnsRecordsRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<RecordImportResponse>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = new RecordImportResponse
				{
					RecordsAdded = 5,
					TotalRecordsParsed = 6,
				}
			};

			_request = new ImportDnsRecordsRequest(ZoneId)
			{
				File = BindConfigContent
			};
		}

		[TestMethod]
		public async Task ShouldImportDnsRecordsFromString()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ImportDnsRecords(_request, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/import", requestPath);
			Assert.IsNotNull(request);

			Assert.AreEqual(1, request.Count());

			var part = request.First();
			Assert.AreEqual("file", part.Headers.ContentDisposition.Name);
			Assert.IsInstanceOfType<ByteArrayContent>(part);
			Assert.AreEqual(BindConfigContent, await part.ReadAsStringAsync(TestContext.CancellationTokenSource.Token));

			_clientMock.Verify(m => m.PostAsync<RecordImportResponse, MultipartFormDataContent>($"/zones/{ZoneId}/dns_records/import", It.IsAny<MultipartFormDataContent>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldImportDnsRecordsFromFile()
		{
			// Arrange
			string file = Path.GetTempFileName();
			try
			{
				File.WriteAllText(file, BindConfigContent);
				_request.File = file;
				var client = GetClient();

				// Act
				var response = await client.ImportDnsRecords(_request, TestContext.CancellationTokenSource.Token);

				// Assert
				Assert.IsNotNull(response);
				Assert.IsTrue(response.Success);
				Assert.AreEqual(_response.Result, response.Result);

				Assert.HasCount(1, _callbacks);

				var (requestPath, request, queryFilter) = _callbacks.First();
				Assert.AreEqual($"/zones/{ZoneId}/dns_records/import", requestPath);
				Assert.IsNotNull(request);

				Assert.AreEqual(1, request.Count());

				var part = request.First();
				Assert.AreEqual("file", part.Headers.ContentDisposition.Name);
				Assert.IsInstanceOfType<ByteArrayContent>(part);
				Assert.AreEqual(BindConfigContent, await part.ReadAsStringAsync(TestContext.CancellationTokenSource.Token));

				_clientMock.Verify(m => m.PostAsync<RecordImportResponse, MultipartFormDataContent>($"/zones/{ZoneId}/dns_records/import", It.IsAny<MultipartFormDataContent>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()), Times.Once);
				_clientMock.VerifyNoOtherCalls();
			}
			finally
			{
				File.Delete(file);
			}
		}

		[TestMethod]
		[DataRow(true)]
		[DataRow(false)]
		public async Task ShouldImportDnsRecordsFromStringWithProxied(bool proxied)
		{
			// Arrange
			_request.Proxied = proxied;
			var client = GetClient();

			// Act
			var response = await client.ImportDnsRecords(_request, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records/import", requestPath);
			Assert.IsNotNull(request);

			Assert.AreEqual(2, request.Count());

			var part = request.First();
			Assert.AreEqual("proxied", part.Headers.ContentDisposition.Name);
			Assert.IsInstanceOfType<StringContent>(part);
			Assert.AreEqual(proxied.ToString().ToLower(), await part.ReadAsStringAsync(TestContext.CancellationTokenSource.Token));

			part = request.Last();
			Assert.AreEqual("file", part.Headers.ContentDisposition.Name);
			Assert.IsInstanceOfType<ByteArrayContent>(part);
			Assert.AreEqual(BindConfigContent, await part.ReadAsStringAsync(TestContext.CancellationTokenSource.Token));

			_clientMock.Verify(m => m.PostAsync<RecordImportResponse, MultipartFormDataContent>($"/zones/{ZoneId}/dns_records/import", It.IsAny<MultipartFormDataContent>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		[DataRow(true)]
		[DataRow(false)]
		public async Task ShouldImportDnsRecordsFromFileWithProxied(bool proxied)
		{
			// Arrange
			string file = Path.GetTempFileName();
			try
			{
				File.WriteAllText(file, BindConfigContent);
				_request.File = file;
				_request.Proxied = proxied;
				var client = GetClient();

				// Act
				var response = await client.ImportDnsRecords(_request, TestContext.CancellationTokenSource.Token);

				// Assert
				Assert.IsNotNull(response);
				Assert.IsTrue(response.Success);
				Assert.AreEqual(_response.Result, response.Result);

				Assert.HasCount(1, _callbacks);

				var (requestPath, request, queryFilter) = _callbacks.First();
				Assert.AreEqual($"/zones/{ZoneId}/dns_records/import", requestPath);
				Assert.IsNotNull(request);

				Assert.AreEqual(2, request.Count());

				var part = request.First();
				Assert.AreEqual("proxied", part.Headers.ContentDisposition.Name);
				Assert.IsInstanceOfType<StringContent>(part);
				Assert.AreEqual(proxied.ToString().ToLower(), await part.ReadAsStringAsync(TestContext.CancellationTokenSource.Token));

				part = request.Last();
				Assert.AreEqual("file", part.Headers.ContentDisposition.Name);
				Assert.IsInstanceOfType<ByteArrayContent>(part);
				Assert.AreEqual(BindConfigContent, await part.ReadAsStringAsync(TestContext.CancellationTokenSource.Token));

				_clientMock.Verify(m => m.PostAsync<RecordImportResponse, MultipartFormDataContent>($"/zones/{ZoneId}/dns_records/import", It.IsAny<MultipartFormDataContent>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()), Times.Once);
				_clientMock.VerifyNoOtherCalls();
			}
			finally
			{
				File.Delete(file);
			}
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public async Task ShouldThrowArgumentNullExceptionForFile(string file)
		{
			// Arrange
			_request.File = file;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () => await client.ImportDnsRecords(_request, TestContext.CancellationTokenSource.Token));
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<RecordImportResponse, MultipartFormDataContent>(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, MultipartFormDataContent, IQueryParameterFilter, CancellationToken>((requestPath, request, queryFilter, _) => _callbacks.Add((requestPath, request, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
