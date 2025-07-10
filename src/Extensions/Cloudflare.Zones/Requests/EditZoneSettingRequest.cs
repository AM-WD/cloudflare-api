namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Represents a request to edit a zone setting.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EditZoneSettingRequest<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EditZoneSettingRequest{T}"/> class.
		/// </summary>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="setting">The zone setting.</param>
		public EditZoneSettingRequest(string zoneId, T setting)
		{
			ZoneId = zoneId;
			Setting = setting;
		}

		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; }

		/// <summary>
		/// The zone setting.
		/// </summary>
		public T Setting { get; set; }

		/// <summary>
		/// Indicates whether the setting should be enabled or disabled.
		/// </summary>
		/// <remarks>
		/// If this property is not <see langword="null"/>,
		/// the value of the <see cref="Setting"/> will not be modified.
		/// </remarks>
		public bool? Enabled { get; set; }
	}
}
