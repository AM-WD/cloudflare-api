using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// The maximum HTTP version supported by the origin server.
	/// </summary>
	public class OriginMaxHTTPVersion : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OriginMaxHTTPVersion"/> class.
		/// </summary>
		public OriginMaxHTTPVersion()
		{
			Id = ZoneSettingId.OriginMaxHTTPVersion;
		}

		/// <summary>
		/// The value of the feature.
		/// </summary>
		[JsonProperty("value")]
		public HttpVersion Value { get; set; }
	}

	/// <summary>
	/// HTTP versions.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/settings.ts#L984">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum HttpVersion
	{
		/// <summary>
		/// HTTP/1
		/// </summary>
		[EnumMember(Value = "1")]
		HTTP1 = 1,

		/// <summary>
		/// HTTP/2
		/// </summary>
		[EnumMember(Value = "2")]
		HTTP2 = 2
	}
}
