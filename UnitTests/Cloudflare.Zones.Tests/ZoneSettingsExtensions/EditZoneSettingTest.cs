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
			var response = await client.EditZoneSetting(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/settings/ssl", callback.RequestPath);

			Assert.IsNotNull(callback.Request);
			Assert.AreEqual("origin_pull", callback.Request["value"]);
			Assert.IsFalse(callback.Request.ContainsKey("enabled"));
		}

		[DataTestMethod]
		[DataRow(true)]
		[DataRow(false)]
		public async Task ShouldEditEnabledState(bool enabled)
		{
			// Arrange
			_request.Enabled = enabled;
			var client = GetClient();

			// Act
			var response = await client.EditZoneSetting(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/settings/ssl", callback.RequestPath);

			Assert.IsNotNull(callback.Request);
			Assert.AreEqual(enabled, callback.Request["enabled"]);
			Assert.IsFalse(callback.Request.ContainsKey("value"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public async Task ShouldThrowArgumentException()
		{
			// Arrange
			var request = new EditZoneSettingRequest<TestSetting>(ZoneId, new TestSetting());
			var client = GetClient();

			// Act
			var response = await client.EditZoneSetting(request);

			// Assert - ArgumentException
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
