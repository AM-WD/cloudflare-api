namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Turn on or off the reordering of query strings. When query strings have the same
	/// structure, caching improves.
	/// </summary>
	public class SortQueryStringForCache : ZoneSettingBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SortQueryStringForCache"/> class.
		/// </summary>
		public SortQueryStringForCache()
		{
			Id = ZoneSettingId.SortQueryStringForCache;
		}

		/// <summary>
		/// The status of Query String Sort.
		/// </summary>
		[JsonProperty("value")]
		public OnOffState? Value { get; set; }
	}
}
