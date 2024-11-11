namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Request to import DNS records from a BIND configuration file.
	/// </summary>
	public class ImportDnsRecordsRequest(string zoneId, string file)
	{
		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; } = zoneId;

		/// <summary>
		/// Whether or not proxiable records should receive the performance and security benefits of Cloudflare.
		/// </summary>
		public bool? Proxied { get; set; }

		/// <summary>
		/// BIND config to import.
		/// </summary>
		/// <remarks>
		/// If the property is an absolute path, the content of that file will be used as the BIND configuration.
		/// <br />
		/// Otherwise the property will be treated as BIND configuration itself.
		/// </remarks>
		public string File { get; set; } = file;
	}
}
