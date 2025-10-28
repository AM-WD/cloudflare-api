using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.ACLs
{
	[TestClass]
	public class UpdateACLTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string AclId = "23ff594956f20c2a721606e94745a8aa";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<ACL> _response;
		private List<(string RequestPath, InternalDnsZoneTransferAclRequest Request)> _callbacks;
		private UpdateDnsZoneTransferAclRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<ACL>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result = new ACL
				{
					Id = AclId,
					IpRange = "192.0.2.53/28",
					Name = "my-acl-1"
				}
			};

			_request = new UpdateDnsZoneTransferAclRequest(
				accountId: AccountId,
				aclId: AclId,
				ipRange: "192.0.2.53/28",
				name: "my-acl-1"
			);
		}

		[TestMethod]
		public async Task ShouldUpdateAcl()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.UpdateACL(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/secondary_dns/acls/{AclId}", requestPath);

			Assert.IsNotNull(request);
			Assert.AreEqual(_request.Name, request.Name);
			Assert.AreEqual(_request.IpRange, request.IpRange);

			Assert.AreEqual("192.0.2.53", _request.IpRangeBaseAddress.ToString());
			Assert.AreEqual(28, _request.IpRangeSubnet);

			_clientMock.Verify(m => m.PutAsync<ACL, InternalDnsZoneTransferAclRequest>($"/accounts/{AccountId}/secondary_dns/acls/{AclId}", It.IsAny<InternalDnsZoneTransferAclRequest>(), TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		[DataRow("127.0.0.1/20")]
		[DataRow("fd00::/56")]
		public async Task ShouldThrowArumentOutOfRangeExceptionForWrongSubnetDefinition(string ipRange)
		{
			// Arrange
			_request.IpRange = ipRange;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.UpdateACL(_request, TestContext.CancellationToken);
			});
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PutAsync<ACL, InternalDnsZoneTransferAclRequest>(It.IsAny<string>(), It.IsAny<InternalDnsZoneTransferAclRequest>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalDnsZoneTransferAclRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
