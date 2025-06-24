using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using Moq;

namespace Cloudflare.Zones.Tests.ZonesExtensions
{
	[TestClass]
	public class ZoneDetailsTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<Zone> _response;

		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<Zone>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = new Zone(
					ZoneId,
					"example.com",
					[
						"bob.ns.cloudflare.com",
						"lola.ns.cloudflare.com"
					],
					new ZoneAccount
					{
						Id = "023e105f4ecef8ad9ca31a8372d0c353",
						Name = "Example Account Name"
					},
					new ZoneMeta
					{
						CdnOnly = true,
						CustomCertificateQuota = 1,
						DnsOnly = true,
						FoundationDns = true,
						PageRuleQuota = 100,
						PhishingDetected = false,
						Step = 2
					},
					new ZoneOwner
					{
						Id = "023e105f4ecef8ad9ca31a8372d0c353",
						Name = "Example Org",
						Type = "organization"
					}
				)
				{
					ActivatedOn = DateTime.Parse("2014-01-02T00:01:00.12345Z"),
					CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
					DevelopmentMode = 7200,
					ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
					OriginalDnsHost = "NameCheap",
					OriginalNameServers =
					[
						"ns1.originaldnshost.com",
						"ns2.originaldnshost.com"
					],
					OriginalRegistrar = "GoDaddy",
					Paused = true,
					Status = ZoneStatus.Initializing,
					Type = ZoneType.Full,
					VanityNameServers =
					[
						"ns1.example.com",
						"ns2.example.com"
					]
				}
			};
		}

		[TestMethod]
		public async Task ShouldReturnZoneDetails()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ZoneDetails(ZoneId);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}", callback.RequestPath);
			Assert.IsNull(callback.QueryFilter);

			_clientMock.Verify(m => m.GetAsync<Zone>($"/zones/{ZoneId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<Zone>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
