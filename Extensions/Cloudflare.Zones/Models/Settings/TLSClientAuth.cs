namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// TLS Client Auth requires Cloudflare to connect to your origin server using a
	/// client certificate (Enterprise Only).
	/// </summary>
	public class TLSClientAuth : ZoneSettingBase
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="TLSClientAuth"/> class.
		/// </summary>
		public TLSClientAuth()
		{
			Id = ZoneSettingId.TLSClientAuth;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
