using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.ACLs
{
	[TestClass]
	public class CreateACLTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<ACL> _response;
		private List<(string RequestPath, InternalDnsZoneTransferAclRequest Request)> _callbacks;
		private CreateACLRequest _request;

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
					Id = "23ff594956f20c2a721606e94745a8aa",
					IpRange = "192.0.2.53/28",
					Name = "my-acl-1"
				}
			};

			_request = new CreateACLRequest(
				accountId: AccountId,
				ipRange: "192.0.2.53/28",
				name: "my-acl-1"
			);
		}

		[TestMethod]
		public async Task ShouldCreateAcl()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.CreateACL(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/secondary_dns/acls", requestPath);

			Assert.IsNotNull(request);
			Assert.AreEqual(_request.Name, request.Name);
			Assert.AreEqual(_request.IpRange, request.IpRange);

			Assert.AreEqual("192.0.2.53", _request.IpRangeBaseAddress.ToString());
			Assert.AreEqual(28, _request.IpRangeSubnet);

			_clientMock.Verify(m => m.PostAsync<ACL, InternalDnsZoneTransferAclRequest>($"/accounts/{AccountId}/secondary_dns/acls", It.IsAny<InternalDnsZoneTransferAclRequest>(), null, TestContext.CancellationToken), Times.Once);
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
				await client.CreateACL(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("\t")]
		[DataRow("   ")]
		public void ShouldThrowArgumentNullExceptionForIpRange(string ipRange)
		{
			// Arrange

			// Act & Assert
			Assert.ThrowsExactly<ArgumentNullException>(() =>
			{
				_ = new CreateACLRequest(AccountId, ipRange, "my-acl-1");
			});
		}

		[TestMethod]
		[DataRow("192.0.2.53")]
		[DataRow("192.0.2.53/28/28")]
		public void ShouldThrowFormatExceptionForInvalidFormat(string ipRange)
		{
			// Arrange

			// Act & Assert
			Assert.ThrowsExactly<FormatException>(() =>
			{
				_ = new CreateACLRequest(AccountId, ipRange, "my-acl-1");
			});
		}

		[TestMethod]
		public void ShouldThrowFormatExceptionForInvalidSubnetFormat()
		{
			// Arrange

			// Act & Assert
			Assert.ThrowsExactly<FormatException>(() =>
			{
				_ = new CreateACLRequest(AccountId, "192.0.2.53/a", "my-acl-1");
			});
		}

		[TestMethod]
		[DataRow("127.0.0.1/-1")]
		[DataRow("127.0.0.1/33")]
		[DataRow("fd00::/-1")]
		[DataRow("fd00::/129")]
		public void ShouldThrowFormatExceptionForInvalidSubnetLength(string ipRange)
		{
			// Arrange

			// Act & Assert
			Assert.ThrowsExactly<FormatException>(() =>
			{
				_ = new CreateACLRequest(AccountId, ipRange, "my-acl-1");
			});
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<ACL, InternalDnsZoneTransferAclRequest>(It.IsAny<string>(), It.IsAny<InternalDnsZoneTransferAclRequest>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalDnsZoneTransferAclRequest, IQueryParameterFilter, CancellationToken>((requestPath, request, _, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
