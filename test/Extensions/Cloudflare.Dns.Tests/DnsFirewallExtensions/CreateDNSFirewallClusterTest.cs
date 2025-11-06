using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using AMWD.Net.Api.Cloudflare.Dns.Internals;
using Moq;

namespace Cloudflare.Dns.Tests.DnsFirewallExtensions
{
	[TestClass]
	public class CreateDNSFirewallClusterTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string ClusterId = "023e105f4ecef8ad9ca31a8372d0c355";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<DnsFirewallCluster> _response;
		private List<(string RequestPath, InternalDNSFirewallClusterRequest Request)> _callbacks;
		private CreateDNSFirewallClusterRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<DnsFirewallCluster>
			{
				Success = true,
				Messages = [new ResponseInfo(1000, "Message 1")],
				Errors = [new ResponseInfo(1000, "Error 1")],
				Result = new DnsFirewallCluster
				{
					Id = ClusterId,
					Name = "example-cluster"
				}
			};

			_request = new CreateDNSFirewallClusterRequest(AccountId, "example-cluster")
			{
				UpstreamIps = ["192.0.2.1"],
				AttackMitigation = new AttackMitigation { Enabled = true, OnlyWhenUpstreamUnhealthy = false },
				DeprecateAnyRequests = true,
				EcsFallback = false,
				MaximumCacheTtl = 3600,
				MinimumCacheTtl = 60,
				NegativeCacheTtl = 120,
				RateLimit = 1000,
				Retries = 1
			};
		}

		[TestMethod]
		public async Task ShouldCreateDnsFirewallCluster()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.CreateDNSFirewallCluster(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_firewall", requestPath);

			Assert.IsNotNull(request);
			Assert.AreEqual(_request.Name, request.Name);
			CollectionAssert.AreEqual(_request.UpstreamIps?.ToList(), request.UpstreamIps?.ToList());
			Assert.IsNotNull(request.AttackMitigation);
			Assert.AreEqual(_request.AttackMitigation?.Enabled, request.AttackMitigation?.Enabled);
			Assert.AreEqual(_request.AttackMitigation?.OnlyWhenUpstreamUnhealthy, request.AttackMitigation?.OnlyWhenUpstreamUnhealthy);
			Assert.AreEqual(_request.DeprecateAnyRequests, request.DeprecateAnyRequests);
			Assert.AreEqual(_request.EcsFallback, request.EcsFallback);
			Assert.AreEqual(_request.MaximumCacheTtl, request.MaximumCacheTtl);
			Assert.AreEqual(_request.MinimumCacheTtl, request.MinimumCacheTtl);
			Assert.AreEqual(_request.NegativeCacheTtl, request.NegativeCacheTtl);
			Assert.AreEqual(_request.RateLimit, request.RateLimit);
			Assert.AreEqual(_request.Retries, request.Retries);

			_clientMock.Verify(m => m.PostAsync<DnsFirewallCluster, InternalDNSFirewallClusterRequest>($"/accounts/{AccountId}/dns_firewall", It.IsAny<InternalDNSFirewallClusterRequest>(), null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldCreateDnsFirewallClusterMinimalSet()
		{
			// Arrange
			_request = new CreateDNSFirewallClusterRequest(AccountId, "example-cluster");
			var client = GetClient();

			// Act
			var response = await client.CreateDNSFirewallCluster(_request, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, request) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_firewall", requestPath);

			Assert.IsNotNull(request);
			Assert.AreEqual(_request.Name, request.Name);
			Assert.IsNull(request.UpstreamIps);
			Assert.IsNull(request.AttackMitigation);
			Assert.IsNull(request.DeprecateAnyRequests);
			Assert.IsNull(request.EcsFallback);
			Assert.IsNull(request.MaximumCacheTtl);
			Assert.IsNull(request.MinimumCacheTtl);
			Assert.IsNull(request.NegativeCacheTtl);
			Assert.IsNull(request.RateLimit);
			Assert.IsNull(request.Retries);

			_clientMock.Verify(m => m.PostAsync<DnsFirewallCluster, InternalDNSFirewallClusterRequest>($"/accounts/{AccountId}/dns_firewall", It.IsAny<InternalDNSFirewallClusterRequest>(), null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public async Task ShouldThrowArgumentExceptionWhenNameIsNullOrWhitespace(string name)
		{
			// Arrange
			_request.Name = name;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentException>(async () =>
			{
				await client.CreateDNSFirewallCluster(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		public async Task ShouldThrowArgumentExceptionWhenNameTooLong()
		{
			// Arrange
			_request.Name = new string('a', 161);
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentException>(async () =>
			{
				await client.CreateDNSFirewallCluster(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		[DataRow(29)]
		[DataRow(36001)]
		public async Task ShouldThrowArgumentOutOfRangeForMaximumCacheTtl(int invalid)
		{
			// Arrange
			_request.MaximumCacheTtl = invalid;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.CreateDNSFirewallCluster(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		[DataRow(29)]
		[DataRow(36001)]
		public async Task ShouldThrowArgumentOutOfRangeForMinimumCacheTtl(int invalid)
		{
			// Arrange
			_request.MinimumCacheTtl = invalid;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.CreateDNSFirewallCluster(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		[DataRow(29)]
		[DataRow(36001)]
		public async Task ShouldThrowArgumentOutOfRangeForNegativeCacheTtl(int invalid)
		{
			// Arrange
			_request.NegativeCacheTtl = invalid;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.CreateDNSFirewallCluster(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		[DataRow(99)]
		[DataRow(1_000_000_001)]
		public async Task ShouldThrowArgumentOutOfRangeForRateLimit(int invalid)
		{
			// Arrange
			_request.RateLimit = invalid;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.CreateDNSFirewallCluster(_request, TestContext.CancellationToken);
			});
		}

		[TestMethod]
		[DataRow(-1)]
		[DataRow(3)]
		public async Task ShouldThrowArgumentOutOfRangeForRetries(int invalid)
		{
			// Arrange
			_request.Retries = invalid;
			var client = GetClient();

			// Act & Assert
			await Assert.ThrowsExactlyAsync<ArgumentOutOfRangeException>(async () =>
			{
				await client.CreateDNSFirewallCluster(_request, TestContext.CancellationToken);
			});
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.PostAsync<DnsFirewallCluster, InternalDNSFirewallClusterRequest>(It.IsAny<string>(), It.IsAny<InternalDNSFirewallClusterRequest>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalDNSFirewallClusterRequest, IQueryParameterFilter, CancellationToken>((requestPath, request, _, _) => _callbacks.Add((requestPath, request)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
