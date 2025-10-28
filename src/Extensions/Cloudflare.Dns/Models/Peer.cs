namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// A Cloudflare peer.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/dns/zone-transfers/peers.ts#L119">Source</see>
	/// </summary>
	public class Peer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Peer"/> class.
		/// </summary>
		/// <param name="id">The unique identifier.</param>
		/// <param name="name">The name of the peer.</param>
		public Peer(string id, string name)
		{
			Id = id;
			Name = name;
		}

		/// <summary>
		/// The unique identifier.
		/// </summary>
		[JsonProperty("id")]
		public string Id { get; set; }

		/// <summary>
		/// The name of the peer.
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// IPv4/IPv6 address of primary or secondary nameserver, depending on what zone this peer is linked to.
		/// <list type="bullet">
		/// <item>For primary zones this IP defines the IP of the secondary nameserver Cloudflare will NOTIFY upon zone changes.</item>
		/// <item>For secondary zones this IP defines the IP of the primary nameserver Cloudflare will send AXFR/IXFR requests to.</item>
		/// </list>
		/// </summary>
		[JsonProperty("ip")]
		public string? IpAddress { get; set; }

		/// <summary>
		/// Enable IXFR transfer protocol, default is AXFR.
		/// <em>Only applicable to secondary zones.</em>
		/// </summary>
		[JsonProperty("ixfr_enable")]
		public bool? IXFREnabled { get; set; }

		/// <summary>
		/// DNS port of primary or secondary nameserver, depending on what zone this peer is linked to.
		/// </summary>
		[JsonProperty("port")]
		public int? Port { get; set; }

		/// <summary>
		/// TSIG authentication will be used for zone transfer if configured.
		/// </summary>
		[JsonProperty("tsig_id")]
		public string? TSIGId { get; set; }
	}
}
