using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using AMWD.Net.Api.Cloudflare.Zones.Internals;
using Moq;

namespace Cloudflare.Zones.Tests.ZonesExtensions
{
	[TestClass]
	public class CreateZoneTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<Zone> _response;

		private List<(string RequestPath, InternalCreateZoneRequest Request)> _callbacks;

		private CreateZoneRequest _request;

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
					Name = "example.com",
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

			_request = new CreateZoneRequest("example.com")
			{
				Type = ZoneType.Full
			};
		}

		[DataTestMethod]
		[DataRow(null, ZoneType.Full)]
		[DataRow("023e105f4ecef8ad9ca31a8372d0c353", null)]
		public async Task ShouldCreateZone(string accountId, ZoneType? type)
		{
			// Arrange
			_request.AccountId = accountId;
			_request.Type = type;
			var client = GetClient();

			// Act
			var response = await client.CreateZone(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual("/zones", callback.RequestPath);
			Assert.IsNotNull(callback.Request);

			Assert.AreEqual(_request.AccountId, callback.Request.Account.Id);
			Assert.AreEqual(_request.Name, callback.Request.Name);
			Assert.AreEqual(_request.Type, callback.Request.Type);

			_clientMock.Verify(m => m.PostAsync<Zone, InternalCreateZoneRequest>("/zones", It.IsAny<InternalCreateZoneRequest>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionOnInvalidType()
		{
			// Arrange
			_request.Type = 0;
			var client = GetClient();

			// Act
			await client.CreateZone(_request);

			// Assert - ArgumentOutOfRangeException
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<Zone, InternalCreateZoneRequest>(It.IsAny<string>(), It.IsAny<InternalCreateZoneRequest>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalCreateZoneRequest, IQueryParameterFilter, CancellationToken>((requestPath, request, _, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
