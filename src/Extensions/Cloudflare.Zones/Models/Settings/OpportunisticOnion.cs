namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Add an Alt-Svc header to all legitimate requests from Tor, allowing the
	/// connection to use our onion services instead of exit nodes.
	/// </summary>
	public class OpportunisticOnion : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OpportunisticOnion"/> class.
		/// </summary>
		public OpportunisticOnion()
		{
			Id = ZoneSettingId.OpportunisticOnion;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState? Value { get; set; }
	}
}
