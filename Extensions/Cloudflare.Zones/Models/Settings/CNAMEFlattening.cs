namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// How to flatten the cname destination.
	/// </summary>
	[Obsolete("Please use the DNS Settings route instead.")]
	public class CNAMEFlattening : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CNAMEFlattening"/> class.
		/// </summary>
		public CNAMEFlattening()
		{
			Id = ZoneSettingId.CNAMEFlattening;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[Obsolete("Please use the DNS Settings route instead.")]
		[JsonProperty("value")]
		public string? Value { get; set; }
	}
}
