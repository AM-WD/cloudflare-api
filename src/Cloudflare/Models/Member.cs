using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// A Cloudflare member.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L282">Source</see>
	/// </summary>
	public class Member
	{
		/// <summary>
		/// Membership identifier tag.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// Access policy for the membership.
		/// </summary>
		[JsonProperty("policies")]
		public IReadOnlyCollection<MemberPolicy>? Policies { get; set; }

		/// <summary>
		/// Roles assigned to this Member.
		/// </summary>
		[JsonProperty("roles")]
		public IReadOnlyCollection<Role>? Roles { get; set; }

		/// <summary>
		/// A member's status in the account.
		/// </summary>
		[JsonProperty("status")]
		public MemberStatus? Status { get; set; }

		/// <summary>
		/// Details of the user associated to the membership.
		/// </summary>
		[JsonProperty("user")]
		public MemberUser? User { get; set; }
	}

	/// <summary>
	/// A member's access policy.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L310">Source</see>
	/// </summary>
	public class MemberPolicy
	{
		/// <summary>
		/// Policy identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// Allow or deny operations against the resources.
		/// </summary>
		[JsonProperty("access")]
		public MemberPolicyAccess? Access { get; set; }

		/// <summary>
		/// A set of permission groups that are specified to the policy.
		/// </summary>
		[JsonProperty("permission_groups")]
		public IReadOnlyCollection<MemberPolicyPermissionGroup>? PermissionGroups { get; set; }

		/// <summary>
		/// A list of resource groups that the policy applies to.
		/// </summary>
		[JsonProperty("resource_groups")]
		public IReadOnlyCollection<MemberPolicyResourceGroup>? ResourceGroups { get; set; }
	}

	/// <summary>
	/// A member's status.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L301">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum MemberStatus
	{
		/// <summary>
		/// The member has accepted the invitation.
		/// </summary>
		[EnumMember(Value = "accepted")]
		Accepted = 1,

		/// <summary>
		/// The member has not yet accepted the invitation.
		/// </summary>
		[EnumMember(Value = "pending")]
		Pending = 2
	}

	/// <summary>
	/// A member's policy access.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L319">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum MemberPolicyAccess
	{
		/// <summary>
		/// Allow operations against the resource.
		/// </summary>
		[EnumMember(Value = "allow")]
		Allow = 1,

		/// <summary>
		/// Deny operations against the resource.
		/// </summary>
		[EnumMember(Value = "deny")]
		Deny = 2
	}

	/// <summary>
	/// A member's permission group.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L337">Source</see>
	/// </summary>
	public class MemberPolicyPermissionGroup
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MemberPolicyPermissionGroup"/> class.
		/// </summary>
		/// <param name="id">Identifier of the permission group.</param>
		public MemberPolicyPermissionGroup(string id)
		{
			Id = id;
		}

		/// <summary>
		/// Identifier of the permission group.
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// Attributes associated to the permission group.
		/// </summary>
		[JsonProperty("meta")]
		public MemberPolicyPermissionGroupMeta? Meta { get; set; }

		/// <summary>
		/// Name of the permission group.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }
	}

	/// <summary>
	/// Attributes associated to the permission group.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L358">Source</see>
	/// </summary>
	public class MemberPolicyPermissionGroupMeta
	{
		/// <summary>
		/// The key of the attribute.
		/// </summary>
		[JsonProperty("key")]
		public string? Key { get; set; }

		/// <summary>
		/// The value of the attribute.
		/// </summary>
		[JsonProperty("value")]
		public string? Value { get; set; }
	}

	/// <summary>
	/// A group of scoped resources.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L368">Source</see>
	/// </summary>
	public class MemberPolicyResourceGroup
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MemberPolicyResourceGroup"/> class.
		/// </summary>
		/// <param name="id">Identifier of the resource group.</param>
		public MemberPolicyResourceGroup(string id)
		{
			Id = id;
		}

		/// <summary>
		/// Identifier of the resource group.
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// The scope associated to the resource group.
		/// </summary>
		[JsonProperty("scope")]
		public IReadOnlyCollection<MemberPolicyResourceGroupScope> Scope { get; set; } = [];

		/// <summary>
		/// Attributes associated to the resource group.
		/// </summary>
		[JsonProperty("meta")]
		public MemberPolicyResourceGroupMeta? Meta { get; set; }

		/// <summary>
		/// Name of the resource group.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }
	}

	/// <summary>
	/// Attributes associated to the resource group.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L394">Source</see>
	/// </summary>
	public class MemberPolicyResourceGroupScope
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MemberPolicyResourceGroupScope"/> class.
		/// </summary>
		/// <param name="key">Combination of pre-defined resource name and identifier.</param>
		public MemberPolicyResourceGroupScope(string key)
		{
			Key = key;
		}

		/// <summary>
		/// This is a combination of pre-defined resource name and identifier (like Account ID etc.)
		/// </summary>
		[JsonProperty("key")]
		public string Key { get; set; }

		/// <summary>
		/// A list of scope objects for additional context.
		/// </summary>
		[JsonProperty("objects")]
		public IReadOnlyCollection<MemberPolicyResourceGroupScopeObject> Objects { get; set; } = [];
	}

	/// <summary>
	/// A scope object for additional context.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L412">Source</see>
	/// </summary>
	public class MemberPolicyResourceGroupScopeObject
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MemberPolicyResourceGroupScopeObject"/> class.
		/// </summary>
		/// <param name="key">Combination of pre-defined resource name and identifier.</param>
		public MemberPolicyResourceGroupScopeObject(string key)
		{
			Key = key;
		}

		/// <summary>
		/// This is a combination of pre-defined resource name and identifier (like Zone ID etc.)
		/// </summary>
		[JsonProperty("key")]
		public string Key { get; set; }
	}

	/// <summary>
	/// Attributes associated to the resource group.
	/// </summary>
	public class MemberPolicyResourceGroupMeta
	{
		/// <summary>
		/// The key of the attribute.
		/// </summary>
		[JsonProperty("key")]
		public string? Key { get; set; }

		/// <summary>
		/// The value of the attribute.
		/// </summary>
		[JsonProperty("value")]
		public string? Value { get; set; }
	}

	/// <summary>
	/// Details of the user associated to the membership.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L435">Source</see>
	/// </summary>
	public class MemberUser
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MemberUser"/> class.
		/// </summary>
		/// <param name="email">The contact email address of the user.</param>
		public MemberUser(string email)
		{
			Email = email;
		}

		/// <summary>
		/// The contact email address of the user.
		/// </summary>
		[JsonProperty("email")]
		public string Email { get; set; }

		/// <summary>
		/// Identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

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
		/// Indicates whether two-factor authentication is enabled for the user account.
		/// Does not apply to API authentication.
		/// </summary>
		[JsonProperty("two_factor_authentication_enabled")]
		public bool? TwoFactorAuthEnabled { get; set; }
	}
}
