using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsAccountSettingsExtensions.Views
{
	[TestClass]
	public class DeleteInternalDnsViewTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string ViewId = "023e105f4ecef8ad9ca31a8372d0c354";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<Identifier> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<Identifier>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = new Identifier
				{
					Id = ViewId
				}
			};
		}

		[TestMethod]
		public async Task ShouldDeleteInternalDnsView()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.DeleteInternalDnsView(AccountId, ViewId, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_settings/views/{ViewId}", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.DeleteAsync<Identifier>($"/accounts/{AccountId}/dns_settings/views/{ViewId}", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.DeleteAsync<Identifier>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
