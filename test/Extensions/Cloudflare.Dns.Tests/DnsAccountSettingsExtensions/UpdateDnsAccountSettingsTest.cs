using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using AMWD.Net.Api.Cloudflare.Dns.Internals;
using Moq;

namespace Cloudflare.Dns.Tests.DnsAccountSettingsExtensions
{
	[TestClass]
	public class UpdateDnsAccountSettingsTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c354";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<DnsAccountSettings> _response;
		private List<(string RequestPath, InternalUpdateDnsAccountSettingsRequest Request)> _callbacks;
		private UpdateDnsAccountSettingsRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<DnsAccountSettings>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = new DnsAccountSettings
				{
					ZoneDefaults = new DnsAccountZoneDefaults
					{
						FlattenAllCnames = true,
						FoundationDns = false,
						InternalDns = new DnsAccountInternalDns
						{
							ReferenceZoneId = ZoneId
						},
						MultiProvider = false,
						Nameservers = new DnsAccountNameservers(
							type: DnsAccountNameserversType.Standard
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
				}
			};

			_request = new UpdateDnsAccountSettingsRequest(AccountId)
			{
				ZoneDefaults = new DnsAccountZoneDefaults
				{
					FlattenAllCnames = true,
					FoundationDns = false,
					InternalDns = new DnsAccountInternalDns
					{
						ReferenceZoneId = ZoneId
					},
					MultiProvider = false,
					Nameservers = new DnsAccountNameservers(
						type: DnsAccountNameserversType.Random
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
				}
			};
		}

		[TestMethod]
		public async Task ShouldUpdateDnsSettingsFull()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.UpdateDnsAccountSettings(_request, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_settings", requestPath);

			Assert.IsNotNull(request);
			Assert.IsTrue(request.ZoneDefaults.FlattenAllCnames);
			Assert.IsFalse(request.ZoneDefaults.FoundationDns);
			Assert.IsNotNull(request.ZoneDefaults.InternalDns);
			Assert.AreEqual(ZoneId, request.ZoneDefaults.InternalDns.ReferenceZoneId);
			Assert.IsFalse(request.ZoneDefaults.MultiProvider);
			Assert.IsNotNull(request.ZoneDefaults.Nameservers);
			Assert.AreEqual(DnsAccountNameserversType.Random, request.ZoneDefaults.Nameservers.Type);
			Assert.AreEqual(86400, request.ZoneDefaults.NameserverTtl);
			Assert.IsFalse(request.ZoneDefaults.SecondaryOverrides);
			Assert.IsNotNull(request.ZoneDefaults.SOA);
			Assert.AreEqual(604800, request.ZoneDefaults.SOA.Expire);
			Assert.AreEqual(1800, request.ZoneDefaults.SOA.MinimumTtl);
			Assert.AreEqual("ns1.example.org", request.ZoneDefaults.SOA.PrimaryNameserver);
			Assert.AreEqual(28800, request.ZoneDefaults.SOA.Refresh);
			Assert.AreEqual(3600, request.ZoneDefaults.SOA.Retry);
			Assert.AreEqual(43200, request.ZoneDefaults.SOA.TimeToLive);
			Assert.AreEqual("admin.example.org", request.ZoneDefaults.SOA.ZoneAdministrator);
			Assert.AreEqual(DnsZoneMode.Standard, request.ZoneDefaults.ZoneMode);

			_clientMock.Verify(m => m.PatchAsync<DnsAccountSettings, InternalUpdateDnsAccountSettingsRequest>($"/accounts/{AccountId}/dns_settings", It.IsAny<InternalUpdateDnsAccountSettingsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldUpdateDnsSettingsNone()
		{
			// Arrange
			var req = new UpdateDnsAccountSettingsRequest(AccountId);
			var client = GetClient();

			// Act
			var response = await client.UpdateDnsAccountSettings(req, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_settings", requestPath);

			Assert.IsNotNull(request);
			Assert.IsNull(request.ZoneDefaults);

			_clientMock.Verify(m => m.PatchAsync<DnsAccountSettings, InternalUpdateDnsAccountSettingsRequest>($"/accounts/{AccountId}/dns_settings", It.IsAny<InternalUpdateDnsAccountSettingsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldUpdateDnsSettingsNoneDefaults()
		{
			// Arrange
			var req = new UpdateDnsAccountSettingsRequest(AccountId)
			{
				ZoneDefaults = new DnsAccountZoneDefaults()
			};
			var client = GetClient();

			// Act
			var response = await client.UpdateDnsAccountSettings(req, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_settings", requestPath);

			Assert.IsNotNull(request);
			Assert.IsNotNull(request.ZoneDefaults);
			Assert.IsNull(request.ZoneDefaults.FlattenAllCnames);
			Assert.IsNull(request.ZoneDefaults.FoundationDns);
			Assert.IsNull(request.ZoneDefaults.MultiProvider);
			Assert.IsNull(request.ZoneDefaults.Nameservers);
			Assert.IsNull(request.ZoneDefaults.NameserverTtl);
			Assert.IsNull(request.ZoneDefaults.SecondaryOverrides);
			Assert.IsNull(request.ZoneDefaults.SOA);
			Assert.IsNull(request.ZoneDefaults.ZoneMode);

			_clientMock.Verify(m => m.PatchAsync<DnsAccountSettings, InternalUpdateDnsAccountSettingsRequest>($"/accounts/{AccountId}/dns_settings", It.IsAny<InternalUpdateDnsAccountSettingsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidMode()
		{
			// Arrange
			_request.ZoneDefaults.ZoneMode = 0;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsAccountSettings(_request, TestContext.CancellationTokenSource.Token);
			});
		}

		[TestMethod]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidNameserverType()
		{
			// Arrange
			_request.ZoneDefaults.Nameservers.Type = 0;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsAccountSettings(_request, TestContext.CancellationTokenSource.Token);
			});
		}

		[TestMethod]
		[DataRow(29)]
		[DataRow(86401)]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidNameserverTtl(int ttl)
		{
			// Arrange
			_request.ZoneDefaults.NameserverTtl = ttl;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsAccountSettings(_request, TestContext.CancellationTokenSource.Token);
			});
		}

		[TestMethod]
		[DataRow(86399)]
		[DataRow(2419201)]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaExpire(int ttl)
		{
			// Arrange
			_request.ZoneDefaults.SOA.Expire = ttl;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsAccountSettings(_request, TestContext.CancellationTokenSource.Token);
			});
		}

		[TestMethod]
		[DataRow(59)]
		[DataRow(86401)]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaMinimumTtl(int ttl)
		{
			// Arrange
			_request.ZoneDefaults.SOA.MinimumTtl = ttl;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsAccountSettings(_request, TestContext.CancellationTokenSource.Token);
			});
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public async Task ShouldThrowArgumentNullExceptionForMissingSoaNameserver(string nameserver)
		{
			// Arrange
			_request.ZoneDefaults.SOA.PrimaryNameserver = nameserver;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () =>
			{
				await client.UpdateDnsAccountSettings(_request, TestContext.CancellationTokenSource.Token);
			});
		}

		[TestMethod]
		[DataRow(599)]
		[DataRow(86401)]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaRefresh(int ttl)
		{
			// Arrange
			_request.ZoneDefaults.SOA.Refresh = ttl;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsAccountSettings(_request, TestContext.CancellationTokenSource.Token);
			});
		}

		[TestMethod]
		[DataRow(599)]
		[DataRow(86401)]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaRetry(int ttl)
		{
			// Arrange
			_request.ZoneDefaults.SOA.Retry = ttl;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsAccountSettings(_request, TestContext.CancellationTokenSource.Token);
			});
		}

		[TestMethod]
		[DataRow(299)]
		[DataRow(86401)]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaTtl(int ttl)
		{
			// Arrange
			_request.ZoneDefaults.SOA.TimeToLive = ttl;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateDnsAccountSettings(_request, TestContext.CancellationTokenSource.Token);
			});
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public async Task ShouldThrowArgumentNullExceptionForMissingSoaAdministrator(string admin)
		{
			// Arrange
			_request.ZoneDefaults.SOA.ZoneAdministrator = admin;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentNullException>(async () =>
			{
				await client.UpdateDnsAccountSettings(_request, TestContext.CancellationTokenSource.Token);
			});
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PatchAsync<DnsAccountSettings, InternalUpdateDnsAccountSettingsRequest>(It.IsAny<string>(), It.IsAny<InternalUpdateDnsAccountSettingsRequest>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalUpdateDnsAccountSettingsRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
