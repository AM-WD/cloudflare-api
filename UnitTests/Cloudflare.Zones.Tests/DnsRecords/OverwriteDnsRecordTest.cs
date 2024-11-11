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
	public class OverwriteDnsRecordTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string RecordId = "023e105f4ecef8ad9ca31a8372d0c355";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<DnsRecord> _response;
		private List<(string RequestPath, InternalDnsRecordRequest Request)> _callbacks;
		private OverwriteDnsRecordRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<DnsRecord>
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
				Result = new DnsRecord
				{
					Id = RecordId,
					ZoneId = ZoneId,
					ZoneName = "example.com",
					Name = "*.example.com",
					Type = DnsRecordType.A,
					Content = "198.51.100.4",
					Proxiable = true,
					Proxied = true,
					Ttl = 1,
					Settings = new(),
					Meta = new DnsRecordMeta
					{
						AutoAdded = false,
						ManagedByApps = false,
						ManagedByArgoTunnel = false
					},
					Comment = "Domain verification record",
					Tags = [],
					CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
					ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
					CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
					TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
				}
			};

			_request = new OverwriteDnsRecordRequest(RecordId, ZoneId, "example.com", DnsRecordType.A)
			{
				Content = "127.0.1.22"
			};
		}

		[TestMethod]
		public async Task ShouldOverwriteDnsRecord()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.OverwriteDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"zones/{ZoneId}/dns_records/{RecordId}", callback.RequestPath);
			Assert.IsNotNull(callback.Request);

			Assert.AreEqual("example.com", callback.Request.Name);
			Assert.AreEqual(DnsRecordType.A, callback.Request.Type);
			Assert.AreEqual("127.0.1.22", callback.Request.Content);

			_clientMock.Verify(m => m.PutAsync<DnsRecord, InternalDnsRecordRequest>($"zones/{ZoneId}/dns_records/{RecordId}", It.IsAny<InternalDnsRecordRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PutAsync<DnsRecord, InternalDnsRecordRequest>(It.IsAny<string>(), It.IsAny<InternalDnsRecordRequest>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalDnsRecordRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
