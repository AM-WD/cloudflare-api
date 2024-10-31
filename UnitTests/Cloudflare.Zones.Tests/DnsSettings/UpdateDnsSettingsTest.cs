using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using AMWD.Net.Api.Cloudflare.Zones.Internals.Requests;
using Moq;

namespace Cloudflare.Zones.Tests.DnsSettings
{
	[TestClass]
	public class UpdateDnsSettingsTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<ZoneDnsSetting> _response;

		private List<(string RequestPath, InternalUpdateDnsSettingsRequest Request)> _callbacks;

		private UpdateDnsSettingsRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<ZoneDnsSetting>
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
				Result = new ZoneDnsSetting
				{
					FlattenAllCnames = false,
					FoundationDns = false,
					MultiProvider = false,
					Nameservers = new Nameserver
					{
						Type = NameserverType.CloudflareStandard,
					},
					NameserverTtl = 86400,
					SecondaryOverrides = false,
					SOA = new StartOfAuthority
					(
						expire: 604800,
						minimumTtl: 1800,
						primaryNameserver: "kristina.ns.cloudflare.com",
						refresh: 10000,
						retry: 2400,
						zoneAdministrator: "admin.example.com",
						ttl: 3600
					),
					Mode = ZoneMode.DnsOnly
				}
			};

			_request = new UpdateDnsSettingsRequest(ZoneId)
			{
				FlattenAllCnames = true,
				FoundationDns = false,
				MultiProvider = false,
				Nameservers = new Nameserver
				{
					Type = NameserverType.CloudflareRandom
				},
				NameserverTtl = 86400,
				SecondaryOverrides = false,
				SOA = new StartOfAuthority
				(
					expire: 604800,
					minimumTtl: 1800,
					primaryNameserver: "ns1.example.org",
					refresh: 28800,
					retry: 3600,
					ttl: 43200,
					zoneAdministrator: "admin.example.org"
				),
				Mode = ZoneMode.Standard
			};
		}

		[TestMethod]
		public async Task ShouldUpdateDnsSettingsFull()
		{
			// Arrange
			if (_request == null) throw new InvalidOperationException();
			var client = GetClient();

			// Act
			var response = await client.UpdateZoneDnsSettings(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response?.Result, response.Result);

			Assert.AreEqual(1, _callbacks?.Count);

			var callback = _callbacks?.First();
			Assert.AreEqual($"zones/{ZoneId}/dns_settings", callback?.RequestPath);

			Assert.IsNotNull(callback?.Request);
			Assert.IsTrue(callback?.Request.FlattenAllCnames);
			Assert.IsFalse(callback?.Request.FoundationDns);
			Assert.IsFalse(callback?.Request.MultiProvider);
			Assert.IsNotNull(callback?.Request.Nameservers);
			Assert.AreEqual(NameserverType.CloudflareRandom, callback?.Request.Nameservers.Type);
			Assert.AreEqual(86400, callback?.Request.NameserverTtl);
			Assert.IsFalse(callback?.Request.SecondaryOverrides);
			Assert.IsNotNull(callback?.Request.Soa);
			Assert.AreEqual(604800, callback?.Request.Soa.Expire);
			Assert.AreEqual(1800, callback?.Request.Soa.MinimumTtl);
			Assert.AreEqual("ns1.example.org", callback?.Request.Soa.PrimaryNameserver);
			Assert.AreEqual(28800, callback?.Request.Soa.Refresh);
			Assert.AreEqual(3600, callback?.Request.Soa.Retry);
			Assert.AreEqual(43200, callback?.Request.Soa.Ttl);
			Assert.AreEqual("admin.example.org", callback?.Request.Soa.ZoneAdministrator);
			Assert.AreEqual(ZoneMode.Standard, callback?.Request.Mode);

			_clientMock?.Verify(m => m.PatchAsync<ZoneDnsSetting, InternalUpdateDnsSettingsRequest>($"zones/{ZoneId}/dns_settings", It.IsAny<InternalUpdateDnsSettingsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock?.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldUpdateDnsSettingsNone()
		{
			// Arrange
			var request = new UpdateDnsSettingsRequest(ZoneId);
			var client = GetClient();

			// Act
			var response = await client.UpdateZoneDnsSettings(request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response?.Result, response.Result);

			Assert.AreEqual(1, _callbacks?.Count);

			var callback = _callbacks?.First();
			Assert.AreEqual($"zones/{ZoneId}/dns_settings", callback?.RequestPath);

			Assert.IsNotNull(callback?.Request);
			Assert.IsNull(callback?.Request.FlattenAllCnames);
			Assert.IsNull(callback?.Request.FoundationDns);
			Assert.IsNull(callback?.Request.MultiProvider);
			Assert.IsNull(callback?.Request.Nameservers);
			Assert.IsNull(callback?.Request.NameserverTtl);
			Assert.IsNull(callback?.Request.SecondaryOverrides);
			Assert.IsNull(callback?.Request.Soa);
			Assert.IsNull(callback?.Request.Mode);

			_clientMock?.Verify(m => m.PatchAsync<ZoneDnsSetting, InternalUpdateDnsSettingsRequest>($"zones/{ZoneId}/dns_settings", It.IsAny<InternalUpdateDnsSettingsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock?.VerifyNoOtherCalls();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidMode()
		{
			// Arrange
			if (_request == null) throw new InvalidOperationException();
			_request.Mode = 0;
			var client = GetClient();

			// Act
			await client.UpdateZoneDnsSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidNameserverType()
		{
			// Arrange
			if (_request == null || _request.Nameservers == null) throw new InvalidOperationException();
			_request.Nameservers.Type = 0;
			var client = GetClient();

			// Act
			await client.UpdateZoneDnsSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(29)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidNameserverTtl(int ttl)
		{
			// Arrange
			if (_request == null) throw new InvalidOperationException();
			_request.NameserverTtl = ttl;
			var client = GetClient();

			// Act
			await client.UpdateZoneDnsSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(86399)]
		[DataRow(2419201)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaExpire(int ttl)
		{
			// Arrange
			if (_request == null || _request.SOA == null) throw new InvalidOperationException();
			_request.SOA.Expire = ttl;
			var client = GetClient();

			// Act
			await client.UpdateZoneDnsSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(59)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaMinimumTtl(int ttl)
		{
			// Arrange
			if (_request == null || _request.SOA == null) throw new InvalidOperationException();
			_request.SOA.MinimumTtl = ttl;
			var client = GetClient();

			// Act
			await client.UpdateZoneDnsSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForMissingSoaNameserver(string nameserver)
		{
			// Arrange
			if (_request == null || _request.SOA == null) throw new InvalidOperationException();
			_request.SOA.PrimaryNameserver = nameserver;
			var client = GetClient();

			// Act
			await client.UpdateZoneDnsSettings(_request);

			// Assert - ArgumentNullException
		}

		[DataTestMethod]
		[DataRow(599)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaRefresh(int ttl)
		{
			// Arrange
			if (_request == null || _request.SOA == null) throw new InvalidOperationException();
			_request.SOA.Refresh = ttl;
			var client = GetClient();

			// Act
			await client.UpdateZoneDnsSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(599)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaRetry(int ttl)
		{
			// Arrange
			if (_request == null || _request.SOA == null) throw new InvalidOperationException();
			_request.SOA.Retry = ttl;
			var client = GetClient();

			// Act
			await client.UpdateZoneDnsSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(299)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaTtl(int ttl)
		{
			// Arrange
			if (_request == null || _request.SOA == null) throw new InvalidOperationException();
			_request.SOA.Ttl = ttl;
			var client = GetClient();

			// Act
			await client.UpdateZoneDnsSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForMissingSoaAdministrator(string admin)
		{
			// Arrange
			if (_request == null || _request.SOA == null) throw new InvalidOperationException();
			_request.SOA.ZoneAdministrator = admin;
			var client = GetClient();

			// Act
			await client.UpdateZoneDnsSettings(_request);

			// Assert - ArgumentNullException
		}

		private ICloudflareClient GetClient()
		{
			if (_response == null) throw new InvalidOperationException();

			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PatchAsync<ZoneDnsSetting, InternalUpdateDnsSettingsRequest>(It.IsAny<string>(), It.IsAny<InternalUpdateDnsSettingsRequest>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalUpdateDnsSettingsRequest, CancellationToken>((requestPath, request, _) => _callbacks?.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
