using System.Linq;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Filter for DNS analytics report by time.
	/// </summary>
	public class GetDnsAnalyticsByTimeFilter : GetDnsAnalyticsReportFilter
	{
		/// <summary>
		/// Unit of time to group data by
		/// </summary>
		public TimeDeltaUnit? TimeDelta { get; set; }

		/// <inheritdoc/>
		public override IReadOnlyDictionary<string, string> GetQueryParameters()
		{
			var dict = base.GetQueryParameters().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			if (TimeDelta.HasValue && Enum.IsDefined(typeof(TimeDeltaUnit), TimeDelta))
				dict.Add("time_delta", TimeDelta.Value.GetEnumMemberValue()!);

			return dict;
		}
	}
}
