namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// WebSockets are open connections sustained between the client and the origin
	/// server. Inside a WebSockets connection, the client and the origin can pass data
	/// back and forth without having to reestablish sessions. This makes exchanging
	/// data within a WebSockets connection fast. WebSockets are often used for real-time
	/// applications such as live chat and gaming. For more information refer to
	/// <see href="https://support.cloudflare.com/hc/en-us/articles/200169466-Can-I-use-Cloudflare-with-WebSockets-">Can I use Cloudflare with Websockets</see>.
	/// </summary>
	public class Websocket : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Websocket"/> class.
		/// </summary>
		public Websocket()
		{
			Id = ZoneSettingId.Websocket;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
