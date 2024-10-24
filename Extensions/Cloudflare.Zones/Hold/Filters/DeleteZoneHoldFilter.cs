using System;
using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	internal class DeleteZoneHoldFilter : IQueryParameterFilter
	{
		public DateTime? HoldAfter { get; set; }

		public IDictionary<string, string> GetQueryParameters()
		{
			var dict = new Dictionary<string, string>();

			if (HoldAfter.HasValue)
				dict.Add("hold_after", HoldAfter.Value.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"));

			return dict;
		}
	}
}
