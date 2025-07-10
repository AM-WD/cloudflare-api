namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Only allows TLS1.2.
	/// </summary>
	public class TLS1_2Only : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TLS1_2Only"/> class.
		/// </summary>
		public TLS1_2Only()
		{
			Id = ZoneSettingId.TLS1_2Only;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
