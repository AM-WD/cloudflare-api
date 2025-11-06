namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents the response of reverse DNS records of a firewall cluster.
	/// </summary>
	public class ReverseDnsResponse
	{
		/// <summary>
		/// Map of cluster IP addresses to PTR record contents.
		/// </summary>
		[JsonProperty("ptr")]
		public IReadOnlyDictionary<string, string>? ReverseDNS { get; set; }
	}
}
