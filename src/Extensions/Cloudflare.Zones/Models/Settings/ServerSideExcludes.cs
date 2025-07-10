namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// If there is sensitive content on your website that you want visible to real
	/// visitors, but that you want to hide from suspicious visitors, all you have to do
	/// is wrap the content with Cloudflare SSE tags. Wrap any content that you want to
	/// be excluded from suspicious visitors in the following SSE tags: <c>&lt;!--sse--&gt;&lt;/!--sse--&gt;</c>.
	/// </summary>
	/// <remarks>
	/// For example: <c>&lt;!--sse--&gt;Bad visitors won't see my phone number, 555-555-5555&lt;/!--sse--&gt;</c>
	/// <br/>
	/// Note: SSE only will work with HTML. If you have HTML minification enabled,
	/// you won't see the SSE tags in your HTML source when it's served through Cloudflare.
	/// SSE will still function in this case, as Cloudflare's HTML minification and SSE
	/// functionality occur on-the-fly as the resource moves through our network to the visitor's computer.
	/// (<see href="https://support.cloudflare.com/hc/en-us/articles/200170036"/>).
	/// </remarks>
	public class ServerSideExcludes : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ServerSideExcludes"/> class.
		/// </summary>
		public ServerSideExcludes()
		{
			Id = ZoneSettingId.ServerSideExcludes;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
