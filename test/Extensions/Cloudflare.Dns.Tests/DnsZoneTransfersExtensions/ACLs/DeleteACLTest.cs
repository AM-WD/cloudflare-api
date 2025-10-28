using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsZoneTransfersExtensions.ACLs
{
	[TestClass]
	public class DeleteACLTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string AclId = "23ff594956f20c2a721606e94745a8aa";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<Identifier> _response;
		private List<string> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = new List<string>();

			_response = new CloudflareResponse<Identifier>
			{
				Success = true,
				Messages = [new ResponseInfo(1000, "Message 1")],
				Errors = [new ResponseInfo(1000, "Error 1")],
				Result = new Identifier
				{
					Id = AclId
				}
			};
		}

		[TestMethod]
		public async Task ShouldDeleteAcl()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.DeleteACL(AccountId, AclId, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			string requestPath = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/secondary_dns/acls/{AclId}", requestPath);

			_clientMock.Verify(m => m.DeleteAsync<Identifier>($"/accounts/{AccountId}/secondary_dns/acls/{AclId}", null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.DeleteAsync<Identifier>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, _, _) => _callbacks.Add(requestPath))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
