namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Aegis provides dedicated egress IPs (from Cloudflare to your origin) for your
	/// layer 7 WAF and CDN services. The egress IPs are reserved exclusively for your
	/// account so that you can increase your origin security by only allowing traffic
	/// from a small list of IP addresses.
	/// </summary>
	public class Aegis : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Aegis"/> class.
		/// </summary>
		public Aegis()
		{
			Id = ZoneSettingId.Aegis;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public AegisValue? Value { get; set; }
	}

	/// <summary>
	/// Value of the zone setting.
	/// </summary>
	public class AegisValue
	{
		/// <summary>
		/// Whether the feature is enabled or not.
		/// </summary>
		[JsonProperty("enabled")]
		public bool? Enabled { get; set; }

		/// <summary>
		/// Egress pool id which refers to a grouping of dedicated egress IPs through which
		/// Cloudflare will connect to origin.
		/// </summary>
		[JsonProperty("pool_id")]
		public string? PoolId { get; set; }
	}
}
