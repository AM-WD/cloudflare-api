namespace AMWD.Net.Api.Cloudflare.Zones.Internals
{
	internal class InternalRemoveZoneHoldFilter : IQueryParameterFilter
	{
		public DateTime? HoldAfter { get; set; }

		public IReadOnlyDictionary<string, string> GetQueryParameters()
		{
			var dict = new Dictionary<string, string>();

			if (HoldAfter.HasValue)
				dict.Add("hold_after", HoldAfter.Value.ToIso8601Format());

			return dict;
		}
	}
}
