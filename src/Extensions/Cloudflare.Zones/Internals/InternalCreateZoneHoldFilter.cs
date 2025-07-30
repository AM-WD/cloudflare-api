namespace AMWD.Net.Api.Cloudflare.Zones.Internals
{
	internal class InternalCreateZoneHoldFilter : IQueryParameterFilter
	{
		public bool? IncludeSubdomains { get; set; }

		public IReadOnlyDictionary<string, string> GetQueryParameters()
		{
			var dict = new Dictionary<string, string>();

			if (IncludeSubdomains.HasValue)
				dict.Add("include_subdomains", IncludeSubdomains.Value ? "true" : "false");

			return dict;
		}
	}
}
