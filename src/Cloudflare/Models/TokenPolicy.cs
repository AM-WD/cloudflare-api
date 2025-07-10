using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// A token policy.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L874">Source</see>
	/// </summary>
	public class TokenPolicy
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TokenPolicy"/> class.
		/// </summary>
		/// <param name="id">Policy identifier.</param>
		/// <param name="effect">Allow or deny operations against the resources.</param>
		public TokenPolicy(string id, TokenPolicyEffect effect)
		{
			Id = id;
			Effect = effect;
		}

		/// <summary>
		/// Policy identifier.
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// Allow or deny operations against the resources.
		/// </summary>
		[JsonProperty("effect")]
		public TokenPolicyEffect Effect { get; set; }

		/// <summary>
		/// A set of permission groups that are specified to the policy.
		/// </summary>
		[JsonProperty("permission_groups")]
		public IReadOnlyCollection<TokenPolicyPermissionGroup> PermissionGroups { get; set; } = [];

		/// <summary>
		/// A list of resource names that the policy applies to.
		/// </summary>
		[JsonProperty("resources")]
		public IDictionary<string, string> Resources { get; set; } = new Dictionary<string, string>();
	}

	/// <summary>
	/// Allow or deny operations against the resources.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L883">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TokenPolicyEffect
	{
		/// <summary>
		/// Allow operations against the resources.
		/// </summary>
		[EnumMember(Value = "allow")]
		Allow = 1,

		/// <summary>
		/// Deny operations against the resources.
		/// </summary>
		[EnumMember(Value = "deny")]
		Deny = 2
	}

	/// <summary>
	/// A named group of permissions that map to a group of operations against resources.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L901">Source</see>
	/// </summary>
	public class TokenPolicyPermissionGroup
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TokenPolicyPermissionGroup"/> class.
		/// </summary>
		/// <param name="id">Identifier of the permission group.</param>
		public TokenPolicyPermissionGroup(string id)
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
		public TokenPolicyPermissionGroupMeta? Meta { get; set; }

		/// <summary>
		/// Name of the permission group.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }
	}

	/// <summary>
	/// Attributes associated to the permission group.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L922">Source</see>
	/// </summary>
	public class TokenPolicyPermissionGroupMeta
	{
		/// <summary>
		/// Key.
		/// </summary>
		[JsonProperty("key")]
		public string? Key { get; set; }

		/// <summary>
		/// Value.
		/// </summary>
		[JsonProperty("value")]
		public string? Value { get; set; }
	}
}
