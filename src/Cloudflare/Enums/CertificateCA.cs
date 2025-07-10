using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// The Certificate Authority that will issue the certificate.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L110">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum CertificateCA
	{
		/// <summary>
		/// DigiCert.
		/// </summary>
		[EnumMember(Value = "digicert")]
		DigiCert = 1,

		/// <summary>
		/// Google.
		/// </summary>
		[EnumMember(Value = "google")]
		Google = 2,

		/// <summary>
		/// Let's Encrypt.
		/// </summary>
		[EnumMember(Value = "lets_encrypt")]
		LetsEncrypt = 3,

		/// <summary>
		/// SSL.com.
		/// </summary>
		[EnumMember(Value = "ssl_com")]
		SslCom = 4
	}
}
