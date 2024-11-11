namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// A nameserver.
	/// </summary>
	public class Nameserver
	{
		/// <summary>
		/// The nameserver type.
		/// </summary>
		[JsonProperty("type")]
		public NameserverType Type { get; set; }

		// TODO: DEPRECATED? - not available on API request.
		///// <summary>
		///// Configured nameserver set to be used for this zone.
		///// </summary>
		///// <value>Range: <c>1 &lt;=</c> X <c>&lt;= 5</c></value>
		//[JsonProperty("ns_set")]
		//public int NameserverSet { get; set; }
	}
}
