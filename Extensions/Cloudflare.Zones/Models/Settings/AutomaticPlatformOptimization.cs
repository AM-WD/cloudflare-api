namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Apply custom caching based on the option selected.
	/// </summary>
	public class AutomaticPlatformOptimization
	{
		/// <summary>
		/// Indicates whether or not
		/// <see href="https://developers.cloudflare.com/automatic-platform-optimization/reference/cache-device-type/">cache by device type</see>
		/// is enabled.
		/// </summary>
		[JsonProperty("cache_by_device_type")]
		public bool? CacheByDeviceType { get; set; }

		/// <summary>
		/// Indicates whether or not Cloudflare proxy is enabled.
		/// </summary>
		[JsonProperty("cf")]
		public bool? CloudflareProxyEnabled { get; set; }

		/// <summary>
		/// Indicates whether or not Automatic Platform Optimization is enabled.
		/// </summary>
		[JsonProperty("enabled")]
		public bool? Enabled { get; set; }

		/// <summary>
		/// An array of hostnames where Automatic Platform Optimization for WordPress is activated.
		/// </summary>
		[JsonProperty("hostnames")]
		public IReadOnlyCollection<string>? Hostnames { get; set; }

		/// <summary>
		/// Indicates whether or not site is powered by WordPress.
		/// </summary>
		[JsonProperty("wordpress")]
		public bool? WordPress { get; set; }

		/// <summary>
		/// Indicates whether or not
		/// <see href="https://wordpress.org/plugins/cloudflare/">Cloudflare for WordPress plugin</see>
		/// is installed.
		/// </summary>
		[JsonProperty("wp_plugin")]
		public bool? WpPlugin { get; set; }
	}
}
