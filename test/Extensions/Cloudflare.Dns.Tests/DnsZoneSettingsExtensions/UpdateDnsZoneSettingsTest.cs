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
		public TestContext TestContext { get; set; }

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
			var response = await client.UpdateDnsZoneSettings(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_settings", requestPath);

			Assert.IsNotNull(request);
			Assert.IsTrue(request.FlattenAllCnames);
			Assert.IsFalse(request.FoundationDns);
			Assert.IsNotNull(request.InternalDns);
			Assert.AreEqual(ZoneId, request.InternalDns.ReferenceZoneId);
			Assert.IsFalse(request.MultiProvider);
			Assert.IsNotNull(request.Nameservers);
			Assert.AreEqual(DnsZoneNameserversType.Standard, request.Nameservers.Type);
			Assert.AreEqual(86400, request.NameserverTtl);
			Assert.IsFalse(request.SecondaryOverrides);
			Assert.IsNotNull(request.SOA);
			Assert.AreEqual(604800, request.SOA.Expire);
			Assert.AreEqual(1800, request.SOA.MinimumTtl);
			Assert.AreEqual("ns1.example.org", request.SOA.PrimaryNameserver);
			Assert.AreEqual(28800, request.SOA.Refresh);
			Assert.AreEqual(3600, request.SOA.Retry);
			Assert.AreEqual(43200, request.SOA.TimeToLive);
			Assert.AreEqual("admin.example.org", request.SOA.ZoneAdministrator);
			Assert.AreEqual(DnsZoneMode.Standard, request.ZoneMode);

			_clientMock.Verify(m => m.PatchAsync<DnsZoneSettings, InternalUpdateDnsZoneSettingsRequest>($"/zones/{ZoneId}/dns_settings", It.IsAny<InternalUpdateDnsZoneSettingsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldUpdateDnsSettingsNone()
		{
			// Arrange
			var req = new UpdateDnsZoneSettingsRequest(ZoneId);
			var client = GetClient();

			// Act
			var response = await client.UpdateDnsZoneSettings(req, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_settings", requestPath);

			Assert.IsNotNull(request);
			Assert.IsNull(request.FlattenAllCnames);
			Assert.IsNull(request.FoundationDns);
			Assert.IsNull(request.MultiProvider);
			Assert.IsNull(request.Nameservers);
			Assert.IsNull(request.NameserverTtl);
			Assert.IsNull(request.SecondaryOverrides);
			Assert.IsNull(request.SOA);
			Assert.IsNull(request.ZoneMode);

			_clientMock.Verify(m => m.PatchAsync<DnsZoneSettings, InternalUpdateDnsZoneSettingsRequest>($"/zones/{ZoneId}/dns_settings", It.IsAny<InternalUpdateDnsZoneSettingsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidMode()
		{
			// Arrange
			_request.ZoneMode = 0;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsZoneSettings(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidNameserverType()
		{
			// Arrange
			_request.Nameservers.Type = 0;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsZoneSettings(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		[DataRow(29)]
		[DataRow(86401)]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidNameserverTtl(int ttl)
		{
			// Arrange
			_request.NameserverTtl = ttl;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsZoneSettings(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		[DataRow(86399)]
		[DataRow(2419201)]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaExpire(int ttl)
		{
			// Arrange
			_request.SOA.Expire = ttl;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsZoneSettings(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		[DataRow(59)]
		[DataRow(86401)]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaMinimumTtl(int ttl)
		{
			// Arrange
			_request.SOA.MinimumTtl = ttl;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsZoneSettings(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public async Task ShouldThrowArgumentNullExceptionForMissingSoaNameserver(string nameserver)
		{
			// Arrange
			_request.SOA.PrimaryNameserver = nameserver;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () =>
			{
				await client.UpdateDnsZoneSettings(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		[DataRow(599)]
		[DataRow(86401)]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaRefresh(int ttl)
		{
			// Arrange
			_request.SOA.Refresh = ttl;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsZoneSettings(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		[DataRow(599)]
		[DataRow(86401)]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaRetry(int ttl)
		{
			// Arrange
			_request.SOA.Retry = ttl;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsZoneSettings(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		[DataRow(299)]
		[DataRow(86401)]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaTtl(int ttl)
		{
			// Arrange
			_request.SOA.TimeToLive = ttl;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsZoneSettings(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public async Task ShouldThrowArgumentNullExceptionForMissingSoaAdministrator(string admin)
		{
			// Arrange
			_request.SOA.ZoneAdministrator = admin;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () =>
			{
				await client.UpdateDnsZoneSettings(_request, TestContext.CancellationToken);
			});
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
