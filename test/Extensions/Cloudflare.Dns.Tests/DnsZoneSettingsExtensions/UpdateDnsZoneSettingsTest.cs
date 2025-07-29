using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using AMWD.Net.Api.Cloudflare.Dns.Internals;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneSettingsExtensions
{
	[TestClass]
	public class UpdateDnsZoneSettingsTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<DnsZoneSettings> _response;
		private List<(string RequestPath, InternalUpdateDnsZoneSettingsRequest Request)> _callbacks;
		private UpdateDnsZoneSettingsRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<DnsZoneSettings>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = new DnsZoneSettings
				{
					FlattenAllCnames = true,
					FoundationDns = false,
					InternalDns = new DnsZoneInternalDns
					{
						ReferenceZoneId = ZoneId
					},
					MultiProvider = false,
					Nameservers = new DnsZoneNameservers(
						type: DnsZoneNameserversType.Zone
					),
					NameserverTtl = 86400,
					SecondaryOverrides = false,
					SOA = new DnsZoneSoa(
						expire: 604800,
						minttl: 1800,
						mname: "bob.ns.example.com",
						refresh: 10000,
						retry: 2400,
						rname: "admin.example.com",
						ttl: 3600
					),
					ZoneMode = DnsZoneMode.DnsOnly
				}
			};

			_request = new UpdateDnsZoneSettingsRequest(ZoneId)
			{
				FlattenAllCnames = true,
				FoundationDns = false,
				InternalDns = new DnsZoneInternalDns
				{
					ReferenceZoneId = ZoneId
				},
				MultiProvider = false,
				Nameservers = new DnsZoneNameservers(
					type: DnsZoneNameserversType.Standard
				),
				NameserverTtl = 86400,
				SecondaryOverrides = false,
				SOA = new DnsZoneSoa(
					expire: 604800,
					minttl: 1800,
					mname: "ns1.example.org",
					refresh: 28800,
					retry: 3600,
					rname: "admin.example.org",
					ttl: 43200
				),
				ZoneMode = DnsZoneMode.Standard
			};
		}

		[TestMethod]
		public async Task ShouldUpdateDnsSettingsFull()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.UpdateDnsZoneSettings(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_settings", callback.RequestPath);

			Assert.IsNotNull(callback.Request);
			Assert.IsTrue(callback.Request.FlattenAllCnames);
			Assert.IsFalse(callback.Request.FoundationDns);
			Assert.IsNotNull(callback.Request.InternalDns);
			Assert.AreEqual(ZoneId, callback.Request.InternalDns.ReferenceZoneId);
			Assert.IsFalse(callback.Request.MultiProvider);
			Assert.IsNotNull(callback.Request.Nameservers);
			Assert.AreEqual(DnsZoneNameserversType.Standard, callback.Request.Nameservers.Type);
			Assert.AreEqual(86400, callback.Request.NameserverTtl);
			Assert.IsFalse(callback.Request.SecondaryOverrides);
			Assert.IsNotNull(callback.Request.SOA);
			Assert.AreEqual(604800, callback.Request.SOA.Expire);
			Assert.AreEqual(1800, callback.Request.SOA.MinimumTtl);
			Assert.AreEqual("ns1.example.org", callback.Request.SOA.PrimaryNameserver);
			Assert.AreEqual(28800, callback.Request.SOA.Refresh);
			Assert.AreEqual(3600, callback.Request.SOA.Retry);
			Assert.AreEqual(43200, callback.Request.SOA.TimeToLive);
			Assert.AreEqual("admin.example.org", callback.Request.SOA.ZoneAdministrator);
			Assert.AreEqual(DnsZoneMode.Standard, callback.Request.ZoneMode);

			_clientMock.Verify(m => m.PatchAsync<DnsZoneSettings, InternalUpdateDnsZoneSettingsRequest>($"/zones/{ZoneId}/dns_settings", It.IsAny<InternalUpdateDnsZoneSettingsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldUpdateDnsSettingsNone()
		{
			// Arrange
			var request = new UpdateDnsZoneSettingsRequest(ZoneId);
			var client = GetClient();

			// Act
			var response = await client.UpdateDnsZoneSettings(request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_settings", callback.RequestPath);

			Assert.IsNotNull(callback.Request);
			Assert.IsNull(callback.Request.FlattenAllCnames);
			Assert.IsNull(callback.Request.FoundationDns);
			Assert.IsNull(callback.Request.MultiProvider);
			Assert.IsNull(callback.Request.Nameservers);
			Assert.IsNull(callback.Request.NameserverTtl);
			Assert.IsNull(callback.Request.SecondaryOverrides);
			Assert.IsNull(callback.Request.SOA);
			Assert.IsNull(callback.Request.ZoneMode);

			_clientMock.Verify(m => m.PatchAsync<DnsZoneSettings, InternalUpdateDnsZoneSettingsRequest>($"/zones/{ZoneId}/dns_settings", It.IsAny<InternalUpdateDnsZoneSettingsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidMode()
		{
			// Arrange
			_request.ZoneMode = 0;
			var client = GetClient();

			// Act
			await client.UpdateDnsZoneSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidNameserverType()
		{
			// Arrange
			_request.Nameservers.Type = 0;
			var client = GetClient();

			// Act
			await client.UpdateDnsZoneSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(29)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidNameserverTtl(int ttl)
		{
			// Arrange
			_request.NameserverTtl = ttl;
			var client = GetClient();

			// Act
			await client.UpdateDnsZoneSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(86399)]
		[DataRow(2419201)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaExpire(int ttl)
		{
			// Arrange
			_request.SOA.Expire = ttl;
			var client = GetClient();

			// Act
			await client.UpdateDnsZoneSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(59)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaMinimumTtl(int ttl)
		{
			// Arrange
			_request.SOA.MinimumTtl = ttl;
			var client = GetClient();

			// Act
			await client.UpdateDnsZoneSettings(_request);

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
			_request.SOA.PrimaryNameserver = nameserver;
			var client = GetClient();

			// Act
			await client.UpdateDnsZoneSettings(_request);

			// Assert - ArgumentNullException
		}

		[DataTestMethod]
		[DataRow(599)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaRefresh(int ttl)
		{
			// Arrange
			_request.SOA.Refresh = ttl;
			var client = GetClient();

			// Act
			await client.UpdateDnsZoneSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(599)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaRetry(int ttl)
		{
			// Arrange
			_request.SOA.Retry = ttl;
			var client = GetClient();

			// Act
			await client.UpdateDnsZoneSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[DataTestMethod]
		[DataRow(299)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaTtl(int ttl)
		{
			// Arrange
			_request.SOA.TimeToLive = ttl;
			var client = GetClient();

			// Act
			await client.UpdateDnsZoneSettings(_request);

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
			_request.SOA.ZoneAdministrator = admin;
			var client = GetClient();

			// Act
			await client.UpdateDnsZoneSettings(_request);

			// Assert - ArgumentNullException
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PatchAsync<DnsZoneSettings, InternalUpdateDnsZoneSettingsRequest>(It.IsAny<string>(), It.IsAny<InternalUpdateDnsZoneSettingsRequest>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalUpdateDnsZoneSettingsRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
