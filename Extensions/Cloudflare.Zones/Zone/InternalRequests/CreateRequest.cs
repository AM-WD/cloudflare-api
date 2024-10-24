namespace AMWD.Net.Api.Cloudflare.Zones.Zones.InternalRequests
{
	internal class CreateRequest
	{
		[JsonProperty("account")]
		public AccountBase Account { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("type")]
		public ZoneType Type { get; set; }
	}
}
