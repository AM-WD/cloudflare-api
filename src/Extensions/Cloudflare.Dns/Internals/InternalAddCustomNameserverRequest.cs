namespace AMWD.Net.Api.Cloudflare.Dns.Internals
{
	internal class InternalAddCustomNameserverRequest
	{
		[JsonProperty("ns_name")]
		public string? NameserverName { get; set; }

		[JsonProperty("ns_set")]
		public int? NameserverSet { get; set; }
	}
}
