namespace AMWD.Net.Api.Cloudflare.Dns.Internals
{
	internal class InternalModifyInternalDnsViewRequest
	{
		[JsonProperty("name")]
		public string? Name { get; set; }

		[JsonProperty("zones")]
		public IReadOnlyCollection<string>? Zones { get; set; }
	}
}
