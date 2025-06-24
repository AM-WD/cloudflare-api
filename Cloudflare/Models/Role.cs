namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// A Cloudflare role.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L641">Source</see>
	/// </summary>
	public class Role
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Role"/> class.
		/// </summary>
		/// <param name="id">Role identifier tag.</param>
		/// <param name="name">Role name.</param>
		/// <param name="description">Description of role's permissions.</param>
		/// <param name="permissions">Permissions.</param>
		public Role(string id, string name, string description, RolePermissions permissions)
		{
			Id = id;
			Name = name;
			Description = description;
			Permissions = permissions;
		}

		/// <summary>
		/// Role identifier tag.
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// Description of role's permissions.
		/// </summary>
		[JsonProperty("description")]
		public string Description { get; set; }

		/// <summary>
		/// Role name.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// Role permissions.
		/// </summary>
		[JsonProperty("permissions")]
		public RolePermissions Permissions { get; set; }
	}

	/// <summary>
	/// Role permissions.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L661">Source</see>
	/// </summary>
	public class RolePermissions
	{
		/// <summary>
		/// Analytics permissions.
		/// </summary>
		[JsonProperty("analytics")]
		public PermissionGrant? Analytics { get; set; }

		/// <summary>
		/// Billing permissions.
		/// </summary>
		[JsonProperty("billing")]
		public PermissionGrant? Billing { get; set; }

		/// <summary>
		/// Cache Purge permissions.
		/// </summary>
		[JsonProperty("cache_purge")]
		public PermissionGrant? CachePurge { get; set; }

		/// <summary>
		/// DNS permissions.
		/// </summary>
		[JsonProperty("dns")]
		public PermissionGrant? Dns { get; set; }

		/// <summary>
		/// DNS Records permissions.
		/// </summary>
		[JsonProperty("dns_records")]
		public PermissionGrant? DnsRecords { get; set; }

		/// <summary>
		/// Load Balancer permissions.
		/// </summary>
		[JsonProperty("lb")]
		public PermissionGrant? LoadBalancer { get; set; }

		/// <summary>
		/// Logs permissions.
		/// </summary>
		[JsonProperty("logs")]
		public PermissionGrant? Logs { get; set; }

		/// <summary>
		/// Organization permissions.
		/// </summary>
		[JsonProperty("organization")]
		public PermissionGrant? Organization { get; set; }

		/// <summary>
		/// SSL permissions.
		/// </summary>
		[JsonProperty("ssl")]
		public PermissionGrant? Ssl { get; set; }

		/// <summary>
		/// WAF permissions.
		/// </summary>
		[JsonProperty("waf")]
		public PermissionGrant? WebApplicationFirewall { get; set; }

		/// <summary>
		/// Zone Settings permissions.
		/// </summary>
		[JsonProperty("zone_settings")]
		public PermissionGrant? ZoneSettings { get; set; }

		/// <summary>
		/// Zones permissions.
		/// </summary>
		[JsonProperty("zones")]
		public PermissionGrant? Zones { get; set; }
	}
}
