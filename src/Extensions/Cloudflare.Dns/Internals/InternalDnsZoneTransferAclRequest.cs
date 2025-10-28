namespace AMWD.Net.Api.Cloudflare.Dns
{
	internal class InternalDnsZoneTransferAclRequest
	{
		[JsonProperty("ip_range")]
		public string? IpRange { get; set; }

		[JsonProperty("name")]
		public string? Name { get; set; }
	}
}
