using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using Moq;
using Newtonsoft.Json.Linq;

namespace Cloudflare.Zones.Tests.ZoneSettingsExtensions
{
	[TestClass]
	public class EditZoneSettingTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<SSL> _response;

		private List<(string RequestPath, JObject Request)> _callbacks;

		private EditZoneSettingRequest<SSL> _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<SSL>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = new SSL { Value = SslMode.Flexible }
			};

			_request = new EditZoneSettingRequest<SSL>(ZoneId, null)
			{
				Setting = new SSL { Value = SslMode.OriginPull }
			};
		}

		[TestMethod]
		public async Task ShouldEditZoneSetting()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.EditZoneSetting(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/settings/ssl", requestPath);

			Assert.IsNotNull(request);
			Assert.AreEqual("origin_pull", request["value"]);
			Assert.IsFalse(request.ContainsKey("enabled"));
		}

		[TestMethod]
		[DataRow(true)]
		[DataRow(false)]
		public async Task ShouldEditEnabledState(bool enabled)
		{
			// Arrange
			_request.Enabled = enabled;
			var client = GetClient();

			// Act
			var response = await client.EditZoneSetting(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/settings/ssl", requestPath);

			Assert.IsNotNull(request);
			Assert.AreEqual(enabled, request["enabled"]);
			Assert.IsFalse(request.ContainsKey("value"));
		}

		[TestMethod]
		public async Task ShouldThrowArgumentException()
		{
			// Arrange
			var request = new EditZoneSettingRequest<TestSetting>(ZoneId, new TestSetting());
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentException>(async () => await client.EditZoneSetting(request, TestContext.CancellationToken));
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PatchAsync<SSL, JObject>(It.IsAny<string>(), It.IsAny<JObject>(), It.IsAny<CancellationToken>()))
				.Callback<string, JObject, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}

		public class TestSetting : ZoneSettingBase
		{
		}
	}
}
