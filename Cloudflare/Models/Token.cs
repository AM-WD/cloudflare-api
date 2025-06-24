using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// A token.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L788">Source</see>
	/// </summary>
	public class Token
	{
		/// <summary>
		/// Token identifier tag.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// Token condition.
		/// </summary>
		[JsonProperty("condition")]
		public TokenCondition? Condition { get; set; }

		/// <summary>
		/// The expiration time on or after which the JWT MUST NOT be accepted for processing.
		/// </summary>
		[JsonProperty("created_on")]
		public DateTime? ExpiresOn { get; set; }

		/// <summary>
		/// The time on which the token was created.
		/// </summary>
		public DateTime? IssuedOn { get; set; }

		/// <summary>
		/// Last time the token was used.
		/// </summary>
		public DateTime? LastUsedOn { get; set; }

		/// <summary>
		/// Last time the token was modified.
		/// </summary>
		public DateTime? ModifiedOn { get; set; }

		/// <summary>
		/// Token name.
		/// </summary>
		public string? Name { get; set; }

		/// <summary>
		/// The time before which the token MUST NOT be accepted for processing.
		/// </summary>
		public DateTime? NotBefore { get; set; }

		/// <summary>
		/// List of access policies assigned to the token.
		/// </summary>
		public IReadOnlyCollection<TokenPolicy>? Policies { get; set; }

		/// <summary>
		/// Status of the token.
		/// </summary>
		public TokenStatus? Status { get; set; }
	}

	/// <summary>
	/// Token condition.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L839">Source</see>
	/// </summary>
	public class TokenCondition
	{
		/// <summary>
		/// Client IP restrictions.
		/// </summary>
		[JsonProperty("request_ip")]
		public TokenConditionRequestIP? RequestIp { get; set; }
	}

	/// <summary>
	/// Client IP restrictions.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L850">Source</see>
	/// </summary>
	public class TokenConditionRequestIP
	{
		/// <summary>
		/// List of IPv4/IPv6 CIDR addresses.
		/// </summary>
		[JsonProperty("in")]
		public IReadOnlyCollection<string>? Allowed { get; set; }

		/// <summary>
		/// List of IPv4/IPv6 CIDR addresses.
		/// </summary>
		[JsonProperty("not_in")]
		public IReadOnlyCollection<string>? Denied { get; set; }
	}

	/// <summary>
	/// Status of the token.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L835">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum TokenStatus
	{
		/// <summary>
		/// The token is active.
		/// </summary>
		[EnumMember(Value = "active")]
		Active = 1,

		/// <summary>
		/// The token is disabled.
		/// </summary>
		[EnumMember(Value = "disabled")]
		Disabled = 2,

		/// <summary>
		/// The token is expired.
		/// </summary>
		[EnumMember(Value = "expired")]
		Expired = 3
	}
}
