using System;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// A zone hold.
	/// </summary>
	public class ZoneHold
	{
		/// <summary>
		/// Gets or sets a value indicating whether the zone is on hold.
		/// </summary>
		[JsonProperty("hold")]
		public bool Hold { get; set; }

		/// <summary>
		/// Gets or sets an information whether subdomains are included in the hold.
		/// </summary>
		[JsonProperty("include_subdomains")]
		public string IncludeSubdomains { get; set; }

		/// <summary>
		/// Gets or sets the time after which the zone is no longer on hold.
		/// </summary>
		[JsonProperty("hold_after")]
		public DateTime HoldAfter { get; set; }
	}
}
