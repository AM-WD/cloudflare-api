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
	public class EditZoneTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<Zone> _response;

		private List<(string RequestPath, InternalEditZoneRequest Request)> _callbacks;

		private EditZoneRequest _request;

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

			_request = new EditZoneRequest(ZoneId)
			{
				Paused = true,
				Type = ZoneType.Full,
				VanityNameServers = ["ns1.example.org", "ns2.example.org"]
			};
		}

		[TestMethod]
		public async Task ShouldReturnModifiedZoneForPaused()
		{
			// Arrange
			_request.Type = null;
			_request.VanityNameServers = null;
			var client = GetClient();

			// Act
			var response = await client.EditZone(_request, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}", requestPath);
			Assert.IsNotNull(request);

			Assert.IsTrue(request.Paused);
			Assert.IsNull(request.Type);
			Assert.IsNull(request.VanityNameServers);

			_clientMock.Verify(m => m.PatchAsync<Zone, InternalEditZoneRequest>($"/zones/{ZoneId}", It.IsAny<InternalEditZoneRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldReturnModifiedZoneForType()
		{
			// Arrange
			_request.Paused = null;
			_request.VanityNameServers = null;
			var client = GetClient();

			// Act
			var response = await client.EditZone(_request, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}", requestPath);
			Assert.IsNotNull(request);

			Assert.IsNull(request.Paused);
			Assert.AreEqual(_request.Type.Value, request.Type.Value);
			Assert.IsNull(request.VanityNameServers);

			_clientMock.Verify(m => m.PatchAsync<Zone, InternalEditZoneRequest>($"/zones/{ZoneId}", It.IsAny<InternalEditZoneRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldReturnModifiedZoneForVanityNameServers()
		{
			// Arrange
			_request.Paused = null;
			_request.Type = null;
			_request.VanityNameServers = [.. _request.VanityNameServers, ""];
			var client = GetClient();

			// Act
			var response = await client.EditZone(_request, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}", requestPath);
			Assert.IsNotNull(request);

			Assert.IsNull(request.Paused);
			Assert.IsNull(request.Type);
			Assert.HasCount(2, request.VanityNameServers);
			Assert.IsTrue(request.VanityNameServers.Contains("ns1.example.org"));
			Assert.IsTrue(request.VanityNameServers.Contains("ns2.example.org"));

			_clientMock.Verify(m => m.PatchAsync<Zone, InternalEditZoneRequest>($"/zones/{ZoneId}", It.IsAny<InternalEditZoneRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldThrowCloudflareExceptionOnMultiplePropertiesSet1()
		{
			// Arrange
			_request.VanityNameServers = null;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<CloudflareException>(async () => await client.EditZone(_request, TestContext.CancellationTokenSource.Token));
		}

		[TestMethod]
		public async Task ShouldThrowCloudflareExceptionOnMultiplePropertiesSet2()
		{
			// Arrange
			_request.Paused = null;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<CloudflareException>(async () => await client.EditZone(_request, TestContext.CancellationTokenSource.Token));
		}

		[TestMethod]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidType()
		{
			// Arrange
			_request.Paused = null;
			_request.Type = 0;
			_request.VanityNameServers = null;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () => await client.EditZone(_request, TestContext.CancellationTokenSource.Token));
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PatchAsync<Zone, InternalEditZoneRequest>(It.IsAny<string>(), It.IsAny<InternalEditZoneRequest>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalEditZoneRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
