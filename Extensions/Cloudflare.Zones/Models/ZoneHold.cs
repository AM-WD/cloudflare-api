namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// A Cloudflare zone hold.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/holds.ts#L88">Source</see>
	/// </summary>
	public class ZoneHold
	{
		/// <summary>
		/// Whether the zone is on hold.
		/// </summary>
		[JsonProperty("hold")]
		public bool? Hold { get; set; }

		/// <summary>
		/// The hold is enabled if the value is in the past.
		/// </summary>
		[JsonProperty("hold_after")]
		public DateTime? HoldAfter { get; set; }

		/// <summary>
		/// Whether to include subdomains in the hold.
		/// </summary>
		[JsonProperty("include_subdomains")]
		public bool? IncludeSubdomains { get; set; }
	}
}
