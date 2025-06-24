using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// A frequency at which to renew subscriptions, etc.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L730">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum RenewFrequency
	{
		/// <summary>
		/// Weekly
		/// </summary>
		[EnumMember(Value = "weekly")]
		Weekly = 1,

		/// <summary>
		/// Monthly
		/// </summary>
		[EnumMember(Value = "monthly")]
		Monthly = 2,

		/// <summary>
		/// Quarterly
		/// </summary>
		[EnumMember(Value = "quarterly")]
		Quarterly = 3,

		/// <summary>
		/// Yearly
		/// </summary>
		[EnumMember(Value = "yearly")]
		Yearly = 4
	}
}
