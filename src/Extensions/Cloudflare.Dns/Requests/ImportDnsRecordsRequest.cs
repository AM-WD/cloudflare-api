namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to import DNS records.
	/// </summary>
	public class ImportDnsRecordsRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ImportDnsRecordsRequest"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		public ImportDnsRecordsRequest(string zoneId)
		{
			ZoneId = zoneId;
		}

		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; }

		/// <summary>
		/// BIND config to import.
		/// </summary>
		public string? File { get; set; }

		/// <summary>
		/// Whether or not proxiable records should receive the performance and
		/// security benefits of Cloudflare.
		/// </summary>
		public bool? Proxied { get; set; }
	}
}
