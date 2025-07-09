using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Apply custom caching based on the option selected.
	/// </summary>
	public class CacheLevel : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CacheLevel"/> class.
		/// </summary>
		public CacheLevel()
		{
			Id = ZoneSettingId.CacheLevel;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public CacheLevelOption? Value { get; set; }
	}

	/// <summary>
	/// Apply custom caching based on the option selected.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/settings.ts#L365">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum CacheLevelOption
	{
		/// <summary>
		/// Cloudflare does not cache.
		/// </summary>
		[EnumMember(Value = "bypass")]
		Bypass = 1,

		/// <summary>
		/// Delivers resources from cache when there is no query string.
		/// </summary>
		[EnumMember(Value = "basic")]
		Basic = 2,

		/// <summary>
		/// Delivers the same resource to everyone independent of the query string.
		/// </summary>
		[EnumMember(Value = "simplified")]
		Simplified = 3,

		/// <summary>
		/// Caches all static content that has a query string.
		/// </summary>
		[EnumMember(Value = "aggressive")]
		Aggressive = 4,

		/// <summary>
		/// Treats all content as static and caches all file types beyond the
		/// <see href="https://developers.cloudflare.com/cache/concepts/default-cache-behavior/#default-cached-file-extensions">Cloudflare default cached content</see>
		/// </summary>
		[EnumMember(Value = "cache_everything")]
		CacheEverything = 5
	}
}
