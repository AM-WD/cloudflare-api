namespace AMWD.Net.Api.Cloudflare.Dns.Internals
{
	internal class InternalUpdateDNSFirewallClusterReverseDNSRequest
	{
		[JsonProperty("ptr")]
		public IReadOnlyDictionary<string, string>? Ptr { get; set; }
	}
}
