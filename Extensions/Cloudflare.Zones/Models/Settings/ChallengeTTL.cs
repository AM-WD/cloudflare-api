namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Specify how long a visitor is allowed access to your site after successfully
	/// completing a challenge (such as a CAPTCHA). After the TTL has expired the
	/// visitor will have to complete a new challenge. We recommend a 15 - 45 minute
	/// setting and will attempt to honor any setting above 45 minutes.
	/// (<see href="https://support.cloudflare.com/hc/en-us/articles/200170136"/>).
	/// </summary>
	public class ChallengeTTL : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ChallengeTTL"/> class.
		/// </summary>
		public ChallengeTTL()
		{
			Id = ZoneSettingId.ChallengeTTL;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public ChallengeTTLValue Value { get; set; }
	}

	/// <summary>
	/// The time-to-live (TTL) of the challenge.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/settings.ts#L403">Soruce</see>
	/// </summary>
	public enum ChallengeTTLValue : int
	{
		/// <summary>
		/// 5 minutes.
		/// </summary>
		FiveMinutes = 300,

		/// <summary>
		/// 15 minutes.
		/// </summary>
		FifteenMinutes = 900,

		/// <summary>
		/// 30 minutes.
		/// </summary>
		HalfHour = 1800,

		/// <summary>
		/// 45 minutes.
		/// </summary>
		ThreeQuartersHour = 2700,

		/// <summary>
		/// 1 hour.
		/// </summary>
		Hour = 3600,

		/// <summary>
		/// 2 hours.
		/// </summary>
		TwoHours = 7200,

		/// <summary>
		/// 3 hours.
		/// </summary>
		ThreeHours = 10800,

		/// <summary>
		/// 4 hours.
		/// </summary>
		FourHours = 14400,

		/// <summary>
		/// 8 hours.
		/// </summary>
		EightHours = 28800,

		/// <summary>
		/// 16 hours.
		/// </summary>
		SixteenHours = 57600,

		/// <summary>
		/// 1 day.
		/// </summary>
		Day = 86400,

		/// <summary>
		/// 1 week.
		/// </summary>
		Week = 604800,

		/// <summary>
		/// 30 days.
		/// </summary>
		Month = 2592000,

		/// <summary>
		/// 365 days.
		/// </summary>
		Year = 31536000
	}
}
