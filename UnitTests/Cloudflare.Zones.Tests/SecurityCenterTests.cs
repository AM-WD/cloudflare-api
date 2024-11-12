using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Zones;
using AMWD.Net.Api.Cloudflare.Zones.Internals.Requests;
using Moq;

namespace Cloudflare.Zones.Tests
{
	[TestClass]
	public class SecurityCenterTests
	{
		private CultureInfo _currentCulture;
		private CultureInfo _currentUICulture;

		private const string ZoneId = "023e105f4ecef8ad9ca31a8372d0c353";

		private Mock<ICloudflareClient> _clientMock;

		private CloudflareResponse<SecurityTxt> _getResponse;
		private CloudflareResponse<object> _updateResponse;
		private CloudflareResponse<object> _deleteResponse;

		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _getCallbacks;
		private List<(string RequestPath, InternalUpdateSecurityTxtRequest Request)> _updateCallbacks;
		private List<(string RequestPath, IQueryParameterFilter QueryFilter)> _deleteCallbacks;

		private UpdateSecurityTxtRequest _request;

		[TestInitialize]
		public void Initialize()
		{
			_getCallbacks = [];
			_updateCallbacks = [];
			_deleteCallbacks = [];

			_getResponse = new CloudflareResponse<SecurityTxt>
			{
				Success = true,
				Result = new SecurityTxt
				{
					Acknowledgements = ["https://example.com/hall-of-fame.html"],
					Canonical = ["https://www.example.com/.well-known/security.txt"],
					Contact = ["mailto:security@example.com"],
					Enabled = true,
					Encryption = ["https://example.com/pgp-key.txt"],
					Expires = DateTime.Parse("2019-08-24T14:15:22Z"),
					Hiring = ["https://example.com/jobs.html"],
					Policy = ["https://example.com/disclosure-policy.html"],
					PreferredLanguages = "de, en"
				}
			};
			_updateResponse = new CloudflareResponse<object> { Success = true };
			_deleteResponse = new CloudflareResponse<object> { Success = true };

			_request = new UpdateSecurityTxtRequest(ZoneId)
			{
				Acknowledgements = ["https://example.com/hall-of-fame.html"],
				Canonical = ["https://www.example.com/.well-known/security.txt"],
				Contact = ["mailto:security@example.com"],
				Enabled = true,
				Encryption = ["https://example.com/pgp-key.txt"],
				Expires = new DateTime(2024, 8, 1, 10, 20, 30, DateTimeKind.Unspecified),
				Hiring = ["https://example.com/jobs.html"],
				Policy = ["https://example.com/disclosure-policy.html"],
				PreferredLanguages = ["de", "en", ""]
			};

			_currentCulture = Thread.CurrentThread.CurrentCulture;
			_currentUICulture = Thread.CurrentThread.CurrentUICulture;

			Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
		}

		[TestCleanup]
		public void Cleanup()
		{
			Thread.CurrentThread.CurrentCulture = _currentCulture;
			Thread.CurrentThread.CurrentUICulture = _currentUICulture;
		}

		[TestMethod]
		public async Task ShouldGetSecurityTxt()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.GetSecurityTxt(ZoneId);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);
			Assert.AreEqual(_getResponse.Result, response.Result);

			Assert.AreEqual(1, _getCallbacks.Count);

			var callback = _getCallbacks.First();
			Assert.AreEqual($"zones/{ZoneId}/security-center/securitytxt", callback.RequestPath);
			Assert.IsNull(callback.QueryFilter);

			_clientMock.Verify(m => m.GetAsync<SecurityTxt>($"zones/{ZoneId}/security-center/securitytxt", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		[TestMethod]
		public async Task ShouldUpdateSecurityTxt()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.UpdateSecurityTxt(_request);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			Assert.AreEqual(1, _updateCallbacks.Count);

			var callback = _updateCallbacks.First();
			Assert.AreEqual($"zones/{ZoneId}/security-center/securitytxt", callback.RequestPath);
			Assert.IsNotNull(callback.Request);
			CollectionAssert.AreEqual(_request.Acknowledgements.ToArray(), callback.Request.Acknowledgements.ToArray());
			CollectionAssert.AreEqual(_request.Canonical.ToArray(), callback.Request.Canonical.ToArray());
			CollectionAssert.AreEqual(_request.Contact.ToArray(), callback.Request.Contact.ToArray());
			Assert.AreEqual(_request.Enabled, callback.Request.Enabled);
			CollectionAssert.AreEqual(_request.Encryption.ToArray(), callback.Request.Encryption.ToArray());
			Assert.AreEqual("01.08.2024 08:20:30 +0", callback.Request.Expires?.ToString("dd.MM.yyyy HH:mm:ss z"));
			CollectionAssert.AreEqual(_request.Hiring.ToArray(), callback.Request.Hiring.ToArray());
			CollectionAssert.AreEqual(_request.Policy.ToArray(), callback.Request.Policy.ToArray());
			Assert.AreEqual("de, en", callback.Request.PreferredLanguages);
		}

		[TestMethod]
		public async Task ShouldDeleteSecurityTxt()
		{
			// Arrange
			var client = GetClient();

			// Act
			var response = await client.DeleteSecurityTxt(ZoneId);

			// Assert
			Assert.IsNotNull(response);
			Assert.IsTrue(response.Success);

			Assert.AreEqual(1, _deleteCallbacks.Count);

			var callback = _deleteCallbacks.First();
			Assert.AreEqual($"zones/{ZoneId}/security-center/securitytxt", callback.RequestPath);
			Assert.IsNull(callback.QueryFilter);

			_clientMock.Verify(m => m.DeleteAsync<object>($"zones/{ZoneId}/security-center/securitytxt", null, It.IsAny<CancellationToken>()), Times.Once);
			_clientMock.VerifyNoOtherCalls();
		}

		private ICloudflareClient GetClient()
		{
			_clientMock = new Mock<ICloudflareClient>();
			_clientMock
				.Setup(m => m.GetAsync<SecurityTxt>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _getCallbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _getResponse);
			_clientMock
				.Setup(m => m.PutAsync<object, InternalUpdateSecurityTxtRequest>(It.IsAny<string>(), It.IsAny<InternalUpdateSecurityTxtRequest>(), It.IsAny<CancellationToken>()))
				.Callback<string, InternalUpdateSecurityTxtRequest, CancellationToken>((requestPath, queryFilter, _) => _updateCallbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _updateResponse);
			_clientMock
				.Setup(m => m.DeleteAsync<object>(It.IsAny<string>(), It.IsAny<IQueryParameterFilter>(), It.IsAny<CancellationToken>()))
				.Callback<string, IQueryParameterFilter, CancellationToken>((requestPath, queryFilter, _) => _deleteCallbacks.Add((requestPath, queryFilter)))
				.ReturnsAsync(() => _deleteResponse);

			return _clientMock.Object;
		}
	}
}
