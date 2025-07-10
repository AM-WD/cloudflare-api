namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Opportunistic Encryption allows browsers to access HTTP URIs over an encrypted
	/// TLS channel. It's not a substitute for HTTPS, but provides additional security
	/// for otherwise vulnerable requests.
	/// </summary>
	public class OpportunisticEncryption : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OpportunisticEncryption"/> class.
		/// </summary>
		public OpportunisticEncryption()
		{
			Id = ZoneSettingId.OpportunisticEncryption;
		}

		/// <summary>
		/// The status of Opportunistic Encryption.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState? Value { get; set; }
	}
}
