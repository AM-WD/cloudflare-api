using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using Moq;

namespace Cloudflare.Zones.Tests.Zones
{
	[TestClass]
	public class RerunActivationCheckTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<ZoneIdResponse> _response;

		private List<(string RequestPath, object Request)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<ZoneIdResponse>
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
				Result = new ZoneIdResponse
				{
					Id = ZoneId
				}
			};
		}

		[TestMethod]
		public async Task ShouldRerunActivationCheck()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.RerunActivationCheck(ZoneId);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"zones/{ZoneId}/activation_check", callback.RequestPath);
			Assert.IsNull(callback.Request);

			_clientMock.Verify(m => m.PutAsync<ZoneIdResponse, object>($"zones/{ZoneId}/activation_check", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PutAsync<ZoneIdResponse, object>(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
				.Callback<string, object, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
