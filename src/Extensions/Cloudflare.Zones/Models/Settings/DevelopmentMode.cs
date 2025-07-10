namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Development Mode temporarily allows you to enter development mode for your
	/// websites if you need to make changes to your site.This will bypass Cloudflare's
	/// accelerated cache and slow down your site, but is useful if you are making
	/// changes to cacheable content (like images, css, or JavaScript) and would like to
	/// see those changes right away. Once entered, development mode will last for 3
	/// hours and then automatically toggle off.
	/// </summary>
	public class DevelopmentMode : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DevelopmentMode"/> class.
		/// </summary>
		public DevelopmentMode()
		{
			Id = ZoneSettingId.DevelopmentMode;
		}

		/// <summary>
		/// Current state of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }

		/// <summary>
		/// The interval (in seconds) from when development mode expires (positive integer)
		/// or last expired (negative integer) for the domain.
		/// If development mode has never been enabled, this value is zero.
		/// </summary>
		[JsonProperty("time_remaining")]
		public int? TimeRemaining { get; set; }
	}
}
