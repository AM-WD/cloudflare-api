using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// A Cloudflare zone.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/zones.ts#L231">Source</see>
	/// </summary>
	public class Zone
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Zone"/> class.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="name">The domain name.</param>
		/// <param name="nameServers">The name servers Cloudflare assigns to the zone.</param>
		/// <param name="account">The account the zone belongs to.</param>
		/// <param name="meta">Metadata about the zone.</param>
		/// <param name="owner">The owner of the zone.</param>
		public Zone(string id, string name, IReadOnlyCollection<string> nameServers, ZoneAccount account, ZoneMeta meta, ZoneOwner owner)
		{
			Id = id;
			Account = account;
			Meta = meta;
			Name = name;
			NameServers = nameServers;
			Owner = owner;

#pragma warning disable CS0612
			Plan = new();
#pragma warning restore CS0612
		}

		/// <summary>
		/// Identifier.
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// The account the zone belongs to.
		/// </summary>
		[JsonProperty("account")]
		public ZoneAccount Account { get; set; }

		/// <summary>
		/// The last time proof of ownership was detected and the zone was made active.
		/// </summary>
		[JsonProperty("activated_on")]
		public DateTime? ActivatedOn { get; set; }

		/// <summary>
		/// When the zone was created.
		/// </summary>
		[JsonProperty("created_on")]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// The interval (in seconds) from when development mode expires (positive integer)
		/// or last expired (negative integer) for the domain. If development mode has never
		/// been enabled, this value is 0.
		/// </summary>
		[JsonProperty("development_mode")]
		public int DevelopmentMode { get; set; }

		/// <summary>
		/// Metadata about the zone.
		/// </summary>
		[JsonProperty("meta")]
		public ZoneMeta Meta { get; set; }

		/// <summary>
		/// When the zone was last modified.
		/// </summary>
		[JsonProperty("modified_on")]
		public DateTime ModifiedOn { get; set; }

		/// <summary>
		/// The domain name.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// The name servers Cloudflare assigns to a zone.
		/// </summary>
		[JsonProperty("name_servers")]
		public IReadOnlyCollection<string> NameServers { get; set; }

		/// <summary>
		/// DNS host at the time of switching to Cloudflare.
		/// </summary>
		[JsonProperty("original_dnshost")]
		public string? OriginalDnsHost { get; set; }

		/// <summary>
		/// Original name servers before moving to Cloudflare.
		/// </summary>
		[JsonProperty("original_name_servers")]
		public IReadOnlyCollection<string>? OriginalNameServers { get; set; }

		/// <summary>
		/// Registrar for the domain at the time of switching to Cloudflare.
		/// </summary>
		[JsonProperty("original_registrar")]
		public string? OriginalRegistrar { get; set; }

		/// <summary>
		/// The owner of the zone.
		/// </summary>
		[JsonProperty("owner")]
		public ZoneOwner Owner { get; set; }

		/// <summary>
		/// A Zones subscription information.
		/// </summary>
		[Obsolete]
		[JsonProperty("plan")]
		public ZonePlan Plan { get; set; }

		/// <summary>
		/// Allows the customer to use a custom apex.
		/// <em>Tenants Only Configuration</em>.
		/// </summary>
		[JsonProperty("cname_suffix")]
		public string? CnameSuffix { get; set; }

		/// <summary>
		/// Indicates whether the zone is only using Cloudflare DNS services.
		/// </summary>
		/// <remarks>
		/// A <see langword="true"/> value means the zone will not receive security or performance benefits.
		/// </remarks>
		[JsonProperty("paused")]
		public bool? Paused { get; set; }

		/// <summary>
		/// Legacy permissions based on legacy user membership information.
		/// </summary>
		[Obsolete]
		[JsonProperty("permissions")]
		public IReadOnlyCollection<string>? Permissions { get; set; }

		/// <summary>
		/// The zone status on Cloudflare.
		/// </summary>
		[JsonProperty("status")]
		public ZoneStatus? Status { get; set; }

		/// <summary>
		/// The root organizational unit that this zone belongs to (such as a tenant or organization).
		/// </summary>
		[JsonProperty("tenant")]
		public ZoneTenant? Tenant { get; set; }

		/// <summary>
		/// The immediate parent organizational unit that this zone belongs to (such as under a tenant or sub-organization).
		/// </summary>
		[JsonProperty("tenant_unit")]
		public ZoneTenantUnit? TenantUnit { get; set; }

		/// <summary>
		/// A full zone implies that DNS is hosted with Cloudflare.
		/// A partial zone is typically a partner-hosted zone or a CNAME setup.
		/// </summary>
		[JsonProperty("type")]
		public ZoneType? Type { get; set; }

		/// <summary>
		/// An array of domains used for custom name servers.
		/// This is only available for Business and Enterprise plans.
		/// </summary>
		[JsonProperty("vanity_name_servers")]
		public IReadOnlyCollection<string>? VanityNameServers { get; set; }

		/// <summary>
		/// Verification key for partial zone setup.
		/// </summary>
		[JsonProperty("verification_key")]
		public string? VerificationKey { get; set; }
	}

	/// <summary>
	/// The account the zone belongs to.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/zones.ts#L359">Source</see>
	/// </summary>
	public class ZoneAccount
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// The name of the account.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }
	}

	/// <summary>
	/// Metadata about the zone.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/zones.ts#L374">Source</see>
	/// </summary>
	public class ZoneMeta
	{
		/// <summary>
		/// The zone is only configured for CDN.
		/// </summary>
		[JsonProperty("cdn_only")]
		public bool? CdnOnly { get; set; }

		/// <summary>
		/// Number of Custom Certificates the zone can have.
		/// </summary>
		[JsonProperty("custom_certificate_quota")]
		public int? CustomCertificateQuota { get; set; }

		/// <summary>
		/// The zone is only configured for DNS.
		/// </summary>
		[JsonProperty("dns_only")]
		public bool? DnsOnly { get; set; }

		/// <summary>
		/// The zone is setup with Foundation DNS.
		/// </summary>
		[JsonProperty("foundation_dns")]
		public bool? FoundationDns { get; set; }

		/// <summary>
		/// Number of Page Rules a zone can have.
		/// </summary>
		[JsonProperty("page_rule_quota")]
		public int? PageRuleQuota { get; set; }

		/// <summary>
		/// The zone has been flagged for phishing.
		/// </summary>
		[JsonProperty("phishing_detected")]
		public bool? PhishingDetected { get; set; }

		/// <summary>
		/// Step.
		/// </summary>
		[JsonProperty("step")]
		public int? Step { get; set; }
	}

	/// <summary>
	/// The owner of the zone.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/zones.ts#L411">Source</see>
	/// </summary>
	public class ZoneOwner
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// Name of the owner.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }

		/// <summary>
		/// The type of owner.
		/// </summary>
		[JsonProperty("type")]
		public string? Type { get; set; }
	}

	/// <summary>
	/// A Zones subscription information.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/zones.ts#L431">Source</see>
	/// </summary>
	[Obsolete]
	public class ZonePlan
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// States if the subscription can be activated.
		/// </summary>
		[JsonProperty("can_subscribe")]
		public bool? CanSubscribe { get; set; }

		/// <summary>
		/// The denomination of the customer.
		/// </summary>
		[JsonProperty("currency")]
		public string? Currency { get; set; }

		/// <summary>
		/// If this Zone is managed by another company.
		/// </summary>
		[JsonProperty("externally_managed")]
		public bool? ExternallyManaged { get; set; }

		/// <summary>
		/// How often the customer is billed.
		/// </summary>
		[JsonProperty("frequency")]
		public Frequency? Frequency { get; set; }

		/// <summary>
		/// States if the subscription active.
		/// </summary>
		[JsonProperty("is_subscribed")]
		public bool? IsSubscribed { get; set; }

		/// <summary>
		/// If the legacy discount applies to this Zone.
		/// </summary>
		[JsonProperty("legacy_discount")]
		public bool? LegacyDiscount { get; set; }

		/// <summary>
		/// The legacy name of the plan.
		/// </summary>
		[JsonProperty("legacy_id")]
		public string? LegacyId { get; set; }

		/// <summary>
		/// Name of the owner.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }

		/// <summary>
		/// How much the customer is paying.
		/// </summary>
		[JsonProperty("price")]
		public decimal? Price { get; set; }
	}

	/// <summary>
	/// Zone status.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/zones.ts#L323">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ZoneStatus
	{
		/// <summary>
		/// Initializing.
		/// </summary>
		[EnumMember(Value = "initializing")]
		Initializing = 1,

		/// <summary>
		/// Pending.
		/// </summary>
		[EnumMember(Value = "pending")]
		Pending = 2,

		/// <summary>
		/// Active.
		/// </summary>
		[EnumMember(Value = "active")]
		Active = 3,

		/// <summary>
		/// Moved.
		/// </summary>
		[EnumMember(Value = "moved")]
		Moved = 4
	}

	/// <summary>
	/// The root organizational unit that this zone belongs to (such as a tenant or organization).
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/zones.ts#L487">Source</see>
	/// </summary>
	public class ZoneTenant
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// The name of the Tenant account.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }
	}

	/// <summary>
	/// The immediate parent organizational unit that this zone belongs to (such as under a tenant or sub-organization).
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/zones.ts#L503">Source</see>
	/// </summary>
	public class ZoneTenantUnit
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }
	}
}
