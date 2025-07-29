namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Components of the zone's SOA record.
	/// </summary>
	public class DnsZoneSoa
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DnsZoneSoa"/> class.
		/// </summary>
		/// <param name="expire">Time in seconds of being unable to query the primary server after which secondary servers should stop serving the zone.</param>
		/// <param name="minttl">The time to live (TTL) for negative caching of records within the zone.</param>
		/// <param name="mname">The primary nameserver for the zone.</param>
		/// <param name="refresh">Time in seconds after which secondary servers should re-check the SOA record to see if the zone has been updated.</param>
		/// <param name="retry">The time to live (TTL) for negative caching of records within the zone.</param>
		/// <param name="rname">The email address of the zone administrator.</param>
		/// <param name="ttl">The time to live (TTL) of the SOA record itself.</param>
		public DnsZoneSoa(int expire, int minttl, string mname, int refresh, int retry, string rname, int ttl)
		{
			Expire = expire;
			MinimumTtl = minttl;
			PrimaryNameserver = mname;
			Refresh = refresh;
			Retry = retry;
			ZoneAdministrator = rname;
			TimeToLive = ttl;
		}

		/// <summary>
		/// Time in seconds of being unable to query the primary server after which
		/// secondary servers should stop serving the zone.
		/// </summary>
		[JsonProperty("expire")]
		public int Expire { get; set; }

		/// <summary>
		/// The time to live (TTL) for negative caching of records within the zone.
		/// </summary>
		[JsonProperty("min_ttl")]
		public int MinimumTtl { get; set; }

		/// <summary>
		/// The primary nameserver, which may be used for outbound zone transfers.
		/// </summary>
		[JsonProperty("mname")]
		public string PrimaryNameserver { get; set; }

		/// <summary>
		/// Time in seconds after which secondary servers should re-check the SOA record to
		/// see if the zone has been updated.
		/// </summary>
		[JsonProperty("refresh")]
		public int Refresh { get; set; }

		/// <summary>
		/// Time in seconds after which secondary servers should retry queries after the
		/// primary server was unresponsive.
		/// </summary>
		[JsonProperty("retry")]
		public int Retry { get; set; }

		/// <summary>
		/// The email address of the zone administrator, with the first label representing
		/// the local part of the email address.
		/// </summary>
		[JsonProperty("rname")]
		public string ZoneAdministrator { get; set; }

		/// <summary>
		/// The time to live (TTL) of the SOA record itself.
		/// </summary>
		[JsonProperty("ttl")]
		public int TimeToLive { get; set; }
	}
}
