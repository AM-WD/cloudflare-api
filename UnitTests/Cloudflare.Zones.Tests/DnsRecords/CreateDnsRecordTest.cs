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
					Id = "023e105f4ecef8ad9ca31a8372d0c353",
					ZoneId = ZoneId,
					ZoneName = "example.com",
					Name = "example.com",
					Type = DnsRecordType.Caa,
					Content = "0 issuewild \"letsencrypt.org\"",
					Proxiable = false,
					Proxied = false,
					Ttl = 1,
					Data = new DnsRecord.CaaData(0, "issuewild", "letsencrypt.org"),
					Settings = new(),
					Meta = new DnsRecordMeta
					{
						AutoAdded = false,
						ManagedByApps = false,
						ManagedByArgoTunnel = false
					},
					Comment = "Certificate authority verification record",
					Tags = [],
					CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
					ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
					CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
					TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
				}
			};

			_request = new CreateDnsRecordRequest(ZoneId, "example.com", DnsRecordType.Loc)
			{
				Comment = "Server location record",
				Data = new DnsRecord.LocData(
					latitudeDegrees: 48,
					latitudeMinutes: 8,
					latitudeSeconds: 8.12682,
					latitudeDirection: DnsRecord.LatitudeDirection.North,

					longitudeDegrees: 11,
					longitudeMinutes: 34,
					longitudeSeconds: 30.9576,
					longitudeDirection: DnsRecord.LongitudeDirection.East,

					altitude: 100,
					size: 80,
					precisionHorizontal: 500,
					precisionVertical: 400
				),
				Proxied = false,
				Settings = new(),
				Tags = ["important"],
				Ttl = 1,
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
			Assert.AreEqual($"zones/{ZoneId}/dns_records", callback.RequestPath);
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
			Assert.AreEqual(_request.Ttl, callback.Request.Ttl);
			CollectionAssert.AreEqual(_request.Tags.ToArray(), callback.Request.Tags.ToArray());

			Assert.IsInstanceOfType<DnsRecord.LocData>(callback.Request.Data);
			var locData = callback.Request.Data as DnsRecord.LocData;
			Assert.AreEqual(48, locData.LatitudeDegrees);
			Assert.AreEqual(8, locData.LatitudeMinutes);
			Assert.AreEqual(8.126, locData.LatitudeSeconds);
			Assert.AreEqual(DnsRecord.LatitudeDirection.North, locData.LatitudeDirection);
			Assert.AreEqual(11, locData.LongitudeDegrees);
			Assert.AreEqual(34, locData.LongitudeMinutes);
			Assert.AreEqual(30.957, locData.LongitudeSeconds);
			Assert.AreEqual(DnsRecord.LongitudeDirection.East, locData.LongitudeDirection);
			Assert.AreEqual(100, locData.Altitude);
			Assert.AreEqual(80, locData.Size);
			Assert.AreEqual(500, locData.PrecisionHorizontal);
			Assert.AreEqual(400, locData.PrecisionVertical);

			_clientMock.Verify(m => m.PostAsync<DnsRecord, InternalDnsRecordRequest>($"zones/{ZoneId}/dns_records", It.IsAny<InternalDnsRecordRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
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

			_clientMock.Verify(m => m.PostAsync<DnsRecord, InternalDnsRecordRequest>($"zones/{ZoneId}/dns_records", It.IsAny<InternalDnsRecordRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldNotSetSettingsWhenObjectIsNull()
		{
			// Arrange
			_request.Type = DnsRecordType.Cname;
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

			_clientMock.Verify(m => m.PostAsync<DnsRecord, InternalDnsRecordRequest>($"zones/{ZoneId}/dns_records", It.IsAny<InternalDnsRecordRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldNotSetSettingsWhenObjectIsNotCorrectType()
		{
			// Arrange
			_request.Type = DnsRecordType.Cname;
			_request.Content = "www.example.com.";
			_request.Settings = new object();
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

			_clientMock.Verify(m => m.PostAsync<DnsRecord, InternalDnsRecordRequest>($"zones/{ZoneId}/dns_records", It.IsAny<InternalDnsRecordRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldSetSettings()
		{
			// Arrange
			_request.Type = DnsRecordType.Cname;
			_request.Content = "www.example.com.";
			_request.Settings = new DnsRecord.CnameSettings(true);
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

			Assert.IsInstanceOfType<DnsRecord.CnameSettings>(callback.Request.Settings);
			Assert.AreEqual(true, ((DnsRecord.CnameSettings)callback.Request.Settings).FlattenCname);

			_clientMock.Verify(m => m.PostAsync<DnsRecord, InternalDnsRecordRequest>($"zones/{ZoneId}/dns_records", It.IsAny<InternalDnsRecordRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
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
		[DataRow(DnsRecordType.Mx)]
		[DataRow(DnsRecordType.Srv)]
		[DataRow(DnsRecordType.Uri)]
		public async Task ShouldSetPriority(DnsRecordType type)
		{
			// Arrange
			_request.Type = type;
			_request.Priority = (ushort)type;

			switch (type)
			{
				case DnsRecordType.Mx: _request.Content = "www.example.com."; break;
				case DnsRecordType.Srv: _request.Data = new DnsRecord.SrvData(10, 0, ".", 10); break;
				case DnsRecordType.Uri: _request.Data = new DnsRecord.UriData(".", 10); break;
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

			_clientMock.Verify(m => m.PostAsync<DnsRecord, InternalDnsRecordRequest>($"zones/{ZoneId}/dns_records", It.IsAny<InternalDnsRecordRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[DataTestMethod]
		[DataRow(DnsRecordType.Mx)]
		[DataRow(DnsRecordType.Srv)]
		[DataRow(DnsRecordType.Uri)]
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
			_request.Ttl = ttl;
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

			_clientMock.Verify(m => m.PostAsync<DnsRecord, InternalDnsRecordRequest>($"zones/{ZoneId}/dns_records", It.IsAny<InternalDnsRecordRequest>(), null, It.IsAny<CancellationToken>()), Times.Once);
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
			_request.Ttl = ttl;
			var client = GetClient();

			// Act
			await client.CreateDnsRecord(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(DnsRecordType.Caa)]
		[DataRow(DnsRecordType.Cert)]
		[DataRow(DnsRecordType.DnsKey)]
		[DataRow(DnsRecordType.Ds)]
		[DataRow(DnsRecordType.Https)]
		[DataRow(DnsRecordType.Loc)]
		[DataRow(DnsRecordType.NaPtr)]
		[DataRow(DnsRecordType.SMimeA)]
		[DataRow(DnsRecordType.Srv)]
		[DataRow(DnsRecordType.SshFp)]
		[DataRow(DnsRecordType.SvcB)]
		[DataRow(DnsRecordType.TlsA)]
		[DataRow(DnsRecordType.Uri)]
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
			_request.Type = DnsRecordType.Caa;
			_request.Data = new DnsRecord.CaaData(1, "issue", "letsencrypt.org");
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
			Assert.IsInstanceOfType(callback.Request.Data, typeof(DnsRecord.CaaData));

			var data = (DnsRecord.CaaData)callback.Request.Data;
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
			_request.Type = DnsRecordType.Caa;
			_request.Data = new DnsRecord.CaaData(1, str, "letsencrypt.org");
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
			_request.Type = DnsRecordType.Caa;
			_request.Data = new DnsRecord.CaaData(1, "issue", str);
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
			_request.Type = DnsRecordType.Cert;
			_request.Data = new DnsRecord.CertData(1, "test", 2, 3);
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
			Assert.IsInstanceOfType(callback.Request.Data, typeof(DnsRecord.CertData));

			var data = (DnsRecord.CertData)callback.Request.Data;
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
			_request.Type = DnsRecordType.Cert;
			_request.Data = new DnsRecord.CertData(1, str, 2, 3);
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
			_request.Type = DnsRecordType.DnsKey;
			_request.Data = new DnsRecord.DnsKeyData(1, 2, 3, "test");
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
			Assert.IsInstanceOfType(callback.Request.Data, typeof(DnsRecord.DnsKeyData));

			var data = (DnsRecord.DnsKeyData)callback.Request.Data;
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
			_request.Type = DnsRecordType.DnsKey;
			_request.Data = new DnsRecord.DnsKeyData(1, 2, 3, str);
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
			_request.Type = DnsRecordType.Ds;
			_request.Data = new DnsRecord.DsData(algorithm: 1, digest: "test", digestType: 3, keyTag: 4);
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
			Assert.IsInstanceOfType(callback.Request.Data, typeof(DnsRecord.DsData));

			var data = (DnsRecord.DsData)callback.Request.Data;
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
			_request.Type = DnsRecordType.Ds;
			_request.Data = new DnsRecord.DsData(algorithm: 1, digest: str, digestType: 3, keyTag: 4);
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
			_request.Type = DnsRecordType.Https;
			_request.Data = new DnsRecord.HttpsData(priority: 10, target: ".", value: "foo.bar");
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
			Assert.IsInstanceOfType(callback.Request.Data, typeof(DnsRecord.HttpsData));

			var data = (DnsRecord.HttpsData)callback.Request.Data;
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
			_request.Type = DnsRecordType.Https;
			_request.Data = new DnsRecord.HttpsData(priority: 10, target: str, value: "foo.bar");
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
			_request.Type = DnsRecordType.Https;
			_request.Data = new DnsRecord.HttpsData(priority: 10, target: ".", value: str);
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
			((DnsRecord.LocData)_request.Data).LatitudeDegrees = val;
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
			((DnsRecord.LocData)_request.Data).LatitudeMinutes = val;
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
			((DnsRecord.LocData)_request.Data).LatitudeSeconds = val;
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
			((DnsRecord.LocData)_request.Data).LatitudeDirection = 0;
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
			((DnsRecord.LocData)_request.Data).LongitudeDegrees = val;
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
			((DnsRecord.LocData)_request.Data).LongitudeMinutes = val;
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
			((DnsRecord.LocData)_request.Data).LongitudeSeconds = val;
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
			((DnsRecord.LocData)_request.Data).LongitudeDirection = 0;
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
			((DnsRecord.LocData)_request.Data).Altitude = val;
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
			((DnsRecord.LocData)_request.Data).Size = val;
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
			((DnsRecord.LocData)_request.Data).PrecisionHorizontal = val;
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
			((DnsRecord.LocData)_request.Data).PrecisionVertical = val;
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
			_request.Type = DnsRecordType.NaPtr;
			_request.Data = new DnsRecord.NaPtrData(flags: "ab", order: 1, preference: 2, regex: "cd", replacement: "ef", service: "gh");
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
			Assert.IsInstanceOfType(callback.Request.Data, typeof(DnsRecord.NaPtrData));

			var data = (DnsRecord.NaPtrData)callback.Request.Data;
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
			_request.Type = DnsRecordType.NaPtr;
			_request.Data = new DnsRecord.NaPtrData(flags: str, order: 1, preference: 2, regex: "cd", replacement: "ef", service: "gh");
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
			_request.Type = DnsRecordType.NaPtr;
			_request.Data = new DnsRecord.NaPtrData(flags: "ab", order: 1, preference: 2, regex: str, replacement: "ef", service: "gh");
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
			_request.Type = DnsRecordType.NaPtr;
			_request.Data = new DnsRecord.NaPtrData(flags: "ab", order: 1, preference: 2, regex: "cd", replacement: str, service: "gh");
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
			_request.Type = DnsRecordType.NaPtr;
			_request.Data = new DnsRecord.NaPtrData(flags: "ab", order: 1, preference: 2, regex: "cd", replacement: "ef", service: str);
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
			_request.Type = DnsRecordType.SMimeA;
			_request.Data = new DnsRecord.SMimeAData(certificate: "cert", matchingType: 1, selector: 2, usage: 3);
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
			Assert.IsInstanceOfType(callback.Request.Data, typeof(DnsRecord.SMimeAData));

			var data = (DnsRecord.SMimeAData)callback.Request.Data;
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
			_request.Type = DnsRecordType.SMimeA;
			_request.Data = new DnsRecord.SMimeAData(certificate: str, matchingType: 1, selector: 2, usage: 3);
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
			_request.Type = DnsRecordType.Srv;
			_request.Priority = 10;
			_request.Data = new DnsRecord.SrvData(port: 123, priority: 345, target: ".", weight: 456);
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
			Assert.IsInstanceOfType(callback.Request.Data, typeof(DnsRecord.SrvData));

			var data = (DnsRecord.SrvData)callback.Request.Data;
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
			_request.Type = DnsRecordType.Srv;
			_request.Priority = 10;
			_request.Data = new DnsRecord.SrvData(port: 123, priority: 345, target: str, weight: 456);
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
			_request.Type = DnsRecordType.SshFp;
			_request.Data = new DnsRecord.SshFpData(algorithm: 1, fingerprint: "fingerprint", type: 2);
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
			Assert.IsInstanceOfType(callback.Request.Data, typeof(DnsRecord.SshFpData));

			var data = (DnsRecord.SshFpData)callback.Request.Data;
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
			_request.Type = DnsRecordType.SshFp;
			_request.Data = new DnsRecord.SshFpData(algorithm: 1, fingerprint: str, type: 2);
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
			_request.Type = DnsRecordType.SvcB;
			_request.Data = new DnsRecord.SvcBData(priority: 10, target: ".", value: "example.com");
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
			Assert.IsInstanceOfType(callback.Request.Data, typeof(DnsRecord.SvcBData));

			var data = (DnsRecord.SvcBData)callback.Request.Data;
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
			_request.Type = DnsRecordType.SvcB;
			_request.Data = new DnsRecord.SvcBData(priority: 10, target: str, value: "example.com");
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
			_request.Type = DnsRecordType.SvcB;
			_request.Data = new DnsRecord.SvcBData(priority: 10, target: ".", value: str);
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
			_request.Type = DnsRecordType.TlsA;
			_request.Data = new DnsRecord.TlsAData(certificate: "cert", matchingType: 1, selector: 2, usage: 3);
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
			Assert.IsInstanceOfType(callback.Request.Data, typeof(DnsRecord.TlsAData));

			var data = (DnsRecord.TlsAData)callback.Request.Data;
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
			_request.Type = DnsRecordType.TlsA;
			_request.Data = new DnsRecord.TlsAData(certificate: str, matchingType: 1, selector: 2, usage: 3);
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
			_request.Type = DnsRecordType.Uri;
			_request.Priority = 10;
			_request.Data = new DnsRecord.UriData(target: "aim", weight: 10);
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
			Assert.IsInstanceOfType(callback.Request.Data, typeof(DnsRecord.UriData));

			var data = (DnsRecord.UriData)callback.Request.Data;
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
			_request.Type = DnsRecordType.Uri;
			_request.Priority = 10;
			_request.Data = new DnsRecord.UriData(target: str, weight: 10);
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
