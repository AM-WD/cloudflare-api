using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/rate-plans.ts#L36">Source</see>
	/// </summary>
	public class RatePlanGetResponse
	{
		/// <summary>
		/// Plan identifier tag.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// Array of available components values for the plan.
		/// </summary>
		[JsonProperty("components")]
		public IReadOnlyCollection<Component>? Components { get; set; }

		/// <summary>
		/// The monetary unit in which pricing information is displayed.
		/// </summary>
		[JsonProperty("currency")]
		public string? Currency { get; set; }

		/// <summary>
		/// The duration of the plan subscription.
		/// </summary>
		[JsonProperty("duration")]
		public int? Duration { get; set; }

		/// <summary>
		/// The frequency at which you will be billed for this plan.
		/// </summary>
		[JsonProperty("frequency")]
		public Frequency? Frequency { get; set; }

		/// <summary>
		/// The plan name.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }

		/// <summary>
		/// Rate plan component.
		/// </summary>
		public class Component
		{
			/// <summary>
			/// The default amount allocated.
			/// </summary>
			[JsonProperty("default")]
			public decimal? Default { get; set; }

			/// <summary>
			/// The unique component.
			/// </summary>
			[JsonProperty("name")]
			public ComponentName? Name { get; set; }

			/// <summary>
			/// The unit price of the addon.
			/// </summary>
			[JsonProperty("unit_price")]
			public decimal? UnitPrice { get; set; }
		}

		/// <summary>
		/// Rate plan component name.
		/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/rate-plans.ts#L78">Source</see>
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public enum ComponentName
		{
			/// <summary>
			/// Zones
			/// </summary>
			[EnumMember(Value = "zones")]
			Zones = 1,

			/// <summary>
			/// Page rules
			/// </summary>
			[EnumMember(Value = "page_rules")]
			PageRules = 2,

			/// <summary>
			/// Dedicated certificates
			/// </summary>
			[EnumMember(Value = "dedicated_certificates")]
			DedicatedCertificatese = 3,

			/// <summary>
			/// Custom dedicated certificates
			/// </summary>
			[EnumMember(Value = "dedicated_certificates_custom")]
			DedicatedCertificatesCustom = 4
		}
	}
}
