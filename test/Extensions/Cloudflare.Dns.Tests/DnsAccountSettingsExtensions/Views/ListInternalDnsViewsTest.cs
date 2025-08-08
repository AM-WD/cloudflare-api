using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsAccountSettingsExtensions.Views
{
	[TestClass]
	public class ListInternalDnsViewsTest
	{
		public TestContext TestContext { get; set; }

		private const string AccountId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;
		private CloudflareResponse<IReadOnlyCollection<InternalDnsView>> _response;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<IReadOnlyCollection<InternalDnsView>>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				Result =
				[
					new InternalDnsView("023e105f4ecef8ad9ca31a8372d0c354", "View1"),
					new InternalDnsView("023e105f4ecef8ad9ca31a8372d0c355", "View2")
				]
			};
		}

		[TestMethod]
		public async Task ShouldListInternalDnsViews()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ListInternalDnsViews(AccountId, cancellationToken: TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);
			Assert.HasCount(2, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_settings/views", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<IReadOnlyCollection<InternalDnsView>>(
				$"/accounts/{AccountId}/dns_settings/views",
				null,
				It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldListInternalDnsViewsWithFilter()
		{
			// Arrange
			var client = GetClient();
			var filter = new ListInternalDnsViewsFilter
			{
				NameContains = "View"
			};

			// Act
			var response = await client.ListInternalDnsViews(AccountId, filter, TestContext.CancellationTokenSource.Token);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.IsNotNull(response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual($"/accounts/{AccountId}/dns_settings/views", requestPath);
			Assert.AreEqual(filter, queryFilter);

			_clientMock.Verify(m => m.GetAsync<IReadOnlyCollection<InternalDnsView>>(
				$"/accounts/{AccountId}/dns_settings/views",
				filter,
				It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		#region QueryFilter

		[TestMethod]
		public void ShouldReturnEmptyParameterList()
		{
			// Arrange
			var filter = new ListInternalDnsViewsFilter();

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
			var filter = new ListInternalDnsViewsFilter
			{
				Direction = SortDirection.Descending,
				Match = FilterMatchType.All,
				NameContains = "view",
				NameEndsWith = "end",
				NameExact = "exactView",
				NameStartsWith = "start",
				OrderBy = InternalDnsViewsOrderBy.ModifiedOn,
				Page = 2,
				PerPage = 100,
				ZoneId = "zone123",
				ZoneName = "zone.example.com",
			};

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.HasCount(11, dict);

			Assert.AreEqual("desc", dict["direction"]);
			Assert.AreEqual("all", dict["match"]);
			Assert.AreEqual("view", dict["name.contains"]);
			Assert.AreEqual("end", dict["name.endswith"]);
			Assert.AreEqual("exactView", dict["name.exact"]);
			Assert.AreEqual("start", dict["name.startswith"]);
			Assert.AreEqual("modified_on", dict["order"]);
			Assert.AreEqual("2", dict["page"]);
			Assert.AreEqual("100", dict["per_page"]);
			Assert.AreEqual("zone123", dict["zone_id"]);
			Assert.AreEqual("zone.example.com", dict["zone_name"]);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddNameContains(string str)
		{
			// Arrange
			var filter = new ListInternalDnsViewsFilter { NameContains = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddNameEndsWith(string str)
		{
			// Arrange
			var filter = new ListInternalDnsViewsFilter { NameEndsWith = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddNameExact(string str)
		{
			// Arrange
			var filter = new ListInternalDnsViewsFilter { NameExact = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddNameStartsWith(string str)
		{
			// Arrange
			var filter = new ListInternalDnsViewsFilter { NameStartsWith = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow((SortDirection)0)]
		public void ShouldNotAddDirection(SortDirection? direction)
		{
			// Arrange
			var filter = new ListInternalDnsViewsFilter { Direction = direction };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow((FilterMatchType)0)]
		public void ShouldNotAddMatch(FilterMatchType? match)
		{
			// Arrange
			var filter = new ListInternalDnsViewsFilter { Match = match };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow((InternalDnsViewsOrderBy)0)]
		public void ShouldNotAddOrder(InternalDnsViewsOrderBy? order)
		{
			// Arrange
			var filter = new ListInternalDnsViewsFilter { OrderBy = order };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow(0)]
		public void ShouldNotAddPage(int? page)
		{
			// Arrange
			var filter = new ListInternalDnsViewsFilter { Page = page };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow(0)]
		[DataRow(5_000_001)]
		public void ShouldNotAddPerPage(int? perPage)
		{
			// Arrange
			var filter = new ListInternalDnsViewsFilter { PerPage = perPage };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddZoneId(string str)
		{
			// Arrange
			var filter = new ListInternalDnsViewsFilter { ZoneId = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddZoneName(string str)
		{
			// Arrange
			var filter = new ListInternalDnsViewsFilter { ZoneName = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		#endregion QueryFilter

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<IReadOnlyCollection<InternalDnsView>>(
					It.IsAny<string>(),
					It.IsAny<IQueryParameterFilter>(),
					It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
