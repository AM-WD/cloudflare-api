namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Time (in seconds) that a resource will be ensured to remain on Cloudflare's
	/// cache servers.
	/// </summary>
	public class SchemasEdgeCacheTTL : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SchemasEdgeCacheTTL"/> class.
		/// </summary>
		public SchemasEdgeCacheTTL()
		{
			Id = ZoneSettingId.SchemasEdgeCacheTTL;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public EdgeCacheTTLValue Value { get; set; }
	}

	/// <summary>
	/// The time-to-live (TTL) of the challenge.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/settings.ts#L1873">Soruce</see>
	/// </summary>
	public enum EdgeCacheTTLValue : int
	{
		/// <summary>
		/// 30 seconds.
		/// </summary>
		HalfMinute = 30,

		/// <summary>
		/// 1 minute.
		/// </summary>
		Minute = 60,

		/// <summary>
		/// 5 minutes.
		/// </summary>
		FiveMinutes = 300,

		/// <summary>
		/// 20 minutes.
		/// </summary>
		TwentyMinutes = 1200,

		/// <summary>
		/// 30 minutes.
		/// </summary>
		HalfHour = 1800,

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
		/// 5 hours.
		/// </summary>
		FiveHours = 18000,

		/// <summary>
		/// 8 hours.
		/// </summary>
		EightHours = 28800,

		/// <summary>
		/// 12 hours.
		/// </summary>
		HalfDay = 43200,

		/// <summary>
		/// 16 hours.
		/// </summary>
		SixteenHours = 57600,

		/// <summary>
		/// 20 hours.
		/// </summary>
		TwentyHours = 72000,

		/// <summary>
		/// 1 day.
		/// </summary>
		Day = 86400,

		/// <summary>
		/// 2 days.
		/// </summary>
		TwoDays = 172800,

		/// <summary>
		/// 3 days.
		/// </summary>
		ThreeDays = 259200,

		/// <summary>
		/// 4 days.
		/// </summary>
		FourDays = 345600,

		/// <summary>
		/// 5 days.
		/// </summary>
		FiveDays = 432000,

		/// <summary>
		/// 6 days.
		/// </summary>
		SixDays = 518400,

		/// <summary>
		/// 1 week.
		/// </summary>
		Week = 604800,
	}
}
