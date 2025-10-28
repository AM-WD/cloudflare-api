namespace AMWD.Net.Api.Cloudflare.Dns.Internals
{
	internal class InternalCreatePeerRequest
	{
		[JsonProperty("name")]
		public string? Name { get; set; }
	}
}
