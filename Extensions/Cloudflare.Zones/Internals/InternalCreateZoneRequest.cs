namespace AMWD.Net.Api.Cloudflare.Zones.Internals
{
	internal class InternalCreateZoneRequest
	{
		public InternalCreateZoneRequest(string? accountId, string name, ZoneType? type)
		{
			Account = new Identifier { Id = accountId };

			Name = name;
			Type = type;
		}

		[JsonProperty("account")]
		public Identifier Account { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("type")]
		public ZoneType? Type { get; set; }
	}
}
