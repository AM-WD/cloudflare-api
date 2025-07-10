namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Maximum time between two read operations from origin.
	/// </summary>
	public class ProxyReadTimeout : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ProxyReadTimeout"/> class.
		/// </summary>
		public ProxyReadTimeout()
		{
			Id = ZoneSettingId.ProxyReadTimeout;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public int Value { get; set; }
	}
}
