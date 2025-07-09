namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// An allowlist of ciphers for TLS termination. These ciphers must be in the
	/// BoringSSL format.
	/// </summary>
	public class Ciphers : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Ciphers"/> class.
		/// </summary>
		public Ciphers()
		{
			Id = ZoneSettingId.Ciphers;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public IReadOnlyCollection<string> Value { get; set; } = [];
	}
}
