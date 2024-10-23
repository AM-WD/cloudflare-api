using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// Whether to match all search requirements or at least one (any).
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum FilterMatchType
	{
		/// <summary>
		/// Match all search requirements.
		/// </summary>
		[EnumMember(Value = "all")]
		All = 1,

		/// <summary>
		/// Match at least one search requirement.
		/// </summary>
		[EnumMember(Value = "any")]
		Any = 2,
	}
}
