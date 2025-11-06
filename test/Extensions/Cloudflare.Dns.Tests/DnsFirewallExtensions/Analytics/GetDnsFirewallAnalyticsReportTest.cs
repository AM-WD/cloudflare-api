using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsFirewallExtensions.Analytics
{
	[TestClass]
	public class GetDnsFirewallAnalyticsReportTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";
		private const string ClusterId = "023e105f4ecef8ad9ca31a8372d0c355";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<DnsAnalyticsReport> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<DnsAnalyticsReport>
			{
				Success = true,
				Messages =
				[
					new ResponseInfo(1000, "Message 1")
				],
				Errors =
				[
					new ResponseInfo(1001, "Error 1")
				],
				ResultInfo = new PaginationInfo
				{
					Count = 1,
					Page = 1,
					PerPage = 100,
					TotalCount = 100,
					TotalPages = 1,
				},
				Result = new DnsAnalyticsReport()
			};
		}

		[TestMethod]
		public async Task ShouldGetDnsFirewallAnalyticsReport()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.GetDnsFirewallAnalyticsReport(AccountId, ClusterId, cancellationToken: TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			Assert.IsNotNull(response.Result);
			Assert.IsInstanceOfType<DnsAnalyticsReport>(response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_firewall/{ClusterId}/dns_analytics/report", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<DnsAnalyticsReport>($"/accounts/{AccountId}/dns_firewall/{ClusterId}/dns_analytics/report", null, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldGetDnsFirewallAnalyticsReportWithFilter()
		{
			// Arrange
			var filter = new GetDnsAnalyticsReportFilter
			{
				Since = DateTime.UtcNow.AddDays(-7),
				Until = DateTime.UtcNow,
				Dimensions = ["queryName", "responseCode"],
				Filters = "queryType eq A",
				Limit = 500,
				Metrics = ["requests", "responses"],
				Sort = ["-requests", "+responses"]
			};

			var client = GetClient();

			// Act
			var response = await client.GetDnsFirewallAnalyticsReport(AccountId, ClusterId, filter, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			Assert.IsNotNull(response.Result);
			Assert.IsInstanceOfType<DnsAnalyticsReport>(response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_firewall/{ClusterId}/dns_analytics/report", requestPath);
			Assert.IsNotNull(queryFilter);

			Assert.IsInstanceOfType<GetDnsAnalyticsReportFilter>(queryFilter);

			_clientMock.Verify(m => m.GetAsync<DnsAnalyticsReport>($"/accounts/{AccountId}/dns_firewall/{ClusterId}/dns_analytics/report", filter, TestContext.CancellationToken), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public void ShouldReturnEmptyParameterList()
		{
			// Arrange
			var filter = new GetDnsAnalyticsReportFilter();

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
			var since = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			var until = new DateTime(2024, 1, 8, 0, 0, 0, DateTimeKind.Utc);
			var filter = new GetDnsAnalyticsReportFilter
			{
				Dimensions = ["queryName", "responseCode"],
				Filters = "queryType eq A",
				Limit = 1000,
				Metrics = ["requests", "responses"],
				Since = since,
				Sort = ["-requests", "+responses"],
				Until = until
			};

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.HasCount(7, dict);

			Assert.IsTrue(dict.ContainsKey("dimensions"));
			Assert.AreEqual("queryName,responseCode", dict["dimensions"]);

			Assert.IsTrue(dict.ContainsKey("filters"));
			Assert.AreEqual("queryType eq A", dict["filters"]);

			Assert.IsTrue(dict.ContainsKey("limit"));
			Assert.AreEqual("1000", dict["limit"]);

			Assert.IsTrue(dict.ContainsKey("metrics"));
			Assert.AreEqual("requests,responses", dict["metrics"]);

			Assert.IsTrue(dict.ContainsKey("since"));
			Assert.AreEqual(since.ToIso8601Format(), dict["since"]);

			Assert.IsTrue(dict.ContainsKey("sort"));
			Assert.AreEqual("-requests,+responses", dict["sort"]);

			Assert.IsTrue(dict.ContainsKey("until"));
			Assert.AreEqual(until.ToIso8601Format(), dict["until"]);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddFilters(string str)
		{
			// Arrange
			var filter = new GetDnsAnalyticsReportFilter { Filters = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow(0)]
		public void ShouldNotAddLimit(int? limit)
		{
			// Arrange
			var filter = new GetDnsAnalyticsReportFilter { Limit = limit };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		public void ShouldNotAddDimensionsIfEmpty()
		{
			// Arrange
			var filter = new GetDnsAnalyticsReportFilter { Dimensions = [] };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		public void ShouldNotAddMetricsIfEmpty()
		{
			// Arrange
			var filter = new GetDnsAnalyticsReportFilter { Metrics = [] };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		public void ShouldNotAddSortIfEmpty()
		{
			// Arrange
			var filter = new GetDnsAnalyticsReportFilter { Sort = [] };

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
				.Setup(m => m.GetAsync<DnsAnalyticsReport>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
