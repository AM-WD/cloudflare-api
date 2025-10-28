using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.ACLs
{
	[TestClass]
	public class ACLDetailsTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string AclId = "23ff594956f20c2a721606e94745a8aa";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<ACL> _response;
		private List<string> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<ACL>
			{
				Success = true,
				Messages = [new ResponseInfo(1000, "Message 1")],
				Errors = [new ResponseInfo(1000, "Error 1")],
				Result = new ACL
				{
					Id = AclId,
					IpRange = "192.0.2.0/24",
					Name = "Test ACL"
				}
			};
		}

		[TestMethod]
		public async Task ShouldGetAclDetails()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ACLDetails(AccountId, AclId, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			string requestPath = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/secondary_dns/acls/{AclId}", requestPath);

			_clientMock.Verify(m => m.GetAsync<ACL>($"/accounts/{AccountId}/secondary_dns/acls/{AclId}", null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<ACL>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, _, _) => _callbacks.Add(requestPath))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
