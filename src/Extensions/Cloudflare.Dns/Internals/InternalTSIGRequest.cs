namespace AMWD.Net.Api.Cloudflare.Dns.Internals
{
	internal class InternalTSIGRequest
	{
		[JsonProperty("algo")]
		public TSigAlgorithm? Algorithm { get; set; }

		[JsonProperty("name")]
		public string? Name { get; set; }

		[JsonProperty("secret")]
		public string? Secret { get; set; }
	}
}
