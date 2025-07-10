namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Enable Network Error Logging reporting on your zone. (Beta)
	/// </summary>
	public class NEL : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NEL"/> class.
		/// </summary>
		public NEL()
		{
			Id = ZoneSettingId.NEL;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public NELValue? Value { get; set; }
	}

	/// <summary>
	/// Current value of the zone setting.
	/// </summary>
	public class NELValue
	{
		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("enabled")]
		public bool? Enabled { get; set; }
	}
}
