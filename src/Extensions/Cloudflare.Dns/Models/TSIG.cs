using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a Transaction Signature (TSIG) used for securing DNS messages.
	/// </summary>
	public class TSIG
	{
		/// <summary>
		/// The unique identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// TSIG algorithm.
		/// </summary>
		[JsonProperty("algo")]
		public TSigAlgorithm? Algorithm { get; set; }

		/// <summary>
		/// TSIG key name.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }

		/// <summary>
		/// TSIG secret.
		/// </summary>
		[JsonProperty("secret")]
		public string? Secret { get; set; }
	}
}
