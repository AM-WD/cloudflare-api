using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// The nameserver type.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum NameserverType
	{
		/// <summary>
		/// Cloudflare standard.
		/// </summary>
		[EnumMember(Value = "cloudflare.standard")]
		CloudflareStandard = 1,

		/// <summary>
		/// Cloudflare random.
		/// </summary>
		[EnumMember(Value = "cloudflare.standard.random")]
		CloudflareRandom = 2,

		/// <summary>
		/// Custom specified by account.
		/// </summary>
		[EnumMember(Value = "custom.account")]
		CustomAccount = 3,

		/// <summary>
		/// Custom specified by tenant.
		/// </summary>
		[EnumMember(Value = "custom.tenant")]
		CustomTenant = 4,

		/// <summary>
		/// Custom specified by zone.
		/// </summary>
		[EnumMember(Value = "custom.zone")]
		CustomZone = 5,
	}
}
