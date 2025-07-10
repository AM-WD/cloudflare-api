namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Turn on or off <strong>Email Obfuscation</strong>.
	/// </summary>
	public class EmailObfuscation : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EmailObfuscation"/> class.
		/// </summary>
		public EmailObfuscation()
		{
			Id = ZoneSettingId.EmailObfuscation;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState? Value { get; set; }
	}
}
