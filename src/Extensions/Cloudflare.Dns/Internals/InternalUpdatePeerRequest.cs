namespace AMWD.Net.Api.Cloudflare.Dns.Internals
{
	internal class InternalUpdatePeerRequest : InternalCreatePeerRequest
	{
		[JsonProperty("ip")]
		public string? Ip { get; set; }

		[JsonProperty("ixfr_enable")]
		public bool? IxfrEnable { get; set; }

		[JsonProperty("port")]
		public int? Port { get; set; }

		[JsonProperty("tsig_id")]
		public string? TSigId { get; set; }
	}
}
