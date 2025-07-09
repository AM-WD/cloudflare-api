namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Enrollment in the SSL/TLS Recommender service which tries to detect and
	/// recommend (by sending periodic emails) the most secure SSL/TLS setting your
	/// origin servers support.
	/// </summary>
	[Obsolete("SSL/TLS Recommender has been decommissioned in favor of Automatic SSL/TLS: ssl_automatic_mode")]
	public class SSLRecommender : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SSLRecommender"/> class.
		/// </summary>
		public SSLRecommender()
		{
			Id = ZoneSettingId.SSLRecommender;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public string? Value { get; set; }
	}
}
