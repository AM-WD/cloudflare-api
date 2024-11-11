using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones.Internals.Requests
{
	internal class InternalBatchRequest
	{
		[JsonProperty("deletes")]
		public IList<InternalDnsRecordId>? Deletes { get; set; }

		[JsonProperty("patches")]
		public IList<InternalBatchUpdateRequest>? Patches { get; set; }

		[JsonProperty("puts")]
		public IList<InternalBatchUpdateRequest>? Puts { get; set; }

		[JsonProperty("posts")]
		public IList<InternalDnsRecordRequest>? Posts { get; set; }
	}

	internal class InternalDnsRecordId
	{
		[JsonProperty("id")]
		public string? Id { get; set; }
	}

	internal class InternalBatchUpdateRequest(string id, InternalDnsRecordRequest baseInstance)
	{
		[JsonProperty("id")]
		public string Id { get; set; } = id;

		[JsonProperty("comment")]
		public string? Comment { get; set; } = baseInstance.Comment;

		[JsonProperty("name")]
		public string Name { get; set; } = baseInstance.Name;

		[JsonProperty("proxied")]
		public bool Proxied { get; set; } = baseInstance.Proxied;

		[JsonProperty("settings")]
		public object? Settings { get; set; } = baseInstance.Settings;

		[JsonProperty("tags")]
		public IList<string>? Tags { get; set; } = baseInstance.Tags;

		[JsonProperty("ttl")]
		public int? Ttl { get; set; } = baseInstance.Ttl;

		[JsonProperty("data")]
		public object? Data { get; set; } = baseInstance.Data;

		[JsonProperty("content")]
		public string? Content { get; set; } = baseInstance.Content;

		[JsonProperty("priority")]
		public ushort? Priority { get; set; } = baseInstance.Priority;

		[JsonProperty("type")]
		public DnsRecordType Type { get; set; } = baseInstance.Type;
	}
}
