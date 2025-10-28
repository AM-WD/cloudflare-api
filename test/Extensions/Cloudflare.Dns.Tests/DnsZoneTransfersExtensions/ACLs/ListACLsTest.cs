using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.ACLs
{
	[TestClass]
	public class ListACLsTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<IReadOnlyCollection<ACL>> _response;
		private List<string> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = new List<string>();

			_response = new CloudflareResponse<IReadOnlyCollection<ACL>>
			{
				Success = true,
				Messages = new List<ResponseInfo> { new ResponseInfo(1000, "Message 1") },
				Errors = new List<ResponseInfo> { new ResponseInfo(1000, "Error 1") },
				Result = new List<ACL>
				{
					new ACL
					{
						Id = "23ff594956f20c2a721606e94745a8aa",
						IpRange = "192.0.2.0/24",
						Name = "Test ACL"
					}
				}
			};
		}

		[TestMethod]
		public async Task ShouldListAcls()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ListACLs(AccountId, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			string requestPath = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/secondary_dns/acls", requestPath);

			_clientMock.Verify(m => m.GetAsync<IReadOnlyCollection<ACL>>($"/accounts/{AccountId}/secondary_dns/acls", null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<IReadOnlyCollection<ACL>>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, _, _) => _callbacks.Add(requestPath))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
