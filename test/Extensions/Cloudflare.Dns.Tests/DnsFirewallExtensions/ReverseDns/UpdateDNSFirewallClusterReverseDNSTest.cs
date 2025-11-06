using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using AMWD.Net.Api.Cloudflare.Dns.Internals;
using Moq;

namespace Cloudflare.Dns.Tests.DnsFirewallExtensions.ReverseDns
{
	[TestClass]
	public class UpdateDNSFirewallClusterReverseDNSTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string ClusterId = "023e105f4ecef8ad9ca31a8372d0c355";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<ReverseDnsResponse> _response;
		private List<(string RequestPath, InternalUpdateDNSFirewallClusterReverseDNSRequest Request)> _callbacks;
		private UpdateDNSFirewallClusterReverseDNSRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<ReverseDnsResponse>
			{
				Success = true,
				Messages = [new ResponseInfo(1000, "Message 1")],
				Errors = [new ResponseInfo(1000, "Error 1")],
				Result = new ReverseDnsResponse
				{
					ReverseDNS = new Dictionary<string, string>
					{
						["10.0.0.1"] = "ptr1.example.com"
					}
				}
			};

			_request = new UpdateDNSFirewallClusterReverseDNSRequest(AccountId, ClusterId)
			{
				ReverseDNS = new Dictionary<string, string>
				{
					["10.0.0.1"] = "ptr1.example.com"
				}
			};
		}

		[TestMethod]
		public async Task ShouldUpdateDnsFirewallClusterReverseDns()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.UpdateDNSFirewallClusterReverseDNS(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);
			Assert.IsTrue(response.Result.ReverseDNS?.ContainsKey("10.0.0.1") ?? false);
			Assert.AreEqual("ptr1.example.com", response.Result.ReverseDNS!["10.0.0.1"]);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_firewall/{ClusterId}/reverse_dns", requestPath);

			Assert.IsNotNull(request);
			Assert.IsNotNull(request.Ptr);
			Assert.IsTrue(request.Ptr.ContainsKey("10.0.0.1"));
			Assert.AreEqual("ptr1.example.com", request.Ptr["10.0.0.1"]);

			_clientMock.Verify(m => m.PatchAsync<ReverseDnsResponse, InternalUpdateDNSFirewallClusterReverseDNSRequest>($"/accounts/{AccountId}/dns_firewall/{ClusterId}/reverse_dns", It.IsAny<InternalUpdateDNSFirewallClusterReverseDNSRequest>(), TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldUpdateDnsFirewallClusterReverseDnsMinimalSet()
		{
			// Arrange
			_request = new UpdateDNSFirewallClusterReverseDNSRequest(AccountId, ClusterId);
			var client = GetClient();

			// Act
			var response = await client.UpdateDNSFirewallClusterReverseDNS(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_firewall/{ClusterId}/reverse_dns", requestPath);
			Assert.IsNull(request.Ptr);

			_clientMock.Verify(m => m.PatchAsync<ReverseDnsResponse, InternalUpdateDNSFirewallClusterReverseDNSRequest>($"/accounts/{AccountId}/dns_firewall/{ClusterId}/reverse_dns", It.IsAny<InternalUpdateDNSFirewallClusterReverseDNSRequest>(), TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PatchAsync<ReverseDnsResponse, InternalUpdateDNSFirewallClusterReverseDNSRequest>(It.IsAny<string>(), It.IsAny<InternalUpdateDNSFirewallClusterReverseDNSRequest>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalUpdateDNSFirewallClusterReverseDNSRequest, CancellationToken>((requestPath, request, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
