using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Only accepts HTTPS requests that use at least the TLS protocol version
	/// specified. For example, if TLS 1.1 is selected, TLS 1.0 connections will be
	/// rejected, while 1.1, 1.2, and 1.3 (if enabled) will be permitted.
	/// </summary>
	public class MinTLSVersion : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MinTLSVersion"/> class.
		/// </summary>
		public MinTLSVersion()
		{
			Id = ZoneSettingId.MinTLSVersion;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		public TlsVersion? Value { get; set; }
	}

	/// <summary>
	/// Available TLS versions.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/settings.ts#L785">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TlsVersion
	{
		/// <summary>
		/// TLS 1.0
		/// </summary>
		[EnumMember(Value = "1.0")]
		Tls10 = 1,

		/// <summary>
		/// TLS 1.1
		/// </summary>
		[EnumMember(Value = "1.1")]
		Tls11 = 2,

		/// <summary>
		/// TLS 1.2
		/// </summary>
		[EnumMember(Value = "1.2")]
		Tls12 = 3,

		/// <summary>
		/// TLS 1.3
		/// </summary>
		[EnumMember(Value = "1.3")]
		Tls13 = 4
	}
}
