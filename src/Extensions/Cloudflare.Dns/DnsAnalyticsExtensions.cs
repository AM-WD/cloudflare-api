using System.Threading;
using System.Threading.Tasks;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Extensions for <see href="https://developers.cloudflare.com/api/resources/dns/subresources/analytics/">DNS Analytics</see>.
	/// </summary>
	public static class DnsAnalyticsExtensions
	{
		/// <summary>
		/// Retrieves a list of summarised aggregate metrics over a given time period.
		/// </summary>
		/// <remarks>
		/// See <see href="https://developers.cloudflare.com/dns/reference/analytics-api-properties/">Analytics API properties</see> for detailed information about the available query parameters.
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="options">Filter options (optional).</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsAnalyticsReport>> GetDnsAnalyticsReport(this ICloudflareClient client, string zoneId, GetDnsAnalyticsReportFilter? options = null, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.GetAsync<DnsAnalyticsReport>($"/zones/{zoneId}/dns_analytics/report", options, cancellationToken);
		}

		/// <summary>
		/// Retrieves a list of aggregate metrics grouped by time interval.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="options">Filter options (optional).</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsAnalyticsByTime>> GetDnsAnalyticsByTime(this ICloudflareClient client, string zoneId, GetDnsAnalyticsByTimeFilter? options = null, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.GetAsync<DnsAnalyticsByTime>($"/zones/{zoneId}/dns_analytics/report/bytime", options, cancellationToken);
		}
	}
}
