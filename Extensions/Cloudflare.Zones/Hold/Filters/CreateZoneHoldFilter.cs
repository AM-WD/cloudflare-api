using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	internal class CreateZoneHoldFilter : IQueryParameterFilter
	{
		public bool IncludeSubdomains { get; set; }

		public IDictionary<string, string> GetQueryParameters()
		{
			var dict = new Dictionary<string, string>();

			if (IncludeSubdomains)
				dict.Add("include_subdomains", "true");

			return dict;
		}
	}
}
