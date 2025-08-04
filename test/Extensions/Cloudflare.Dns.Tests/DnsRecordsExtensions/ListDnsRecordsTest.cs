using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using Moq;

namespace Cloudflare.Dns.Tests.DnsRecordsExtensions
{
	[TestClass]
	public class ListDnsRecordsTest
	{
		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<IReadOnlyCollection<DnsRecord>> _response;

		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _callbacks;

		[TestInitialize]
		public void Initialize()
		{
			_callbacks = [];

			_response = new CloudflareResponse<IReadOnlyCollection<DnsRecord>>
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
					TotalCount = 2000,
					TotalPages = 100,
				},
				Result = [
					new ARecord("example.com")
					{
						Id = "023e105f4ecef8ad9ca31a8372d0c353",
						Content = "96.7.128.175",
						Proxiable = true,
						Proxied = true,
						TimeToLive = 1,
						Settings = new(),
						Comment = "Domain verification record",
						Tags = [],
						CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
						ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
						CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
						TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
					},
					new AAAARecord("example.com")
					{
						Id = "023e105f4ecef8ad9ca31a8372d0c355",
						Content = "2600:1408:ec00:36::1736:7f31",
						Proxiable = true,
						Proxied = true,
						TimeToLive = 1,
						Settings = new(),
						Comment = "Domain verification record",
						Tags = [],
						CreatedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
						ModifiedOn = DateTime.Parse("2014-01-01T05:20:00.12345Z"),
						CommentModifiedOn = DateTime.Parse("2024-01-01T05:20:00.12345Z"),
						TagsModifiedOn = DateTime.Parse("2025-01-01T05:20:00.12345Z"),
					}
				]
			};
		}

		[TestMethod]
		public async Task ShouldListDnsRecords()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.ListDnsRecords(ZoneId);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records", callback.RequestPath);
			Assert.IsNull(callback.QueryFilter);

			_clientMock.Verify(m => m.GetAsync<IReadOnlyCollection<DnsRecord>>($"/zones/{ZoneId}/dns_records", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldListDnsRecordsWithFilter()
		{
			// Arrange
			var filter = new ListDnsRecordsFilter
			{
				Name = "example.com"
			};

			var client = GetClient();

			// Act
			var response = await client.ListDnsRecords(ZoneId, filter);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_response.Result, response.Result);

			Assert.AreEqual(1, _callbacks.Count);

			var callback = _callbacks.First();
			Assert.AreEqual($"/zones/{ZoneId}/dns_records", callback.RequestPath);
			Assert.IsNotNull(callback.QueryFilter);

			Assert.IsInstanceOfType<ListDnsRecordsFilter>(callback.QueryFilter);
			Assert.AreEqual("example.com", ((ListDnsRecordsFilter)callback.QueryFilter).Name);

			_clientMock.Verify(m => m.GetAsync<IReadOnlyCollection<DnsRecord>>($"/zones/{ZoneId}/dns_records", filter, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public void ShouldReturnEmptyParameterList()
		{
			// Arrange
			var filter = new ListDnsRecordsFilter();

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		public void ShouldReturnFullParameterList()
		{
			// Arrange
			var filter = new ListDnsRecordsFilter
			{
				Comment = "Hello World",
				CommentAbsent = true,
				CommentContains = "lo Wor",
				CommentEndsWith = "ld",
				CommentExact = "Hello World",
				CommentPresent = true,
				CommentStartsWith = "He",

				Content = "127.0.0.1",
				ContentContains = "7.0.0",
				ContentEndsWith = "0.1",
				ContentExact = "127.0.0.1",
				ContentStartsWith = "127",

				Direction = SortDirection.Descending,
				Match = FilterMatchType.All,

				Name = "example.com",
				NameContains = "ample",
				NameEndsWith = ".com",
				NameExact = "example.com",
				NameStartsWith = "ex",

				OrderBy = DnsRecordsOrderBy.Name,
				Page = 2,
				PerPage = 5,
				Proxied = true,
				Search = "Some Search",

				Tag = "team:DNS",
				TagAbsent = "important",
				TagContains = "greeting:ello",
				TagEndsWith = "greeting:rld",
				TagExact = "greeting:Hello World",
				TagPresent = "important",
				TagStartsWith = "greeting:Hel",

				TagMatch = FilterMatchType.Any,
				Type = DnsRecordType.A,
			};

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(33, dict.Count);

			Assert.IsTrue(dict.ContainsKey("comment"));
			Assert.IsTrue(dict.ContainsKey("comment.absent"));
			Assert.IsTrue(dict.ContainsKey("comment.contains"));
			Assert.IsTrue(dict.ContainsKey("comment.endswith"));
			Assert.IsTrue(dict.ContainsKey("comment.exact"));
			Assert.IsTrue(dict.ContainsKey("comment.present"));
			Assert.IsTrue(dict.ContainsKey("comment.startswith"));

			Assert.IsTrue(dict.ContainsKey("content"));
			Assert.IsTrue(dict.ContainsKey("content.contains"));
			Assert.IsTrue(dict.ContainsKey("content.endswith"));
			Assert.IsTrue(dict.ContainsKey("content.exact"));
			Assert.IsTrue(dict.ContainsKey("content.startswith"));

			Assert.IsTrue(dict.ContainsKey("direction"));
			Assert.IsTrue(dict.ContainsKey("match"));

			Assert.IsTrue(dict.ContainsKey("name"));
			Assert.IsTrue(dict.ContainsKey("name.contains"));
			Assert.IsTrue(dict.ContainsKey("name.endswith"));
			Assert.IsTrue(dict.ContainsKey("name.exact"));
			Assert.IsTrue(dict.ContainsKey("name.startswith"));

			Assert.IsTrue(dict.ContainsKey("order"));
			Assert.IsTrue(dict.ContainsKey("page"));
			Assert.IsTrue(dict.ContainsKey("per_page"));
			Assert.IsTrue(dict.ContainsKey("proxied"));
			Assert.IsTrue(dict.ContainsKey("search"));

			Assert.IsTrue(dict.ContainsKey("tag"));
			Assert.IsTrue(dict.ContainsKey("tag.absent"));
			Assert.IsTrue(dict.ContainsKey("tag.contains"));
			Assert.IsTrue(dict.ContainsKey("tag.endswith"));
			Assert.IsTrue(dict.ContainsKey("tag.exact"));
			Assert.IsTrue(dict.ContainsKey("tag.present"));
			Assert.IsTrue(dict.ContainsKey("tag.startswith"));

			Assert.IsTrue(dict.ContainsKey("tag_match"));
			Assert.IsTrue(dict.ContainsKey("type"));

			Assert.AreEqual("Hello World", dict["comment"]);
			Assert.AreEqual("true", dict["comment.absent"]);
			Assert.AreEqual("lo Wor", dict["comment.contains"]);
			Assert.AreEqual("ld", dict["comment.endswith"]);
			Assert.AreEqual("Hello World", dict["comment.exact"]);
			Assert.AreEqual("true", dict["comment.present"]);
			Assert.AreEqual("He", dict["comment.startswith"]);

			Assert.AreEqual("127.0.0.1", dict["content"]);
			Assert.AreEqual("7.0.0", dict["content.contains"]);
			Assert.AreEqual("0.1", dict["content.endswith"]);
			Assert.AreEqual("127.0.0.1", dict["content.exact"]);
			Assert.AreEqual("127", dict["content.startswith"]);

			Assert.AreEqual("desc", dict["direction"]);
			Assert.AreEqual("all", dict["match"]);

			Assert.AreEqual("example.com", dict["name"]);
			Assert.AreEqual("ample", dict["name.contains"]);
			Assert.AreEqual(".com", dict["name.endswith"]);
			Assert.AreEqual("example.com", dict["name.exact"]);
			Assert.AreEqual("ex", dict["name.startswith"]);

			Assert.AreEqual("name", dict["order"]);
			Assert.AreEqual("2", dict["page"]);
			Assert.AreEqual("5", dict["per_page"]);
			Assert.AreEqual("true", dict["proxied"]);
			Assert.AreEqual("Some Search", dict["search"]);

			Assert.AreEqual("team:DNS", dict["tag"]);
			Assert.AreEqual("important", dict["tag.absent"]);
			Assert.AreEqual("greeting:ello", dict["tag.contains"]);
			Assert.AreEqual("greeting:rld", dict["tag.endswith"]);
			Assert.AreEqual("greeting:Hello World", dict["tag.exact"]);
			Assert.AreEqual("important", dict["tag.present"]);
			Assert.AreEqual("greeting:Hel", dict["tag.startswith"]);

			Assert.AreEqual("any", dict["tag_match"]);
			Assert.AreEqual("A", dict["type"]);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddComment(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { Comment = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow(false)]
		public void ShouldNotAddCommentAbsent(bool? b)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { CommentAbsent = b };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddCommentContains(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { CommentContains = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddCommentEndsWith(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { CommentEndsWith = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddCommentExact(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { CommentExact = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow(false)]
		public void ShouldNotAddCommentPresent(bool? b)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { CommentPresent = b };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddCommentStartsWith(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { CommentStartsWith = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddContent(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { Content = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddContentContains(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { ContentContains = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddContentEndsWith(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { ContentEndsWith = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddContentExact(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { ContentExact = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddContentStartsWith(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { ContentStartsWith = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow((SortDirection)0)]
		public void ShouldNotAddDirection(SortDirection? direction)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { Direction = direction };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow((FilterMatchType)0)]
		public void ShouldNotAddMatch(FilterMatchType? match)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { Match = match };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddName(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { Name = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddNameContains(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { NameContains = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddNameEndsWith(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { NameEndsWith = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddNameExact(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { NameExact = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddNameStartsWith(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { NameStartsWith = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow((DnsRecordsOrderBy)0)]
		public void ShouldNotAddOrder(DnsRecordsOrderBy? order)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { OrderBy = order };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow(0)]
		public void ShouldNotAddPage(int? page)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { Page = page };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow(0)]
		[DataRow(5_000_001)]
		public void ShouldNotAddPerPage(int? perPage)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { PerPage = perPage };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddTag(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { Tag = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddTagAbsent(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { TagAbsent = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddTagContains(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { TagContains = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddTagEndsWith(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { TagEndsWith = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddTagExact(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { TagExact = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddTagPresent(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { TagPresent = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow("")]
		[DataRow("   ")]
		public void ShouldNotAddTagStartsWith(string str)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { TagStartsWith = str };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow((FilterMatchType)0)]
		public void ShouldNotAddTagMatch(FilterMatchType? tagMatch)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { TagMatch = tagMatch };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		[TestMethod]
		[DataRow(null)]
		[DataRow((DnsRecordType)0)]
		public void ShouldNotAddType(DnsRecordType? type)
		{
			// Arrange
			var filter = new ListDnsRecordsFilter { Type = type };

			// Act
			var dict = filter.GetQueryParameters();

			// Assert
			Assert.IsNotNull(dict);
			Assert.AreEqual(0, dict.Count);
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<IReadOnlyCollection<DnsRecord>>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _callbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _response);

			return _clientMock.Object;
		}
	}
}
