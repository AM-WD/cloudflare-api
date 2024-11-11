using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using Moq;

namespace Cloudflare.Zones.Tests.DnsRecords
{
	[TestClass]
	public class ImportDnsRecordsTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string BindConfigContent = "www.example.com.		300	IN	A	127.0.0.1";

		private Mock<ICloudflareClient> _clientMock;

		private List<(string RequestPath, MultipartFormDataContent Request, IQueryParameterFilter QueryFilter)> _callbacks;
		private CloudflareResponse<ImportDnsRecordsResult> _response;
		private ImportDnsRecordsRequest _request;

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

			_request = new ImportDnsRecordsRequest(ZoneId, BindConfigContent);
		}

		[TestMethod]
		public async Task ShouldImportDnsRecordsFromString()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ImportDnsRecords(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"zones/{ZoneId}/dns_records/import", callback.RequestPath);
			Assert.IsNotNull(callback.Request);

			Assert.AreEqual(1, callback.Request.Count());

			var part = callback.Request.First();
			Assert.AreEqual("file", part.Headers.ContentDisposition.Name);
			Assert.IsInstanceOfType<ByteArrayContent>(part);
			Assert.AreEqual(BindConfigContent, await part.ReadAsStringAsync());

			_clientMock.Verify(m => m.PostAsync<ImportDnsRecordsResult, MultipartFormDataContent>($"zones/{ZoneId}/dns_records/import", It.IsAny<MultipartFormDataContent>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()), Times.Once);
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
				var response = await client.ImportDnsRecords(_request);

				// Assert
				Assert.IsNotNull(response);
				Assert.IsTrue(response.Success);
				Assert.AreEqual(_response.Result, response.Result);

				Assert.AreEqual(1, _callbacks.Count);

				var callback = _callbacks.First();
				Assert.AreEqual($"zones/{ZoneId}/dns_records/import", callback.RequestPath);
				Assert.IsNotNull(callback.Request);

				Assert.AreEqual(1, callback.Request.Count());

				var part = callback.Request.First();
				Assert.AreEqual("file", part.Headers.ContentDisposition.Name);
				Assert.IsInstanceOfType<ByteArrayContent>(part);
				Assert.AreEqual(BindConfigContent, await part.ReadAsStringAsync());

				_clientMock.Verify(m => m.PostAsync<ImportDnsRecordsResult, MultipartFormDataContent>($"zones/{ZoneId}/dns_records/import", It.IsAny<MultipartFormDataContent>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()), Times.Once);
				_clientMock.VerifyNoOtherCalls();
			}
			finally
			{
				File.Delete(file);
			}
		}

		[DataTestMethod]
		[DataRow(true)]
		[DataRow(false)]
		public async Task ShouldImportDnsRecordsFromStringWithProxied(bool proxied)
		{
			// Arrange
			_request.Proxied = proxied;
			var client = GetClient();

			// Act
			var response = await client.ImportDnsRecords(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"zones/{ZoneId}/dns_records/import", callback.RequestPath);
			Assert.IsNotNull(callback.Request);

			Assert.AreEqual(2, callback.Request.Count());

			var part = callback.Request.First();
			Assert.AreEqual("proxied", part.Headers.ContentDisposition.Name);
			Assert.IsInstanceOfType<StringContent>(part);
			Assert.AreEqual(proxied.ToString().ToLower(), await part.ReadAsStringAsync());

			part = callback.Request.Last();
			Assert.AreEqual("file", part.Headers.ContentDisposition.Name);
			Assert.IsInstanceOfType<ByteArrayContent>(part);
			Assert.AreEqual(BindConfigContent, await part.ReadAsStringAsync());

			_clientMock.Verify(m => m.PostAsync<ImportDnsRecordsResult, MultipartFormDataContent>($"zones/{ZoneId}/dns_records/import", It.IsAny<MultipartFormDataContent>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[DataTestMethod]
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
				var response = await client.ImportDnsRecords(_request);

				// Assert
				Assert.IsNotNull(response);
				Assert.IsTrue(response.Success);
				Assert.AreEqual(_response.Result, response.Result);

				Assert.AreEqual(1, _callbacks.Count);

				var callback = _callbacks.First();
				Assert.AreEqual($"zones/{ZoneId}/dns_records/import", callback.RequestPath);
				Assert.IsNotNull(callback.Request);

				Assert.AreEqual(2, callback.Request.Count());

				var part = callback.Request.First();
				Assert.AreEqual("proxied", part.Headers.ContentDisposition.Name);
				Assert.IsInstanceOfType<StringContent>(part);
				Assert.AreEqual(proxied.ToString().ToLower(), await part.ReadAsStringAsync());

				part = callback.Request.Last();
				Assert.AreEqual("file", part.Headers.ContentDisposition.Name);
				Assert.IsInstanceOfType<ByteArrayContent>(part);
				Assert.AreEqual(BindConfigContent, await part.ReadAsStringAsync());

				_clientMock.Verify(m => m.PostAsync<ImportDnsRecordsResult, MultipartFormDataContent>($"zones/{ZoneId}/dns_records/import", It.IsAny<MultipartFormDataContent>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()), Times.Once);
				_clientMock.VerifyNoOtherCalls();
			}
			finally
			{
				File.Delete(file);
			}
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForFile(string file)
		{
			// Arrange
			_request.File = file;
			var client = GetClient();

			// Act
			await client.ImportDnsRecords(_request);

			// Assert - ArgumentNullException
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<ImportDnsRecordsResult, MultipartFormDataContent>(It.IsAny<string>(), It.IsAny<MultipartFormDataContent>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, MultipartFormDataContent, IQueryParameterFilter, CancellationToken>((requestPath, request, queryFilter, _) => _callbacks.Add((requestPath, request, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
