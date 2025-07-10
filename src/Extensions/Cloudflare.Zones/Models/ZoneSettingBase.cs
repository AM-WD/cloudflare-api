namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// A Cloudflare zone setting.
	/// </summary>
	public abstract class ZoneSettingBase
	{
		/// <summary>
		/// The ID of the zone setting.
		/// </summary>
		[JsonProperty("id")]
		public ZoneSettingId? Id { get; protected set; }

		/// <summary>
		/// Whether or not this setting can be modified for this zone (based on your Cloudflare plan level).
		/// </summary>
		[JsonProperty("editable")]
		public bool? Editable { get; set; }

		/// <summary>
		/// The last time this setting was modified.
		/// </summary>
		[JsonProperty("modified_on")]
		public DateTime? ModifiedOn { get; set; }
	}
}
