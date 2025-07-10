namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// When enabled, Cloudflare serves limited copies of web pages available from the
	/// <see href="https://archive.org/web/">Internet Archive's Wayback Machine</see>
	/// if your server is offline.
	/// Refer to <see href="https://developers.cloudflare.com/cache/about/always-online">Always Online</see>
	/// for more information.
	/// </summary>
	public class AlwaysOnline : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AlwaysOnline"/> class.
		/// </summary>
		public AlwaysOnline()
		{
			Id = ZoneSettingId.AlwaysOnline;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
