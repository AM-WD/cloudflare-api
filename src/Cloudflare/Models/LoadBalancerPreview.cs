namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// A load balancer preview.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L273">Source</see>
	/// </summary>
	public class LoadBalancerPreview
	{
		/// <summary>
		/// Monitored pool IDs mapped to their respective names.
		/// </summary>
		[JsonProperty("pools")]
		public IDictionary<string, string>? Pools { get; set; }

		/// <summary>
		/// Preview ID.
		/// </summary>
		[JsonProperty("preview_id")]
		public string? PreviewId { get; set; }
	}
}
