namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Canonical Name record.
	/// </summary>
	public class CNAMERecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CNAMERecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public CNAMERecord(string name)
			: base(name)
		{
			Type = DnsRecordType.CNAME;
		}

		/// <summary>
		/// Settings for the DNS record.
		/// </summary>
		[JsonProperty("settings")]
		public new CNAMERecordSettings? Settings { get; set; }

		/// <inheritdoc/>
		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public override string ToString()
		{
			return $"{Name}  {TimeToLive}  IN  CNAME  {Content}";
		}
	}

	/// <summary>
	/// Settings for the DNS record.
	/// </summary>
	public class CNAMERecordSettings : DnsRecordSettings
	{
		/// <summary>
		/// If enabled, causes the CNAME record to be resolved externally and the resulting
		/// address records (e.g., A and AAAA) to be returned instead of the CNAME record
		/// itself.
		/// </summary>
		/// <remarks>
		/// This setting is unavailable for proxied records, since they are always flattened.
		/// </remarks>
		[JsonProperty("flatten_cname")]
		public bool? FlattenCname { get; set; }
	}
}
