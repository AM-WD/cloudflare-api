using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using AMWD.Net.Api.Cloudflare.Zones.Zones.InternalRequests;
using Moq;

namespace Cloudflare.Zones.Tests.Zones
{
	[TestClass]
	public class EditZoneTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<Zone> _response;

		private List<(string RequestPath, EditRequest Request)> _callbacks;

		private EditZoneRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<Zone>
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
				Result = new Zone
				{
					Id = "023e105f4ecef8ad9ca31a8372d0c353",
					Account = new AccountBase
					{
						Id = "023e105f4ecef8ad9ca31a8372d0c353",
						Name = "Example Account Name"
					},
					ActivatedOn = DateTime.Parse("2014-01-02T00:01:00.12345Z"),
					CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
					DevelopmentMode = 7200,
					Meta = new ZoneMetaData
					{
						CdnOnly = true,
						CustomCertificateQuota = 1,
						DnsOnly = true,
						FoundationDns = true,
						PageRuleQuota = 100,
						PhishingDetected = false,
						Step = 2
					},
					ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
					Name = "example.com",
					NameServers =
					[
						"bob.ns.cloudflare.com",
						"lola.ns.cloudflare.com"
					],
					OriginalDnshost = "NameCheap",
					OriginalNameServers =
					[
						"ns1.originaldnshost.com",
						"ns2.originaldnshost.com"
					],
					OriginalRegistrar = "GoDaddy",
					Owner = new OwnerBase
					{
						Id = "023e105f4ecef8ad9ca31a8372d0c353",
						Name = "Example Org",
						Type = "organization"
					},
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

			_request = new EditZoneRequest
			{
				Id = ZoneId,
				Type = ZoneType.Full,
				VanityNameServers = ["ns1.example.org", "ns2.example.org"]
			};
		}

		[TestMethod]
		public async Task ShouldReturnModifiedZoneForType()
		{
			// Arrange
			_request.VanityNameServers = null;
			var client = GetClient();

			// Act
			var response = await client.EditZone(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"zones/{ZoneId}", callback.RequestPath);
			Assert.IsNotNull(callback.Request);

			Assert.AreEqual(_request.Type.Value, callback.Request.Type.Value);
			Assert.IsNull(callback.Request.VanityNameServers);

			_clientMock.Verify(m => m.PatchAsync<Zone, EditRequest>($"zones/{ZoneId}", It.IsAny<EditRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldReturnModifiedZoneForVanityNameServers()
		{
			// Arrange
			_request.Type = null;
			_request.VanityNameServers.Add("");
			var client = GetClient();

			// Act
			var response = await client.EditZone(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"zones/{ZoneId}", callback.RequestPath);
			Assert.IsNotNull(callback.Request);

			Assert.IsNull(callback.Request.Type);
			Assert.AreEqual(2, callback.Request.VanityNameServers.Count);
			Assert.IsTrue(callback.Request.VanityNameServers.Contains("ns1.example.org"));
			Assert.IsTrue(callback.Request.VanityNameServers.Contains("ns2.example.org"));

			_clientMock.Verify(m => m.PatchAsync<Zone, EditRequest>($"zones/{ZoneId}", It.IsAny<EditRequest>(), It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		[ExpectedException(typeof(CloudflareException))]
		public async Task ShouldThrowCloudflareExceptionOnMultiplePropertiesSet()
		{
			// Arrange
			var client = GetClient();

			// Act
			await client.EditZone(_request);

			// Assert - CloudflareException
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionForInvalidType()
		{
			// Arrange
			_request.Type = 0;
			_request.VanityNameServers = null;
			var client = GetClient();

			// Act
			await client.EditZone(_request);

			// Assert - ArgumentOutOfRangeException
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PatchAsync<Zone, EditRequest>(It.IsAny<string>(), It.IsAny<EditRequest>(), It.IsAny<CancellationToken>()))
				.Callback<string, EditRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
