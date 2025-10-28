namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to update an existing peer with new configuration details.
	/// </summary>
	public class UpdatePeerRequest : CreatePeerRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UpdatePeerRequest"/> class.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="peerId">The peer identifier.</param>
		/// <param name="name">The name of the peer</param>
		public UpdatePeerRequest(string accountId, string peerId, string name)
			: base(accountId, name)
		{
			PeerId = peerId;
		}

		/// <summary>
		/// The peer identifier.
		/// </summary>
		public string PeerId { get; set; }

		/// <summary>
		/// IPv4/IPv6 address of primary or secondary nameserver, depending on what zone this peer is linked to.
		/// <list type="bullet">
		/// <item>For primary zones this IP defines the IP of the secondary nameserver Cloudflare will NOTIFY upon zone changes.</item>
		/// <item>For secondary zones this IP defines the IP of the primary nameserver Cloudflare will send AXFR/IXFR requests to.</item>
		/// </list>
		/// </summary>
		public string? IpAddress { get; set; }

		/// <summary>
		/// Enable IXFR transfer protocol, default is AXFR. Only applicable to secondary zones.
		/// </summary>
		public bool? IXFREnable { get; set; }

		/// <summary>
		/// DNS port of primary or secondary nameserver, depending on what zone this peer is linked to.
		/// </summary>
		public int? Port { get; set; }

		/// <summary>
		/// TSIG authentication will be used for zone transfer if configured.
		/// </summary>
		public string? TSIGId { get; set; }
	}
}
