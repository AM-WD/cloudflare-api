using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones.Internals.Requests
{
	internal class InternalDnsRecordRequest
	{
		public InternalDnsRecordRequest(DnsRecordType type, string name)
		{
			Type = type;
			Name = name;
		}

		[JsonProperty("comment")]
		public string? Comment { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("proxied")]
		public bool Proxied { get; set; }

		[JsonProperty("settings")]
		public object? Settings { get; set; }

		[JsonProperty("tags")]
		public IList<string>? Tags { get; set; }

		[JsonProperty("ttl")]
		public int? Ttl { get; set; }

		[JsonProperty("data")]
		public object? Data { get; set; }

		[JsonProperty("content")]
		public string? Content { get; set; }

		[JsonProperty("priority")]
		public ushort? Priority { get; set; }

		[JsonProperty("type")]
		public DnsRecordType Type { get; set; }
	}
}
