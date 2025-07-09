using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using Moq;

namespace Cloudflare.Zones.Tests.ZoneSettingsExtensions
{
	[TestClass]
	public class EditMultipleZoneSettingsTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<IReadOnlyCollection<ZoneSettingBase>> _response;

		private List<(string RequestPath, IReadOnlyCollection<ZoneSettingBase> Request)> _callbacks;

		private EditMultipleZoneSettingsRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<IReadOnlyCollection<ZoneSettingBase>>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = [
					new SSL { Value = SslMode.Flexible },
					new WebP { Value = OnOffState.Off }
				]
			};

			_request = new EditMultipleZoneSettingsRequest(ZoneId)
			{
				Settings = [
					new SSL { Value = SslMode.Strict },
					new WebP { Value = OnOffState.On }
				]
			};
		}

		[TestMethod]
		public async Task ShouldEditMultipleZoneSettings()
		{
			// Arrange
			var client = GetClient();

			// Act
#pragma warning disable CS0618
			var response = await client.EditMultipleZoneSettings(_request);
#pragma warning restore CS0618

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/settings", callback.RequestPath);

			Assert.IsNotNull(callback.Request);
			Assert.AreEqual(2, callback.Request.Count);

			Assert.IsInstanceOfType<SSL>(callback.Request.First());
			Assert.IsInstanceOfType<WebP>(callback.Request.Last());
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PatchAsync<IReadOnlyCollection<ZoneSettingBase>, IReadOnlyCollection<ZoneSettingBase>>(It.IsAny<string>(), It.IsAny<IReadOnlyCollection<ZoneSettingBase>>(), It.IsAny<CancellationToken>()))
				.Callback<string, IReadOnlyCollection<ZoneSettingBase>, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
