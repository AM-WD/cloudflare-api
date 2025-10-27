using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using Moq;

namespace Cloudflare.Zones.Tests.ZonesExtensions
{
	[TestClass]
	public class ListZonesTest
	{
		public TestContext TestContext { get; set; }

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<IReadOnlyCollection<Zone>> _response;

		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<IReadOnlyCollection<Zone>>
			{
				Success = true,
				Messages = [
					new ResponseInfo(1000, "Message 1")
				],
				Errors = [
					new ResponseInfo(1000, "Error 1")
				],
				ResultInfo = new PaginationInfo
				{
					Count = 1,
					Page = 1,
					PerPage = 20,
					TotalCount = 2000
				},
				Result =
				[
					new Zone(
						ZoneId,
						"example.com",
						[
							"bob.ns.cloudflare.com",
							"lola.ns.cloudflare.com"
						],
						new ZoneAccount
						{
							Id = "023e105f4ecef8ad9ca31a8372d0c353",
							Name = "Example Account Name"
						},
						new ZoneMeta
						{
							CdnOnly = true,
							CustomCertificateQuota = 1,
							DnsOnly = true,
							FoundationDns = true,
							PageRuleQuota = 100,
							PhishingDetected = false,
							Step = 2
						},
						new ZoneOwner
						{
							Id = "023e105f4ecef8ad9ca31a8372d0c353",
							Name = "Example Org",
							Type = "organization"
						}
					)
					{
						ActivatedOn = DateTime.Parse("2014-01-02T00:01:00.12345Z"),
						CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
						DevelopmentMode = 7200,
						ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
						OriginalDnsHost = "NameCheap",
						OriginalNameServers =
						[
							"ns1.originaldnshost.com",
							"ns2.originaldnshost.com"
						],
						OriginalRegistrar = "GoDaddy",
						Paused = true,
						Status = ZoneStatus.Initializing,
						Type = ZoneType.Full,
						VanityNameServers =
						[
							"ns1.example.com",
							"ns2.example.com"
						]
					}
				]
			};
		}

		[TestMethod]
		public async Task ShouldReturnListOfZones()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ListZones(cancellationToken: TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual("/zones", requestPath);
			Assert.IsNull(queryFilter);

			_clientMock.Verify(m => m.GetAsync<IReadOnlyCollection<Zone>>("/zones", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldReturnListOfZonesWithFilter()
		{
			// Arrange
			var filter = new ListZonesFilter
			{
				AccountId = "023e105f4ecef8ad9ca31a8372d0c353"
			};

			var client = GetClient();

			// Act
			var response = await client.ListZones(filter, TestContext.CancellationToken);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.HasCount(1, _callbacks);

			var (requestPath, queryFilter) = _callbacks.First();
			Assert.AreEqual("/zones", requestPath);
			Assert.AreEqual(filter, queryFilter);

			_clientMock.Verify(m => m.GetAsync<IReadOnlyCollection<Zone>>("/zones", filter, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public void ShouldReturnEmptyParameterList()
		{
			// Arrange
			var filter = new ListZonesFilter();

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
			var filter = new ListZonesFilter
			{
				AccountId = "023e105f4ecef8ad9ca31a8372d0c353",
				AccountName = "Example Account Name",
				Match = ListZonesMatch.Any,
				Name = "example.com",
				PerPage = 13,
				Page = 5,
				OrderBy = ListZonesOrderBy.AccountName,
				Direction = SortDirection.Descending,
				Status = ZoneStatus.Active
			};

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.HasCount(9, dict);

			Assert.IsTrue(dict.ContainsKey("account.id"));
			Assert.IsTrue(dict.ContainsKey("account.name"));
			Assert.IsTrue(dict.ContainsKey("direction"));
			Assert.IsTrue(dict.ContainsKey("match"));
			Assert.IsTrue(dict.ContainsKey("name"));
			Assert.IsTrue(dict.ContainsKey("order"));
			Assert.IsTrue(dict.ContainsKey("page"));
			Assert.IsTrue(dict.ContainsKey("per_page"));
			Assert.IsTrue(dict.ContainsKey("status"));

			Assert.AreEqual("023e105f4ecef8ad9ca31a8372d0c353", dict["account.id"]);
			Assert.AreEqual("Example Account Name", dict["account.name"]);
			Assert.AreEqual("desc", dict["direction"]);
			Assert.AreEqual("any", dict["match"]);
			Assert.AreEqual("example.com", dict["name"]);
			Assert.AreEqual("account.name", dict["order"]);
			Assert.AreEqual("5", dict["page"]);
			Assert.AreEqual("13", dict["per_page"]);
			Assert.AreEqual("active", dict["status"]);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddAccountId(string id)
		{
			// Arrange
			var filter = new ListZonesFilter
			{
				AccountId = id
			};

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
		public void ShouldNotAddAccountName(string name)
		{
			// Arrange
			var filter = new ListZonesFilter
			{
				AccountName = name
			};

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		public void ShouldNotAddDirection()
		{
			// Arrange
			var filter = new ListZonesFilter
			{
				Direction = 0
			};

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		public void ShouldNotAddMatch()
		{
			// Arrange
			var filter = new ListZonesFilter
			{
				Match = 0
			};

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
		public void ShouldNotAddName(string name)
		{
			// Arrange
			var filter = new ListZonesFilter
			{
				Name = name
			};

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		public void ShouldNotAddOrder()
		{
			// Arrange
			var filter = new ListZonesFilter
			{
				OrderBy = 0
			};

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		public void ShouldNotAddPage()
		{
			// Arrange
			var filter = new ListZonesFilter
			{
				Page = 0
			};

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		[DataRow(4)]
		[DataRow(51)]
		public void ShouldNotAddPerPage(int perPage)
		{
			// Arrange
			var filter = new ListZonesFilter
			{
				PerPage = perPage
			};

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.IsEmpty(dict);
		}

		[TestMethod]
		public void ShouldNotAddStatus()
		{
			// Arrange
			var filter = new ListZonesFilter
			{
				Status = 0
			};

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
				.Setup(m => m.GetAsync<IReadOnlyCollection<Zone>>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
