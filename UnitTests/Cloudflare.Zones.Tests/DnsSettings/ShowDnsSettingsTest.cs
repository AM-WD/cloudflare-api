using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using Moq;

namespace Cloudflare.Zones.Tests.DnsSettings
{
	[TestClass]
	public class ShowDnsSettingsTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<ZoneDnsSetting> _response;

		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<ZoneDnsSetting>
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
				Result = new ZoneDnsSetting
				{
					FlattenAllCnames = false,
					FoundationDns = false,
					MultiProvider = false,
					Nameservers = new Nameserver
					{
						Type = NameserverType.CloudflareStandard,
					},
					NameserverTtl = 86400,
					SecondaryOverrides = false,
					SOA = new StartOfAuthority
					(
						expire: 604800,
						minimumTtl: 1800,
						primaryNameserver: "kristina.ns.cloudflare.com",
						refresh: 10000,
						retry: 2400,
						zoneAdministrator: "admin.example.com",
						ttl: 3600
					),
					Mode = ZoneMode.DnsOnly
				}
			};
		}

		[TestMethod]
		public async Task ShouldGetDnsSetting()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ShowZoneDnsSettings(ZoneId);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response?.Result, response.Result);

			Assert.AreEqual(1, _callbacks?.Count);

			var callback = _callbacks?.First();
			Assert.AreEqual($"zones/{ZoneId}/dns_settings", callback?.RequestPath);

			Assert.IsNull(callback?.QueryFilter);

			_clientMock?.Verify(m => m.GetAsync<ZoneDnsSetting>($"zones/{ZoneId}/dns_settings", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock?.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			if (_response == null) throw new InvalidOperationException();

			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<ZoneDnsSetting>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks?.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
