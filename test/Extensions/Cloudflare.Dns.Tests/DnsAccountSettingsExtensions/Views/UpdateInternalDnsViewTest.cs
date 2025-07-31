using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using AMWD.Net.Api.Cloudflare.Dns.Internals;
using Moq;

namespace Cloudflare.Dns.Tests.DnsAccountSettingsExtensions.Views
{
	[TestClass]
	public class UpdateInternalDnsViewTest
	{
		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string ViewId = "023e105f4ecef8ad9ca31a8372d0c354";
		private const string ViewName = "InternalView";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<InternalDnsView> _response;
		private List<(string RequestPath, InternalModifyInternalDnsViewRequest Request)> _callbacks;
		private UpdateInternalDnsViewRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<InternalDnsView>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = new InternalDnsView(ViewId, ViewName)
			};

			_request = new UpdateInternalDnsViewRequest(AccountId, ViewId, ViewName)
			{
				ZoneIds = ["zone1", "zone2"]
			};
		}

		[TestMethod]
		public async Task ShouldUpdateInternalDnsView()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.UpdateInternalDnsView(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_settings/views/{ViewId}", callback.RequestPath);
			Assert.IsNotNull(callback.Request);

			Assert.AreEqual(ViewName, callback.Request.Name);
			CollectionAssert.AreEqual(_request.ZoneIds.ToList(), callback.Request.Zones.ToList());

			_clientMock.Verify(m => m.PatchAsync<InternalDnsView, InternalModifyInternalDnsViewRequest>(
				$"/accounts/{AccountId}/dns_settings/views/{ViewId}",
				It.IsAny<InternalModifyInternalDnsViewRequest>(),
				It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[DataTestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task ShouldThrowArgumentNullExceptionWhenNameIsNull(string name)
		{
			// Arrange
			_request.Name = name;
			var client = GetClient();

			// Act
			var response = await client.UpdateInternalDnsView(_request);

			// Assert - ArgumentNullException
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public async Task ShouldThrowArgumentOutOfRangeExceptionWhenNameTooLong()
		{
			// Arrange
			_request.Name = new string('a', 256);
			var client = GetClient();

			// Act
			var response = await client.UpdateInternalDnsView(_request);

			// Assert - ArgumentOutOfRangeException
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PatchAsync<InternalDnsView, InternalModifyInternalDnsViewRequest>(
					It.IsAny<string>(),
					It.IsAny<InternalModifyInternalDnsViewRequest>(),
					It.IsAny<CancellationToken>()))
				.Callback<string, InternalModifyInternalDnsViewRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
