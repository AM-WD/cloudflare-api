using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsFirewallExtensions
{
	[TestClass]
	public class ListDNSFirewallClustersTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<IReadOnlyCollection<DnsFirewallCluster>> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = new List<(string, IQueryParameterFilter)>();

			_response = new CloudflareResponse<IReadOnlyCollection<DnsFirewallCluster>>
			{
				Success = true,
				Messages =
				[
					new ResponseInfo(1000, "Message 1")
				],
				Errors =
				[
					new ResponseInfo(1000, "Error 1")
				],
				ResultInfo = new PaginationInfo
				{
					Count = 1,
					Page = 1,
					PerPage = 20,
					TotalCount = 2000,
					TotalPages = 100,
				},
				Result =
				[
					new DnsFirewallCluster
					{
						Id = "cluster-1",
						Name = "example-cluster",
						ModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
						DeprecateAnyRequests = true,
						MaximumCacheTtl = 3600,
						MinimumCacheTtl = 60,
						RateLimit = 1000,
						Retries = 2,
						DnsFirewallIps = ["10.0.0.1"],
						UpstreamIps = ["192.0.2.1"],
						AttackMitigation = new AttackMitigation { Enabled = true, OnlyWhenUpstreamUnhealthy = false }
					}
				]
			};
		}

		[TestMethod]
		public async Task ShouldListDNSFirewallClusters()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ListDNSFirewallClusters(AccountId, cancellationToken: TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			Assert.HasCount(1, response.Result);
			Assert.AreEqual("cluster-1", response.Result.First().Id);
			Assert.AreEqual("example-cluster", response.Result.First().Name);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_firewall", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<IReadOnlyCollection<DnsFirewallCluster>>($"/accounts/{AccountId}/dns_firewall", null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldListDNSFirewallClustersWithFilter()
		{
			// Arrange
			var filter = new ListDNSFirewallClustersFilter
			{
				Page = 2,
				PerPage = 10
			};

			var client = GetClient();

			// Act
			var response = await client.ListDNSFirewallClusters(AccountId, filter, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			Assert.HasCount(1, response.Result);
			Assert.AreEqual("cluster-1", response.Result.First().Id);
			Assert.AreEqual("example-cluster", response.Result.First().Name);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_firewall", requestPath);
			Assert.IsNotNull(queryFilter);

			Assert.IsInstanceOfType<ListDNSFirewallClustersFilter>(queryFilter);
			Assert.AreEqual(2, ((ListDNSFirewallClustersFilter)queryFilter).Page);
			Assert.AreEqual(10, ((ListDNSFirewallClustersFilter)queryFilter).PerPage);

			_clientMock.Verify(m => m.GetAsync<IReadOnlyCollection<DnsFirewallCluster>>($"/accounts/{AccountId}/dns_firewall", filter, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public void ShouldReturnEmptyParameterList()
		{
			// Arrange
			var filter = new ListDNSFirewallClustersFilter();

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		public void ShouldReturnFullParameterList()
		{
			// Arrange
			var filter = new ListDNSFirewallClustersFilter
			{
				Page = 2,
				PerPage = 20
			};

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.HasCount(2, dict);

			Assert.IsTrue(dict.ContainsKey("page"));
			Assert.IsTrue(dict.ContainsKey("per_page"));

			Assert.AreEqual("2", dict["page"]);
			Assert.AreEqual("20", dict["per_page"]);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow(0)]
		public void ShouldNotAddPage(int? page)
		{
			// Arrange
			var filter = new ListDNSFirewallClustersFilter { Page = page };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow(0)]
		[DataRow(101)]
		public void ShouldNotAddPerPage(int? perPage)
		{
			// Arrange
			var filter = new ListDNSFirewallClustersFilter { PerPage = perPage };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<IReadOnlyCollection<DnsFirewallCluster>>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
