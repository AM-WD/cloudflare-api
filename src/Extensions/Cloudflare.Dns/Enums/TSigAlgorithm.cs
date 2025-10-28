using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Available TSIG algorithms as <see href="https://www.rfc-editor.org/rfc/rfc8945.html#section-6">recommended by IANA</see>.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TSigAlgorithm
	{
		/// <summary>
		/// HMAC SHA1.
		/// </summary>
		/// <remarks>
		/// Implementation: <strong>must</strong>
		/// <br/>
		/// Use: <em>not recommended</em>
		/// </remarks>
		[EnumMember(Value = "hmac-sha1")]
		HMAC_SHA1 = 1,

		/// <summary>
		/// HMAC SHA224.
		/// </summary>
		/// <remarks>
		/// Implementation: <strong>may</strong>
		/// <br/>
		/// Use: <em>may</em>
		/// </remarks>
		[EnumMember(Value = "hmac-sha224")]
		HMAC_SHA224 = 2,

		/// <summary>
		/// HMAC SHA256.
		/// </summary>
		/// <remarks>
		/// Implementation: <strong>must</strong>
		/// <br/>
		/// Use: <em>recommended</em>
		/// </remarks>
		[EnumMember(Value = "hmac-sha256")]
		HMAC_SHA256 = 3,

		/// <summary>
		/// HMAC SHA384.
		/// </summary>
		/// <remarks>
		/// Implementation: <strong>may</strong>
		/// <br/>
		/// Use: <em>may</em>
		/// </remarks>
		[EnumMember(Value = "hmac-sha384")]
		HMAC_SHA384 = 4,

		/// <summary>
		/// HMAC SHA512.
		/// </summary>
		/// <remarks>
		/// Implementation: <strong>may</strong>
		/// <br/>
		/// Use: <em>may</em>
		/// </remarks>
		[EnumMember(Value = "hmac-sha512")]
		HMAC_SHA512 = 5,

		/// <summary>
		/// HMAC SHA256 128.
		/// </summary>
		/// <remarks>
		/// Implementation: <strong>may</strong>
		/// <br/>
		/// Use: <em>may</em>
		/// </remarks>
		[EnumMember(Value = "hmac-sha256-128")]
		HMAC_SHA256_128 = 6,

		/// <summary>
		/// HMAC SHA384 192.
		/// </summary>
		/// <remarks>
		/// Implementation: <strong>may</strong>
		/// <br/>
		/// Use: <em>may</em>
		/// </remarks>
		[EnumMember(Value = "hmac-sha384-192")]
		HMAC_SHA384_192 = 7,

		/// <summary>
		/// HMAC SHA512 256.
		/// </summary>
		/// <remarks>
		/// Implementation: <strong>may</strong>
		/// <br/>
		/// Use: <em>may</em>
		/// </remarks>
		[EnumMember(Value = "hmac-sha512-256")]
		HMAC_SHA512_256 = 8,
	}
}
