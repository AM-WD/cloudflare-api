using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// The direction to order the entity.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum OrderDirection
	{
		/// <summary>
		/// Order in ascending order.
		/// </summary>
		[EnumMember(Value = "asc")]
		Asc = 1,

		/// <summary>
		/// Order in descending order.
		/// </summary>
		[EnumMember(Value = "desc")]
		Desc = 2
	}
}
