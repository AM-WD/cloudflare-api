using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Control options for the SSL feature of the Edge Certificates tab in the
	/// Cloudflare SSL/TLS app.
	/// </summary>
	public class SSL : ZoneSettingBase
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="SSL"/> class.
		/// </summary>
		public SSL()
		{
			Id = ZoneSettingId.SSL;
		}

		/// <summary>
		/// The encryption mode that Cloudflare uses to connect to your origin server.
		/// </summary>
		[JsonProperty("value")]
		public SslMode? Value { get; set; }
	}

	/// <summary>
	/// SSL encryption modes.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/settings.ts#L1307">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum SslMode
	{
		/// <summary>
		/// No encryption applied.
		/// Turning off SSL disables HTTPS and causes browsers to show a warning that your website is not secure.
		/// </summary>
		[EnumMember(Value = "off")]
		Off = 1,

		/// <summary>
		/// Enable encryption only between your visitors and Cloudflare.
		/// This will avoid browser security warnings, but all connections between Cloudflare and your origin are made through HTTP.
		/// </summary>
		[EnumMember(Value = "flexible")]
		Flexible = 2,

		/// <summary>
		/// Enable encryption end-to-end.
		/// Use this mode when your origin server supports SSL certification but does not use a valid, publicly trusted certificate.
		/// </summary>
		[EnumMember(Value = "full")]
		Full = 3,

		/// <summary>
		/// Enable encryption end-to-end and enforce validation on origin certificates.
		/// Use Cloudflare’s Origin CA to generate certificates for your origin.
		/// </summary>
		[EnumMember(Value = "strict")]
		Strict = 4,

		/// <summary>
		/// Enforce encryption between Cloudflare and your origin.
		/// Use this mode to guarantee connections to your origin will always be encrypted, regardless of your visitor’s request.
		/// </summary>
		[EnumMember(Value = "origin_pull")]
		OriginPull = 5
	}
}
