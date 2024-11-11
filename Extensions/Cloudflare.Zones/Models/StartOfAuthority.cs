namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// The SOA (Start of Authority) record.
	/// </summary>
	public class StartOfAuthority
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StartOfAuthority"/> class.
		/// </summary>
		/// <param name="primaryNameserver">The primary nameserver.</param>
		/// <param name="zoneAdministrator">The zone administrator. First dot will be interpreted as @-sign.</param>
		/// <param name="ttl">The time to live of the SOA record.</param>
		/// <param name="refresh">Time in seconds after which secondary servers should re-check the SOA record to see if the zone has been updated.</param>
		/// <param name="retry">Time in seconds after which secondary servers should retry queries after the primary server was unresponsive.</param>
		/// <param name="expire">Time in seconds of being unable to query the primary server after which secondary servers should stop serving the zone.</param>
		/// <param name="minimumTtl">The time to live (TTL) for negative caching of records within the zone.</param>
		public StartOfAuthority(string primaryNameserver, string zoneAdministrator, int ttl, int refresh, int retry, int expire, int minimumTtl)
		{
			PrimaryNameserver = primaryNameserver;
			ZoneAdministrator = zoneAdministrator;
			Ttl = ttl;
			Refresh = refresh;
			Retry = retry;
			Expire = expire;
			MinimumTtl = minimumTtl;
		}

		/// <summary>
		/// Time in seconds of being unable to query the primary server after which secondary servers should stop serving the zone.
		/// </summary>
		/// <value>Unit: seconds. Range: <c>86400 &lt;=</c> X <c>&lt;= 2419200</c></value>
		[JsonProperty("expire")]
		public int Expire { get; set; }

		/// <summary>
		/// The time to live (TTL) for negative caching of records within the zone.
		/// </summary>
		/// <value>Unit: seconds. Range: <c>60 &lt;=</c> X <c>&lt;= 86400</c></value>
		[JsonProperty("min_ttl")]
		public int MinimumTtl { get; set; }

		/// <summary>
		/// The primary nameserver, which may be used for outbound zone transfers.
		/// </summary>
		[JsonProperty("mname", NullValueHandling = NullValueHandling.Include)]
		public string PrimaryNameserver { get; set; }

		/// <summary>
		/// Time in seconds after which secondary servers should re-check the SOA record to see if the zone has been updated.
		/// </summary>
		/// <value>Unit: seconds. Range: <c>600 &lt;=</c> X <c>&lt;= 86400</c></value>
		[JsonProperty("refresh")]
		public int Refresh { get; set; }

		/// <summary>
		/// Time in seconds after which secondary servers should retry queries after the primary server was unresponsive.
		/// </summary>
		/// <value>Unit: seconds. Range: <c>600 &lt;=</c> X <c>&lt;= 86400</c></value>
		[JsonProperty("retry")]
		public int Retry { get; set; }

		/// <summary>
		/// The email address of the zone administrator, with the first label representing the local part of the email address.
		/// </summary>
		/// <remarks>
		/// The first dot is interpreted as @ sign.
		/// <br/>
		/// admin.example.com => admin@example.com
		/// <br/>
		/// test\.user.example.org => test.user@example.org
		/// </remarks>
		[JsonProperty("rname", NullValueHandling = NullValueHandling.Include)]
		public string ZoneAdministrator { get; set; }

		/// <summary>
		/// The time to live (TTL) of the SOA record itself.
		/// </summary>
		/// <value>Unit: seconds. Range: <c>300 &lt;=</c> X <c>&lt;= 86400</c></value>
		[JsonProperty("ttl")]
		public int Ttl { get; set; }
	}
}
