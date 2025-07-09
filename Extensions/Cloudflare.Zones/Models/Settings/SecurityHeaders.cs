namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Cloudflare security header for a zone.
	/// </summary>
	public class SecurityHeaders : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SecurityHeaders"/> class.
		/// </summary>
		public SecurityHeaders()
		{
			Id = ZoneSettingId.SecurityHeaders;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public SecurityHeaderValue? Value { get; set; }
	}

	/// <summary>
	/// Current value of the zone setting.
	/// </summary>
	public class SecurityHeaderValue
	{
		/// <summary>
		/// Strict Transport Security.
		/// </summary>
		[JsonProperty("strict_transport_security")]
		public SecurityHeaderStrictTransportSecurity? StrictTransportSecurity { get; set; }
	}

	/// <summary>
	/// Strict Transport Security.
	/// </summary>
	public class SecurityHeaderStrictTransportSecurity
	{
		/// <summary>
		/// Whether or not strict transport security is enabled.
		/// </summary>
		[JsonProperty("enabled")]
		public bool? Enabled { get; set; }

		/// <summary>
		/// Include all subdomains for strict transport security.
		/// </summary>
		[JsonProperty("include_subdomains")]
		public bool? IncludeSubdomains { get; set; }

		/// <summary>
		/// Max age in seconds of the strict transport security.
		/// </summary>
		[JsonProperty("max_age")]
		public int? MaxAge { get; set; }

		/// <summary>
		/// Whether or not to include <c>X-Content-Type-Options: nosniff</c> header.
		/// </summary>
		[JsonProperty("nosniff")]
		public bool? NoSniff { get; set; }

		/// <summary>
		/// Enable automatic preload of the HSTS configuration.
		/// </summary>
		[JsonProperty("preload")]
		public bool? Preload { get; set; }
	}
}
