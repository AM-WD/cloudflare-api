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
			var response = await client.UpdateDnsAccountSettings(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_settings", callback.RequestPath);

			Assert.IsNotNull(callback.Request);
			Assert.IsTrue(callback.Request.ZoneDefaults.FlattenAllCnames);
			Assert.IsFalse(callback.Request.ZoneDefaults.FoundationDns);
			Assert.IsNotNull(callback.Request.ZoneDefaults.InternalDns);
			Assert.AreEqual(ZoneId, callback.Request.ZoneDefaults.InternalDns.ReferenceZoneId);
			Assert.IsFalse(callback.Request.ZoneDefaults.MultiProvider);
			Assert.IsNotNull(callback.Request.ZoneDefaults.Nameservers);
			Assert.AreEqual(DnsAccountNameserversType.Random, callback.Request.ZoneDefaults.Nameservers.Type);
			Assert.AreEqual(86400, callback.Request.ZoneDefaults.NameserverTtl);
			Assert.IsFalse(callback.Request.ZoneDefaults.SecondaryOverrides);
			Assert.IsNotNull(callback.Request.ZoneDefaults.SOA);
			Assert.AreEqual(604800, callback.Request.ZoneDefaults.SOA.Expire);
			Assert.AreEqual(1800, callback.Request.ZoneDefaults.SOA.MinimumTtl);
			Assert.AreEqual("ns1.example.org", callback.Request.ZoneDefaults.SOA.PrimaryNameserver);
			Assert.AreEqual(28800, callback.Request.ZoneDefaults.SOA.Refresh);
			Assert.AreEqual(3600, callback.Request.ZoneDefaults.SOA.Retry);
			Assert.AreEqual(43200, callback.Request.ZoneDefaults.SOA.TimeToLive);
			Assert.AreEqual("admin.example.org", callback.Request.ZoneDefaults.SOA.ZoneAdministrator);
			Assert.AreEqual(DnsZoneMode.Standard, callback.Request.ZoneDefaults.ZoneMode);

			_clientMock.Verify(m => m.PatchAsync<DnsAccountSettings, InternalUpdateDnsAccountSettingsRequest>($"/accounts/{AccountId}/dns_settings", It.IsAny<InternalUpdateDnsAccountSettingsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldUpdateDnsSettingsNone()
		{
			// Arrange
			var request = new UpdateDnsAccountSettingsRequest(AccountId);
			var client = GetClient();

			// Act
			var response = await client.UpdateDnsAccountSettings(request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_settings", callback.RequestPath);

			Assert.IsNotNull(callback.Request);
			Assert.IsNull(callback.Request.ZoneDefaults);

			_clientMock.Verify(m => m.PatchAsync<DnsAccountSettings, InternalUpdateDnsAccountSettingsRequest>($"/accounts/{AccountId}/dns_settings", It.IsAny<InternalUpdateDnsAccountSettingsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldUpdateDnsSettingsNoneDefaults()
		{
			// Arrange
			var request = new UpdateDnsAccountSettingsRequest(AccountId)
			{
				ZoneDefaults = new DnsAccountZoneDefaults()
			};
			var client = GetClient();

			// Act
			var response = await client.UpdateDnsAccountSettings(request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_settings", callback.RequestPath);

			Assert.IsNotNull(callback.Request);
			Assert.IsNotNull(callback.Request.ZoneDefaults);
			Assert.IsNull(callback.Request.ZoneDefaults.FlattenAllCnames);
			Assert.IsNull(callback.Request.ZoneDefaults.FoundationDns);
			Assert.IsNull(callback.Request.ZoneDefaults.MultiProvider);
			Assert.IsNull(callback.Request.ZoneDefaults.Nameservers);
			Assert.IsNull(callback.Request.ZoneDefaults.NameserverTtl);
			Assert.IsNull(callback.Request.ZoneDefaults.SecondaryOverrides);
			Assert.IsNull(callback.Request.ZoneDefaults.SOA);
			Assert.IsNull(callback.Request.ZoneDefaults.ZoneMode);

			_clientMock.Verify(m => m.PatchAsync<DnsAccountSettings, InternalUpdateDnsAccountSettingsRequest>($"/accounts/{AccountId}/dns_settings", It.IsAny<InternalUpdateDnsAccountSettingsRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidMode()
		{
			// Arrange
			_request.ZoneDefaults.ZoneMode = 0;
			var client = GetClient();

			// Act
			await client.UpdateDnsAccountSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidNameserverType()
		{
			// Arrange
			_request.ZoneDefaults.Nameservers.Type = 0;
			var client = GetClient();

			// Act
			await client.UpdateDnsAccountSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[TestMethod]
		[DataRow(29)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidNameserverTtl(int ttl)
		{
			// Arrange
			_request.ZoneDefaults.NameserverTtl = ttl;
			var client = GetClient();

			// Act
			await client.UpdateDnsAccountSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[TestMethod]
		[DataRow(86399)]
		[DataRow(2419201)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaExpire(int ttl)
		{
			// Arrange
			_request.ZoneDefaults.SOA.Expire = ttl;
			var client = GetClient();

			// Act
			await client.UpdateDnsAccountSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[TestMethod]
		[DataRow(59)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaMinimumTtl(int ttl)
		{
			// Arrange
			_request.ZoneDefaults.SOA.MinimumTtl = ttl;
			var client = GetClient();

			// Act
			await client.UpdateDnsAccountSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForMissingSoaNameserver(string nameserver)
		{
			// Arrange
			_request.ZoneDefaults.SOA.PrimaryNameserver = nameserver;
			var client = GetClient();

			// Act
			await client.UpdateDnsAccountSettings(_request);

			// Assert - ArgumentNullException
		}

		[TestMethod]
		[DataRow(599)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaRefresh(int ttl)
		{
			// Arrange
			_request.ZoneDefaults.SOA.Refresh = ttl;
			var client = GetClient();

			// Act
			await client.UpdateDnsAccountSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[TestMethod]
		[DataRow(599)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaRetry(int ttl)
		{
			// Arrange
			_request.ZoneDefaults.SOA.Retry = ttl;
			var client = GetClient();

			// Act
			await client.UpdateDnsAccountSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[TestMethod]
		[DataRow(299)]
		[DataRow(86401)]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidSoaTtl(int ttl)
		{
			// Arrange
			_request.ZoneDefaults.SOA.TimeToLive = ttl;
			var client = GetClient();

			// Act
			await client.UpdateDnsAccountSettings(_request);

			// Assert - ArgumentOutOfRangeException
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionForMissingSoaAdministrator(string admin)
		{
			// Arrange
			_request.ZoneDefaults.SOA.ZoneAdministrator = admin;
			var client = GetClient();

			// Act
			await client.UpdateDnsAccountSettings(_request);

			// Assert - ArgumentNullException
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
