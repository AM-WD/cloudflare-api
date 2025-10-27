using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsAccountSettingsExtensions
{
	[TestClass]
	public class ShowDnsAccountSettingsTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c354";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<DnsAccountSettings> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

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
							type: DnsAccountNameserversType.Random
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
		}

		[TestMethod]
		public async Task ShouldGetDnsSettings()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ShowDnsAccountSettings(AccountId, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_settings", requestPath);

			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<DnsAccountSettings>($"/accounts/{AccountId}/dns_settings", null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<DnsAccountSettings>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
