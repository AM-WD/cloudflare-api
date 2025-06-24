namespace AMWD.Net.Api.Cloudflare.Zones.Internals
{
	internal class InternalUpdateDomainRequest
	{
		[JsonProperty("auto_renew")]
		public bool? AutoRenew { get; set; }

		[JsonProperty("locked")]
		public bool? Locked { get; set; }

		[JsonProperty("privacy")]
		public bool? Privacy { get; set; }
	}
}
