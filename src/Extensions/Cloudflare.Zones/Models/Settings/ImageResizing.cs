namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Image Transformations provides on-demand resizing, conversion and optimization
	/// for images served through Cloudflare's network. Refer to the
	/// <see href="https://developers.cloudflare.com/images/">Image Transformations documentation</see>
	/// for more information.
	/// </summary>
	public class ImageResizing : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ImageResizing"/> class.
		/// </summary>
		public ImageResizing()
		{
			Id = ZoneSettingId.ImageResizing;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffOpenState Value { get; set; }
	}
}
