using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneSettingsExtensions
{
	[TestClass]
	public class ShowDnsZoneSettingsTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<DnsZoneSettings> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

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
		}

		[TestMethod]
		public async Task ShouldGetDnsSettings()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ShowDnsZoneSettings(ZoneId);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_settings", callback.RequestPath);

			Assert.IsNull(callback.QueryFilter);

			_clientMock?.Verify(m => m.GetAsync<DnsZoneSettings>($"/zones/{ZoneId}/dns_settings", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock?.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<DnsZoneSettings>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
