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
	public class CreateDnsRecordTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private List<(string RequestPath, InternalDnsRecordRequest Request, IQueryParameterFilter QueryFilter)> _callbacks;
		private CloudflareResponse<DnsRecord> _response;
		private CreateDnsRecordRequest _request;

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
				Result = new CNAMERecord("example.com")
				{
					Id = "023e105f4ecef8ad9ca31a8372d0c353",
					Name = "test.example.com",
					Content = "public.r2.dev",
					Proxiable = true,
					Proxied = true,
					TimeToLive = 1,
					Settings = new CNAMERecordSettings(),
					Meta = new JObject
					{
						["r2_bucket"] = "test",
						["read_only"] = true
					},
					Comment = "Certificate authority verification record",
					Tags = [],
					CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
					ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
					CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
					TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
				}
			};

			_request = new CreateDnsRecordRequest(ZoneId, "example.com")
			{
				Comment = "Server location record",
				Data = new LOCRecordData
				{
					LatitudeDegrees = 48,
					LatitudeMinutes = 8,
					LatitudeSeconds = 8.12682,
					LatitudeDirection = LOCRecordLatitudeDirection.North,

					LongitudeDegrees = 11,
					LongitudeMinutes = 34,
					LongitudeSeconds = 30.9576,
					LongitudeDirection = LOCRecordLongitudeDirection.East,

					Altitude = 100,
					Size = 80,
					PrecisionHorizontal = 500,
					PrecisionVertical = 400
				},
				Proxied = false,
				Tags = ["important"],
				TimeToLive = 1,
				Type = DnsRecordType.LOC
			};
		}

		[TestMethod]
		public async Task ShouldCreateDnsRecord()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records", callback.RequestPath);
			Assert.IsNull(callback.QueryFilter);

			Assert.IsNotNull(callback.Request);
			Assert.AreEqual(_request.Name, callback.Request.Name);
			Assert.AreEqual(_request.Type, callback.Request.Type);
			Assert.IsNull(callback.Request.Content);
			Assert.IsNotNull(callback.Request.Data);
			Assert.IsNull(callback.Request.Settings);
			Assert.IsNull(callback.Request.Priority);
			Assert.AreEqual(_request.Proxied, callback.Request.Proxied);
			Assert.AreEqual(_request.Comment, callback.Request.Comment);
			Assert.AreEqual(_request.TimeToLive, callback.Request.Ttl);
			CollectionAssert.AreEqual(_request.Tags.ToArray(), callback.Request.Tags.ToArray());

			Assert.IsInstanceOfType<LOCRecordData>(callback.Request.Data);
			var locData = callback.Request.Data as LOCRecordData;
			Assert.AreEqual(48, locData.LatitudeDegrees);
			Assert.AreEqual(8, locData.LatitudeMinutes);
			Assert.AreEqual(8.126, locData.LatitudeSeconds);
			Assert.AreEqual(LOCRecordLatitudeDirection.North, locData.LatitudeDirection);
			Assert.AreEqual(11, locData.LongitudeDegrees);
			Assert.AreEqual(34, locData.LongitudeMinutes);
			Assert.AreEqual(30.957, locData.LongitudeSeconds);
			Assert.AreEqual(LOCRecordLongitudeDirection.East, locData.LongitudeDirection);
			Assert.AreEqual(100, locData.Altitude);
			Assert.AreEqual(80, locData.Size);
			Assert.AreEqual(500, locData.PrecisionHorizontal);
			Assert.AreEqual(400, locData.PrecisionVertical);

			_clientMock.Verify(m => m.PostAsync<DnsRecord, InternalDnsRecordRequest>($"/zones/{ZoneId}/dns_records", It.IsAny<InternalDnsRecordRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForName(string str)
		{
			// Arrange
			_request.Name = str;
			var client = GetClient();

			// Act
			await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForType()
		{
			// Arrange
			_request.Type = 0;
			var client = GetClient();

			// Act
			await client.CreateDnsRecord(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForContent(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.A;
			_request.Content = str;
			var client = GetClient();

			// Act
			await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		[TestMethod]
		public async Task ShouldSetContentForType()
		{
			// Arrange
			_request.Type = DnsRecordType.A;
			_request.Content = "127.0.1.53";
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual(_request.Content, callback.Request.Content);

			_clientMock.Verify(m => m.PostAsync<DnsRecord, InternalDnsRecordRequest>($"/zones/{ZoneId}/dns_records", It.IsAny<InternalDnsRecordRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldNotSetSettingsWhenObjectIsNull()
		{
			// Arrange
			_request.Type = DnsRecordType.CNAME;
			_request.Content = "www.example.com.";
			_request.Settings = null;
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.IsNull(callback.Request.Settings);

			_clientMock.Verify(m => m.PostAsync<DnsRecord, InternalDnsRecordRequest>($"/zones/{ZoneId}/dns_records", It.IsAny<InternalDnsRecordRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldNotSetSettingsWhenObjectIsNotCorrectType()
		{
			// Arrange
			_request.Type = DnsRecordType.CNAME;
			_request.Content = "www.example.com.";
			_request.Settings = new();
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.IsNull(callback.Request.Settings);

			_clientMock.Verify(m => m.PostAsync<DnsRecord, InternalDnsRecordRequest>($"/zones/{ZoneId}/dns_records", It.IsAny<InternalDnsRecordRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldSetSettings()
		{
			// Arrange
			_request.Type = DnsRecordType.CNAME;
			_request.Content = "www.example.com.";
			_request.Settings = new CNAMERecordSettings { FlattenCname = true };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.IsNotNull(callback.Request.Settings);

			Assert.IsInstanceOfType<CNAMERecordSettings>(callback.Request.Settings);
			Assert.AreEqual(true, ((CNAMERecordSettings)callback.Request.Settings).FlattenCname);

			_clientMock.Verify(m => m.PostAsync<DnsRecord, InternalDnsRecordRequest>($"/zones/{ZoneId}/dns_records", It.IsAny<InternalDnsRecordRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForData()
		{
			// Arrange
			_request.Data = null;
			var client = GetClient();

			// Act
			await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		[DataTestMethod]
		[DataRow(DnsRecordType.MX)]
		[DataRow(DnsRecordType.SRV)]
		[DataRow(DnsRecordType.URI)]
		public async Task ShouldSetPriority(DnsRecordType type)
		{
			// Arrange
			_request.Type = type;
			_request.Priority = (int)type;

			switch (type)
			{
				case DnsRecordType.MX: _request.Content = "www.example.com."; break;
				case DnsRecordType.SRV: _request.Data = new SRVRecordData { Port = 10, Priority = 0, Target = ".", Weight = 10 }; break;
				case DnsRecordType.URI: _request.Data = new URIRecordData { Target = ".", Weight = 10 }; break;
			}

			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual(_request.Priority, callback.Request.Priority);

			_clientMock.Verify(m => m.PostAsync<DnsRecord, InternalDnsRecordRequest>($"/zones/{ZoneId}/dns_records", It.IsAny<InternalDnsRecordRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[DataTestMethod]
		[DataRow(DnsRecordType.MX)]
		[DataRow(DnsRecordType.SRV)]
		[DataRow(DnsRecordType.URI)]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForPriority(DnsRecordType type)
		{
			// Arrange
			_request.Type = type;
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		[DataTestMethod]
		[DataRow(1)]
		[DataRow(3600)]
		public async Task ShouldSetTtl(int ttl)
		{
			// Arrange
			_request.TimeToLive = ttl;
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual(ttl, callback.Request.Ttl);

			_clientMock.Verify(m => m.PostAsync<DnsRecord, InternalDnsRecordRequest>($"/zones/{ZoneId}/dns_records", It.IsAny<InternalDnsRecordRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[DataTestMethod]
		[DataRow(0)]
		[DataRow(20)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForTtl(int ttl)
		{
			// Arrange
			_request.TimeToLive = ttl;
			var client = GetClient();

			// Act
			await client.CreateDnsRecord(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(DnsRecordType.CAA)]
		[DataRow(DnsRecordType.CERT)]
		[DataRow(DnsRecordType.DNSKEY)]
		[DataRow(DnsRecordType.DS)]
		[DataRow(DnsRecordType.HTTPS)]
		[DataRow(DnsRecordType.LOC)]
		[DataRow(DnsRecordType.NAPTR)]
		[DataRow(DnsRecordType.SMIMEA)]
		[DataRow(DnsRecordType.SRV)]
		[DataRow(DnsRecordType.SSHFP)]
		[DataRow(DnsRecordType.SVCB)]
		[DataRow(DnsRecordType.TLSA)]
		[DataRow(DnsRecordType.URI)]
		[ExpectedException(typeof(ArgumentException))]
		public async Task ShouldThrowArgumentExceptionForInvalidTypeOfData(DnsRecordType type)
		{
			// Arrange
			_request.Type = type;
			_request.Priority = 10;
			_request.Data = new object();
			var client = GetClient();

			// Act
			await client.CreateDnsRecord(_request);

			// Assert - ArgumentException
		}

		#region CAA

		[TestMethod]
		public async Task ShouldSetCaaData()
		{
			// Arrange
			_request.Type = DnsRecordType.CAA;
			_request.Data = new CAARecordData { Flags = 1, Tag = "issue", Value = "letsencrypt.org" };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.IsNotNull(callback.Request.Data);
			Assert.IsInstanceOfType<CAARecordData>(callback.Request.Data);

			var data = (CAARecordData)callback.Request.Data;
			Assert.AreEqual(1, data.Flags);
			Assert.AreEqual("issue", data.Tag);
			Assert.AreEqual("letsencrypt.org", data.Value);
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForCaaDataTag(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.CAA;
			_request.Data = new CAARecordData { Flags = 1, Tag = str, Value = "letsencrypt.org" };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForCaaDataValue(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.CAA;
			_request.Data = new CAARecordData { Flags = 1, Tag = "issue", Value = str };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		#endregion CAA

		#region CERT

		[TestMethod]
		public async Task ShouldSetCertData()
		{
			// Arrange
			_request.Type = DnsRecordType.CERT;
			_request.Data = new CERTRecordData { Algorithm = 1, Certificate = "test", KeyTag = 2, Type = 3 };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.IsNotNull(callback.Request.Data);
			Assert.IsInstanceOfType(callback.Request.Data, typeof(CERTRecordData));

			var data = (CERTRecordData)callback.Request.Data;
			Assert.AreEqual(1, data.Algorithm);
			Assert.AreEqual("test", data.Certificate);
			Assert.AreEqual(2, data.KeyTag);
			Assert.AreEqual(3, data.Type);
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForCertDataValue(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.CERT;
			_request.Data = new CERTRecordData { Algorithm = 1, Certificate = str, KeyTag = 2, Type = 3 };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		#endregion CERT

		#region DNSKEY

		[TestMethod]
		public async Task ShouldSetDnsKeyData()
		{
			// Arrange
			_request.Type = DnsRecordType.DNSKEY;
			_request.Data = new DNSKEYRecordData { Algorithm = 1, Flags = 2, Protocol = 3, PublicKey = "test" };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.IsNotNull(callback.Request.Data);
			Assert.IsInstanceOfType(callback.Request.Data, typeof(DNSKEYRecordData));

			var data = (DNSKEYRecordData)callback.Request.Data;
			Assert.AreEqual(1, data.Algorithm);
			Assert.AreEqual(2, data.Flags);
			Assert.AreEqual(3, data.Protocol);
			Assert.AreEqual("test", data.PublicKey);
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForDnsKeyDataPublicKey(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.DNSKEY;
			_request.Data = new DNSKEYRecordData { Algorithm = 1, Flags = 2, Protocol = 3, PublicKey = str };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		#endregion DNSKEY

		#region DS

		[TestMethod]
		public async Task ShouldSetDsData()
		{
			// Arrange
			_request.Type = DnsRecordType.DS;
			_request.Data = new DSRecordData { Algorithm = 1, Digest = "test", DigestType = 3, KeyTag = 4 };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.IsNotNull(callback.Request.Data);
			Assert.IsInstanceOfType(callback.Request.Data, typeof(DSRecordData));

			var data = (DSRecordData)callback.Request.Data;
			Assert.AreEqual(1, data.Algorithm);
			Assert.AreEqual("test", data.Digest);
			Assert.AreEqual(3, data.DigestType);
			Assert.AreEqual(4, data.KeyTag);
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForDsDataDigest(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.DS;
			_request.Data = new DSRecordData { Algorithm = 1, Digest = str, DigestType = 3, KeyTag = 4 };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		#endregion DS

		#region HTTPS

		[TestMethod]
		public async Task ShouldSetHttpsData()
		{
			// Arrange
			_request.Type = DnsRecordType.HTTPS;
			_request.Data = new HTTPSRecordData { Priority = 10, Target = ".", Value = "foo.bar" };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.IsNotNull(callback.Request.Data);
			Assert.IsInstanceOfType(callback.Request.Data, typeof(HTTPSRecordData));

			var data = (HTTPSRecordData)callback.Request.Data;
			Assert.AreEqual(10, data.Priority);
			Assert.AreEqual(".", data.Target);
			Assert.AreEqual("foo.bar", data.Value);
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForHttpsDataTarget(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.HTTPS;
			_request.Data = new HTTPSRecordData { Priority = 10, Target = str, Value = "foo.bar" };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForHttpsDataValue(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.HTTPS;
			_request.Data = new HTTPSRecordData { Priority = 10, Target = ".", Value = str };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		#endregion HTTPS

		#region LOC

		[DataTestMethod]
		[DataRow(-1)]
		[DataRow(91)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForLocDataLatitudeDegree(int val)
		{
			// Arrange
			((LOCRecordData)_request.Data).LatitudeDegrees = val;
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(-1)]
		[DataRow(60)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForLocDataLatitudeMinutes(int val)
		{
			// Arrange
			((LOCRecordData)_request.Data).LatitudeMinutes = val;
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(-1.0)]
		[DataRow(59.9991)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForLocDataLatitudeSeconds(double val)
		{
			// Arrange
			((LOCRecordData)_request.Data).LatitudeSeconds = val;
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForLocDataLatitudeDirection()
		{
			// Arrange
			((LOCRecordData)_request.Data).LatitudeDirection = 0;
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(-1)]
		[DataRow(181)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForLocDataLongitudeDegree(int val)
		{
			// Arrange
			((LOCRecordData)_request.Data).LongitudeDegrees = val;
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(-1)]
		[DataRow(60)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForLocDataLongitudeMinutes(int val)
		{
			// Arrange
			((LOCRecordData)_request.Data).LongitudeMinutes = val;
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(-1.0)]
		[DataRow(59.9991)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForLocDataLongitudeSeconds(double val)
		{
			// Arrange
			((LOCRecordData)_request.Data).LongitudeSeconds = val;
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForLocDataLongitudeDirection()
		{
			// Arrange
			((LOCRecordData)_request.Data).LongitudeDirection = 0;
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(-100_000.1)]
		[DataRow(42_849_672.951)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForLocDataAltitde(double val)
		{
			// Arrange
			((LOCRecordData)_request.Data).Altitude = val;
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(-1)]
		[DataRow(90_000_001)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForLocDataSize(int val)
		{
			// Arrange
			((LOCRecordData)_request.Data).Size = val;
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(-1)]
		[DataRow(90_000_001)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForLocDataPrecisionHorizontal(int val)
		{
			// Arrange
			((LOCRecordData)_request.Data).PrecisionHorizontal = val;
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(-1)]
		[DataRow(90_000_001)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForLocDataPrecisionVertical(int val)
		{
			// Arrange
			((LOCRecordData)_request.Data).PrecisionVertical = val;
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentOutOfRangeException
		}

		#endregion LOC

		#region NAPTR

		[TestMethod]
		public async Task ShouldSetNaPtrData()
		{
			// Arrange
			_request.Type = DnsRecordType.NAPTR;
			_request.Data = new NAPTRRecordData { Flags = "ab", Order = 1, Preference = 2, Regex = "cd", Replacement = "ef", Service = "gh" };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.IsNotNull(callback.Request.Data);
			Assert.IsInstanceOfType(callback.Request.Data, typeof(NAPTRRecordData));

			var data = (NAPTRRecordData)callback.Request.Data;
			Assert.AreEqual("ab", data.Flags);
			Assert.AreEqual(1, data.Order);
			Assert.AreEqual(2, data.Preference);
			Assert.AreEqual("cd", data.Regex);
			Assert.AreEqual("ef", data.Replacement);
			Assert.AreEqual("gh", data.Service);
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForNaPtrDataFlags(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.NAPTR;
			_request.Data = new NAPTRRecordData { Flags = str, Order = 1, Preference = 2, Regex = "cd", Replacement = "ef", Service = "gh" };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForNaPtrDataRegex(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.NAPTR;
			_request.Data = new NAPTRRecordData { Flags = "ab", Order = 1, Preference = 2, Regex = str, Replacement = "ef", Service = "gh" };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForNaPtrDataReplacement(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.NAPTR;
			_request.Data = new NAPTRRecordData { Flags = "ab", Order = 1, Preference = 2, Regex = "cd", Replacement = str, Service = "gh" };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForNaPtrDataService(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.NAPTR;
			_request.Data = new NAPTRRecordData { Flags = "ab", Order = 1, Preference = 2, Regex = "cd", Replacement = "ef", Service = str };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		#endregion NAPTR

		#region SMIMEA

		[TestMethod]
		public async Task ShouldSetSMimeAData()
		{
			// Arrange
			_request.Type = DnsRecordType.SMIMEA;
			_request.Data = new SMIMEARecordData { Certificate = "cert", MatchingType = 1, Selector = 2, Usage = 3 };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.IsNotNull(callback.Request.Data);
			Assert.IsInstanceOfType(callback.Request.Data, typeof(SMIMEARecordData));

			var data = (SMIMEARecordData)callback.Request.Data;
			Assert.AreEqual("cert", data.Certificate);
			Assert.AreEqual(1, data.MatchingType);
			Assert.AreEqual(2, data.Selector);
			Assert.AreEqual(3, data.Usage);
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForSMimeACertificate(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.SMIMEA;
			_request.Data = new SMIMEARecordData { Certificate = str, MatchingType = 1, Selector = 2, Usage = 3 };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		#endregion SMIMEA

		#region SRV

		[TestMethod]
		public async Task ShouldSetSrvData()
		{
			// Arrange
			_request.Type = DnsRecordType.SRV;
			_request.Priority = 10;
			_request.Data = new SRVRecordData { Port = 123, Priority = 345, Target = ".", Weight = 456 };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.IsNotNull(callback.Request.Data);
			Assert.IsInstanceOfType(callback.Request.Data, typeof(SRVRecordData));

			var data = (SRVRecordData)callback.Request.Data;
			Assert.AreEqual(123, data.Port);
			Assert.AreEqual(345, data.Priority);
			Assert.AreEqual(".", data.Target);
			Assert.AreEqual(456, data.Weight);
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForSrvTarget(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.SRV;
			_request.Priority = 10;
			_request.Data = new SRVRecordData { Port = 123, Priority = 345, Target = str, Weight = 456 };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		#endregion SRV

		#region SSHFP

		[TestMethod]
		public async Task ShouldSetSshFpData()
		{
			// Arrange
			_request.Type = DnsRecordType.SSHFP;
			_request.Data = new SSHFPRecordData { Algorithm = 1, Fingerprint = "fingerprint", Type = 2 };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.IsNotNull(callback.Request.Data);
			Assert.IsInstanceOfType(callback.Request.Data, typeof(SSHFPRecordData));

			var data = (SSHFPRecordData)callback.Request.Data;
			Assert.AreEqual(1, data.Algorithm);
			Assert.AreEqual("fingerprint", data.Fingerprint);
			Assert.AreEqual(2, data.Type);
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForSshFpFingerprint(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.SSHFP;
			_request.Data = new SSHFPRecordData { Algorithm = 1, Fingerprint = str, Type = 2 };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		#endregion SSHFP

		#region SVCB

		[TestMethod]
		public async Task ShouldSetSvcBData()
		{
			// Arrange
			_request.Type = DnsRecordType.SVCB;
			_request.Data = new SVCBRecordData { Priority = 10, Target = ".", Value = "example.com" };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.IsNotNull(callback.Request.Data);
			Assert.IsInstanceOfType(callback.Request.Data, typeof(SVCBRecordData));

			var data = (SVCBRecordData)callback.Request.Data;
			Assert.AreEqual(10, data.Priority);
			Assert.AreEqual(".", data.Target);
			Assert.AreEqual("example.com", data.Value);
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForSvcBTarget(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.SVCB;
			_request.Data = new SVCBRecordData { Priority = 10, Target = str, Value = "example.com" };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForSvcBValue(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.SVCB;
			_request.Data = new SVCBRecordData { Priority = 10, Target = ".", Value = str };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		#endregion SVCB

		#region TLSA

		[TestMethod]
		public async Task ShouldSetTlsAData()
		{
			// Arrange
			_request.Type = DnsRecordType.TLSA;
			_request.Data = new TLSARecordData { Certificate = "cert", MatchingType = 1, Selector = 2, Usage = 3 };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.IsNotNull(callback.Request.Data);
			Assert.IsInstanceOfType(callback.Request.Data, typeof(TLSARecordData));

			var data = (TLSARecordData)callback.Request.Data;
			Assert.AreEqual("cert", data.Certificate);
			Assert.AreEqual(1, data.MatchingType);
			Assert.AreEqual(2, data.Selector);
			Assert.AreEqual(3, data.Usage);
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForTlsACertificate(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.TLSA;
			_request.Data = new TLSARecordData { Certificate = str, MatchingType = 1, Selector = 2, Usage = 3 };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		#endregion TLSA

		#region URI

		[TestMethod]
		public async Task ShouldSetUriData()
		{
			// Arrange
			_request.Type = DnsRecordType.URI;
			_request.Priority = 10;
			_request.Data = new URIRecordData { Target = "aim", Weight = 10 };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.IsNotNull(callback.Request.Data);
			Assert.IsInstanceOfType(callback.Request.Data, typeof(URIRecordData));

			var data = (URIRecordData)callback.Request.Data;
			Assert.AreEqual("aim", data.Target);
			Assert.AreEqual(10, data.Weight);
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForUriTarget(string str)
		{
			// Arrange
			_request.Type = DnsRecordType.URI;
			_request.Priority = 10;
			_request.Data = new URIRecordData { Target = str, Weight = 10 };
			var client = GetClient();

			// Act
			var response = await client.CreateDnsRecord(_request);

			// Assert - ArgumentNullException
		}

		#endregion URI

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<DnsRecord, InternalDnsRecordRequest>(It.IsAny<string>(), It.IsAny<InternalDnsRecordRequest>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalDnsRecordRequest, IQueryParameterFilter, CancellationToken>((requestPath, request, queryFilter, _) => _callbacks.Add((requestPath, request, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
