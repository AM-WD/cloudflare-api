using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// The rate plan applied to the subscription.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L503">Source</see>
	/// </summary>
	public class RatePlan
	{
		/// <summary>
		/// The ID of the rate plan.
		/// </summary>
		[JsonProperty("id")]
		public RatePlanId? Id { get; set; }

		/// <summary>
		/// The currency applied to the rate plan subscription.
		/// </summary>
		[JsonProperty("currency")]
		public string? Currency { get; set; }

		/// <summary>
		/// Whether this rate plan is managed externally from Cloudflare.
		/// </summary>
		[JsonProperty("externally_managed")]
		public bool? ExternallyManaged { get; set; }

		/// <summary>
		/// Whether a rate plan is enterprise-based (or newly adopted term contract).
		/// </summary>
		[JsonProperty("is_contract")]
		public bool? IsContract { get; set; }

		/// <summary>
		/// The full name of the rate plan.
		/// </summary>
		[JsonProperty("public_name")]
		public string? PublicName { get; set; }

		/// <summary>
		/// The scope that this rate plan applies to.
		/// </summary>
		[JsonProperty("scope")]
		public string? Scope { get; set; }

		/// <summary>
		/// The list of sets this rate plan applies to.
		/// </summary>
		[JsonProperty("sets")]
		public IReadOnlyCollection<string>? Sets { get; set; }
	}

	/// <summary>
	/// Available rate plan ids.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L507">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum RatePlanId
	{
		/// <summary>
		/// The free rate plan.
		/// </summary>
		[EnumMember(Value = "free")]
		Free = 1,

		/// <summary>
		/// The lite rate plan.
		/// </summary>
		[EnumMember(Value = "lite")]
		Lite = 2,

		/// <summary>
		/// The pro rate plan.
		/// </summary>
		[EnumMember(Value = "pro")]
		Pro = 3,

		/// <summary>
		/// The pro+ rate plan.
		/// </summary>
		[EnumMember(Value = "pro_plus")]
		ProPlus = 4,

		/// <summary>
		/// The business rate plan.
		/// </summary>
		[EnumMember(Value = "business")]
		Business = 5,

		/// <summary>
		/// The enterprise rate plan.
		/// </summary>
		[EnumMember(Value = "enterprise")]
		Enterprise = 6,

		/// <summary>
		/// The partners free rate plan.
		/// </summary>
		[EnumMember(Value = "partners_free")]
		PartnersFree = 7,

		/// <summary>
		/// The partners pro rate plan.
		/// </summary>
		[EnumMember(Value = "partners_pro")]
		PartnersPro = 8,

		/// <summary>
		/// The partners business rate plan.
		/// </summary>
		[EnumMember(Value = "partners_business")]
		PartnersBusiness = 9,

		/// <summary>
		/// The partners enterprise rate plan.
		/// </summary>
		[EnumMember(Value = "partners_enterprise")]
		PartnersEnterprise = 10
	}
}
