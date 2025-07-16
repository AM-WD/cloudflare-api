namespace AMWD.Net.Api.Cloudflare.Dns.Internals
{
	internal class InternalDnsRecordRequest
	{
		[JsonProperty("comment")]
		public string? Comment { get; set; }

		[JsonProperty("name")]
		public string? Name { get; set; }

		[JsonProperty("proxied")]
		public bool? Proxied { get; set; }

		[JsonProperty("settings")]
		public object? Settings { get; set; }

		[JsonProperty("tags")]
		public IReadOnlyCollection<string>? Tags { get; set; }

		[JsonProperty("ttl")]
		public int? Ttl { get; set; }

		[JsonProperty("content")]
		public string? Content { get; set; }

		[JsonProperty("type")]
		public DnsRecordType Type { get; set; }

		[JsonProperty("priority")]
		public int? Priority { get; set; }

		[JsonProperty("data")]
		public object? Data { get; set; }
	}
}
