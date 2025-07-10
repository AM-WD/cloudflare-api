namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Represents a request to edit multiple zone settings.
	/// </summary>
	public class EditMultipleZoneSettingsRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EditMultipleZoneSettingsRequest"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		public EditMultipleZoneSettingsRequest(string zoneId)
		{
			ZoneId = zoneId;
		}

		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; }

		/// <summary>
		/// The zone settings.
		/// </summary>
		public IReadOnlyCollection<ZoneSettingBase>? Settings { get; set; }
	}
}
