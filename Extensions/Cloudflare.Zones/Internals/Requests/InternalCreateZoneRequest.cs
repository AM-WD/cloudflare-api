namespace AMWD.Net.Api.Cloudflare.Zones.Internals.Requests
{
	internal class InternalCreateZoneRequest
	{
		public InternalCreateZoneRequest(AccountBase account, string name)
		{
			Account = account;
			Name = name;
		}

		[JsonProperty("account")]
		public AccountBase Account { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("type")]
		public ZoneType? Type { get; set; }
	}
}
