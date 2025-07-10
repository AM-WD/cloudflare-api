namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Privacy Pass.
	/// </summary>
	[Obsolete("Privacy Pass v1 was deprecated in 2023.")]
	public class PrivacyPass : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PrivacyPass"/> class.
		/// </summary>
		public PrivacyPass()
		{
			Id = ZoneSettingId.PrivacyPass;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState Value { get; set; }
	}
}
