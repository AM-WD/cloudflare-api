using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Field to order zones by.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ZonesOrderBy
	{
		/// <summary>
		/// Order by zone name.
		/// </summary>
		[EnumMember(Value = "name")]
		Name = 1,

		/// <summary>
		/// Order by zone status.
		/// </summary>
		[EnumMember(Value = "status")]
		Status = 2,

		/// <summary>
		/// Order by account ID.
		/// </summary>
		[EnumMember(Value = "account.id")]
		AccountId = 3,

		/// <summary>
		/// Order by account name.
		/// </summary>
		[EnumMember(Value = "account.name")]
		AccountName = 4,
	}
}
