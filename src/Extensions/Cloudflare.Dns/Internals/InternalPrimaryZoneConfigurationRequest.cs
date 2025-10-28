namespace AMWD.Net.Api.Cloudflare.Dns.Internals
{
	internal class InternalPrimaryZoneConfigurationRequest
	{
		[JsonProperty("name")]
		public string? Name { get; set; }

		[JsonProperty("peers")]
		public IReadOnlyCollection<string>? Peers { get; set; }
	}
}
