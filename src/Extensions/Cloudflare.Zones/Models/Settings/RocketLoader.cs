namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Turn on or off Rocket Loader in the Cloudflare Speed app.
	/// </summary>
	public class RocketLoader : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RocketLoader"/> class.
		/// </summary>
		public RocketLoader()
		{
			Id = ZoneSettingId.RocketLoader;
		}

		/// <summary>
		/// The status of Rocket Loader.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState? Value { get; set; }
	}
}
