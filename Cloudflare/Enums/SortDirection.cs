using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// The direction to sort the entity.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum SortDirection
	{
		/// <summary>
		/// Sort in ascending order.
		/// </summary>
		[EnumMember(Value = "asc")]
		Ascending = 1,

		/// <summary>
		/// Sort in descending order.
		/// </summary>
		[EnumMember(Value = "desc")]
		Descending = 2
	}
}
