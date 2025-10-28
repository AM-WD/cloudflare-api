using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.Outgoing
{
	[TestClass]
	public class DeletePrimaryZoneConfigurationTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

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
				Messages = new List<ResponseInfo> { new(1000, "Message 1") },
				Errors = [],
				Result = new Identifier
				{
					Id = "269d8f4853475ca241c4e730be286b20"
				}
			};
		}

		[TestMethod]
		public async Task ShouldDeletePrimaryZoneConfiguration()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.DeletePrimaryZoneConfiguration(ZoneId, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);
			Assert.AreEqual("269d8f4853475ca241c4e730be286b20", response.Result.Id);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/secondary_dns/outgoing", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.DeleteAsync<Identifier>($"/zones/{ZoneId}/secondary_dns/outgoing", null, TestContext.CancellationToken), Times.Once);
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
