using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Options how to match the query filter.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum FilterMatchType
	{
		/// <summary>
		/// Match any rule.
		/// </summary>
		[EnumMember(Value = "any")]
		Any = 1,

		/// <summary>
		/// Match all rules.
		/// </summary>
		[EnumMember(Value = "all")]
		All = 2
	}
}
