namespace AMWD.Net.Api.Cloudflare.Dns.Internals
{
	internal class InternalBatchRequest
	{
		[JsonProperty("deletes")]
		public IReadOnlyCollection<Identifier>? Deletes { get; set; }

		[JsonProperty("patches")]
		public IReadOnlyCollection<InternalBatchUpdateRequest>? Patches { get; set; }

		[JsonProperty("posts")]
		public IReadOnlyCollection<InternalDnsRecordRequest>? Posts { get; set; }

		[JsonProperty("puts")]
		public IReadOnlyCollection<InternalBatchUpdateRequest>? Puts { get; set; }
	}

	internal class InternalBatchUpdateRequest : InternalDnsRecordRequest
	{
		[JsonProperty("id")]
		public string? Id { get; set; }
	}
}
