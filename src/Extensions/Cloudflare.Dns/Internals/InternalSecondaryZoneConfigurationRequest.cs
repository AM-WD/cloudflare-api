namespace AMWD.Net.Api.Cloudflare.Dns.Internals
{
	internal class InternalSecondaryZoneConfigurationRequest
	{
		[JsonProperty("name")]
		public string? Name { get; set; }

		[JsonProperty("auto_refresh_seconds")]
		public int? AutoRefreshSeconds { get; set; }

		[JsonProperty("peers")]
		public IReadOnlyCollection<string>? Peers { get; set; }
	}
}
