namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// If enabled, any <c>http://</c> URL is converted to <c>https://</c> through a 301
	/// redirect.
	/// </summary>
	public class AlwaysUseHTTPS : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AlwaysUseHTTPS"/> class.
		/// </summary>
		public AlwaysUseHTTPS()
		{
			Id = ZoneSettingId.AlwaysUseHTTPS;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState? Value { get; set; }
	}
}
