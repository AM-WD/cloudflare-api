namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Turn on or off whether Cloudflare should wait for an entire file from the origin
	/// server before forwarding it to the site visitor. By default, Cloudflare sends
	/// packets to the client as they arrive from the origin server.
	/// </summary>
	public class ResponseBuffering : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ResponseBuffering"/> class.
		/// </summary>
		public ResponseBuffering()
		{
			Id = ZoneSettingId.ResponseBuffering;
		}

		/// <summary>
		/// The status of Response Buffering.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState? Value { get; set; }
	}
}
