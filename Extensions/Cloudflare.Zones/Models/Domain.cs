using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// A Cloudflare registrar domain.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/registrar/domains.ts#L79">Source</see>
	/// </summary>
	public class Domain
	{
		/// <summary>
		/// Domain identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// Shows if a domain is available for transferring into Cloudflare Registrar.
		/// </summary>
		[JsonProperty("available")]
		public bool? Available { get; set; }

		/// <summary>
		/// Indicates if the domain can be registered as a new domain.
		/// </summary>
		[JsonProperty("can_register")]
		public bool? CanRegister { get; set; }

		/// <summary>
		/// Shows time of creation.
		/// </summary>
		[JsonProperty("created_at")]
		public DateTime? CreatedAt { get; set; }

		/// <summary>
		/// Shows name of current registrar.
		/// </summary>
		[JsonProperty("current_registrar")]
		public string? CurrentRegistrar { get; set; }

		/// <summary>
		/// Shows when domain name registration expires.
		/// </summary>
		[JsonProperty("expires_at")]
		public DateTime? ExpiresAt { get; set; }

		/// <summary>
		/// Shows whether a registrar lock is in place for a domain.
		/// </summary>
		[JsonProperty("locked")]
		public bool? Locked { get; set; }

		/// <summary>
		/// Shows contact information for domain registrant.
		/// </summary>
		[JsonProperty("registrant_contact")]
		public DomainRegistrantContact? RegistrantContact { get; set; }

		/// <summary>
		/// A comma-separated list of registry status codes.
		/// </summary>
		/// <remarks>
		/// A full list of status codes can be found at <see href="https://www.icann.org/resources/pages/epp-status-codes-2014-06-16-en">EPP Status Codes</see>.
		/// </remarks>
		[JsonProperty("registry_statuses")]
		public string? RegistryStatuses { get; set; }

		/// <summary>
		/// Whether a particular TLD is currently supported by Cloudflare Registrar.
		/// </summary>
		/// <remarks>
		/// Refer to <see href="https://www.cloudflare.com/tld-policies/">TLD Policies</see> for a list of supported TLDs.
		/// </remarks>
		[JsonProperty("supported_tld")]
		public bool? SupportedTld { get; set; }

		/// <summary>
		/// Statuses for domain transfers into Cloudflare Registrar.
		/// </summary>
		[JsonProperty("transfer_in")]
		public DomainTransferIn? TransferIn { get; set; }

		/// <summary>
		/// Last updated.
		/// </summary>
		[JsonProperty("updated_at")]
		public DateTime? UpdatedAt { get; set; }
	}

	/// <summary>
	/// Shows contact information for domain registrant.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/registrar/domains.ts#L149">Source</see>
	/// </summary>
	public class DomainRegistrantContact
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DomainRegistrantContact"/> class.
		/// </summary>
		/// <param name="address">Address.</param>
		/// <param name="city">City.</param>
		/// <param name="state">State.</param>
		/// <param name="organization">User's organization.</param>
		public DomainRegistrantContact(string address, string city, string state, string organization)
		{
			Address = address;
			City = city;
			State = state;
			Organization = organization;
		}

		/// <summary>
		/// Address.
		/// </summary>
		[JsonProperty("address")]
		public string Address { get; set; }

		/// <summary>
		/// City.
		/// </summary>
		[JsonProperty("city")]
		public string City { get; set; }

		/// <summary>
		/// The country in which the user lives..
		/// </summary>
		[JsonProperty("country")]
		public string? Country { get; set; }

		/// <summary>
		/// User's first name.
		/// </summary>
		[JsonProperty("first_name")]
		public string? FirstName { get; set; }

		/// <summary>
		/// User's last name.
		/// </summary>
		[JsonProperty("last_name")]
		public string? LastName { get; set; }

		/// <summary>
		/// Name of organization.
		/// </summary>
		[JsonProperty("organization")]
		public string Organization { get; set; }

		/// <summary>
		/// User's telephone number.
		/// </summary>
		[JsonProperty("phone")]
		public string? Phone { get; set; }

		/// <summary>
		/// State.
		/// </summary>
		[JsonProperty("state")]
		public string State { get; set; }

		/// <summary>
		/// The zipcode or postal code where the user lives.
		/// </summary>
		[JsonProperty("zip")]
		public string? ZipCode { get; set; }

		/// <summary>
		/// Contact Identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// Optional address line for unit, floor, suite, etc.
		/// </summary>
		[JsonProperty("address2")]
		public string? Address2 { get; set; }

		/// <summary>
		/// The contact email address of the user.
		/// </summary>
		[JsonProperty("email")]
		public string? Email { get; set; }

		/// <summary>
		/// Contact fax number.
		/// </summary>
		[JsonProperty("fax")]
		public string? Fax { get; set; }
	}

	/// <summary>
	/// Statuses for domain transfers into Cloudflare Registrar.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/registrar/domains.ts#L219">Source</see>
	/// </summary>
	public class DomainTransferIn
	{
		/// <summary>
		/// Form of authorization has been accepted by the registrant.
		/// </summary>
		[JsonProperty("accept_foa")]
		public DomainTransferInAcceptFoa? AcceptFoa { get; set; }

		/// <summary>
		/// Shows transfer status with the registry.
		/// </summary>
		[JsonProperty("approve_transfer")]
		public DomainTransferInApproveTransfer? ApproveTransfer { get; set; }

		/// <summary>
		/// Indicates if cancellation is still possible.
		/// </summary>
		[JsonProperty("can_cancel_transfer")]
		public bool? CanCancelTransfer { get; set; }

		/// <summary>
		/// Privacy guards are disabled at the foreign registrar.
		/// </summary>
		[JsonProperty("disable_privacy")]
		public DomainTransferInDisablePrivacy? DisablePrivacy { get; set; }

		/// <summary>
		/// Auth code has been entered and verified.
		/// </summary>
		[JsonProperty("enter_auth_code")]
		public DomainTransferInEnterAuthCode? EnterAuthCode { get; set; }

		/// <summary>
		/// Domain is unlocked at the foreign registrar.
		/// </summary>
		[JsonProperty("unlock_domain")]
		public DomainTransferInUnlockDomain? UnlockDomain { get; set; }
	}

	/// <summary>
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/registrar/domains.ts#L223">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum DomainTransferInAcceptFoa
	{
		/// <summary>
		/// Needed.
		/// </summary>
		[EnumMember(Value = "needed")]
		Needed = 1,

		/// <summary>
		/// Ok.
		/// </summary>
		[EnumMember(Value = "ok")]
		Ok = 2
	}

	/// <summary>
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/registrar/domains.ts#L228">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum DomainTransferInApproveTransfer
	{
		/// <summary>
		/// Needed.
		/// </summary>
		[EnumMember(Value = "needed")]
		Needed = 1,

		/// <summary>
		/// Ok.
		/// </summary>
		[EnumMember(Value = "ok")]
		Ok = 2,

		/// <summary>
		/// Pending.
		/// </summary>
		[EnumMember(Value = "pending")]
		Pending = 3,

		/// <summary>
		/// Trying.
		/// </summary>
		[EnumMember(Value = "trying")]
		Trying = 4,

		/// <summary>
		/// Rejected.
		/// </summary>
		[EnumMember(Value = "rejected")]
		Rejected = 5,

		/// <summary>
		/// Unknown.
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 6
	}

	/// <summary>
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/registrar/domains.ts#L238">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum DomainTransferInDisablePrivacy
	{
		/// <summary>
		/// Needed.
		/// </summary>
		[EnumMember(Value = "needed")]
		Needed = 1,

		/// <summary>
		/// Ok.
		/// </summary>
		[EnumMember(Value = "ok")]
		Ok = 2,

		/// <summary>
		/// Unknown.
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 3
	}

	/// <summary>
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/registrar/domains.ts#L243">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum DomainTransferInEnterAuthCode
	{
		/// <summary>
		/// Needed.
		/// </summary>
		[EnumMember(Value = "needed")]
		Needed = 1,

		/// <summary>
		/// Ok.
		/// </summary>
		[EnumMember(Value = "ok")]
		Ok = 2,

		/// <summary>
		/// Pending.
		/// </summary>
		[EnumMember(Value = "pending")]
		Pending = 3,

		/// <summary>
		/// Trying.
		/// </summary>
		[EnumMember(Value = "trying")]
		Trying = 4,

		/// <summary>
		/// Rejected.
		/// </summary>
		[EnumMember(Value = "rejected")]
		Rejected = 5
	}

	/// <summary>
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/registrar/domains.ts#L248">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum DomainTransferInUnlockDomain
	{
		/// <summary>
		/// Needed.
		/// </summary>
		[EnumMember(Value = "needed")]
		Needed = 1,

		/// <summary>
		/// Ok.
		/// </summary>
		[EnumMember(Value = "ok")]
		Ok = 2,

		/// <summary>
		/// Pending.
		/// </summary>
		[EnumMember(Value = "pending")]
		Pending = 3,

		/// <summary>
		/// Trying.
		/// </summary>
		[EnumMember(Value = "trying")]
		Trying = 4,

		/// <summary>
		/// Unknown.
		/// </summary>
		[EnumMember(Value = "unknown")]
		Unknown = 5
	}
}
