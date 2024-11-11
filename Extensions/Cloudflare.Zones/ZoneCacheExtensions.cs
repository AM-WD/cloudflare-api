using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Zones.Internals.Requests;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Extends the <see cref="ICloudflareClient"/> with methods for working with zones.
	/// </summary>
	public static class ZoneCacheExtensions
	{
		/// <summary>
		/// Purges all cached contents for a zone.
		/// </summary>
		/// <remarks>
		/// Removes ALL files from Cloudflare's cache. All tiers can purge everything.
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IdResponse>> PurgeCachedContent(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			var req = new InternalPurgeCacheRequest
			{
				PurgeEverything = true
			};
			return client.PostAsync<IdResponse, InternalPurgeCacheRequest>($"zones/{zoneId}/purge_cache", req, cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Purges cached contents by URLs.
		/// <br />
		/// Granularly removes one or more files from Cloudflare's cache by specifying URLs.
		/// All tiers can purge by URL.
		/// </summary>
		/// <remarks>
		/// <para>
		/// To purge files with custom cache keys, include the headers used to compute the cache key as in the example.
		/// If you have a device type or geo in your cache key, you will need to include the CF-Device-Type or CF-IPCountry headers.
		/// If you have lang in your cache key, you will need to include the Accept-Language header.
		/// </para>
		/// <para>
		/// <strong>NB</strong>: When including the Origin header, be sure to include the <strong>scheme</strong> and <strong>hostname</strong>.
		/// The port number can be omitted if it is the default port (80 for http, 443 for https), but must be included otherwise.
		/// </para>
		/// <para>
		/// <strong>NB</strong>: For Zones on Free/Pro/Business plan, you may purge up to 30 URLs in one API call.
		/// For Zones on Enterprise plan, you may purge up to 500 URLs in one API call.
		/// </para>
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="urls">List of URLs to purge.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IdResponse>> PurgeCachedContentByUrl(this ICloudflareClient client, string zoneId, IReadOnlyList<ZonePurgeCachedUrlRequest> urls, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			var req = new InternalPurgeCacheRequest();

			if (urls.Any(u => u.Headers.Count > 0))
			{
				req.UrlsWithHeaders = urls.Where(u => !string.IsNullOrWhiteSpace(u.Url))
					.Select(u => new PurgeUrlWithHeaders
					{
						Url = u.Url,
						Headers = u.Headers
							.Where(kvp => !string.IsNullOrWhiteSpace(kvp.Key))
							.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
					}).ToList();
			}
			else
			{
				req.Urls = urls.Where(u => !string.IsNullOrWhiteSpace(u.Url)).Select(u => u.Url).ToList();
			}

			return client.PostAsync<IdResponse, InternalPurgeCacheRequest>($"zones/{zoneId}/purge_cache", req, cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Purges cached contents by cache-tags.
		/// </summary>
		/// <remarks>
		/// <para>
		/// For more information on cache tags and purging by tags, please refer to
		/// <see href="https://developers.cloudflare.com/cache/how-to/purge-cache/purge-by-tags/#purge-cache-by-cache-tags-enterprise-only">purge by cache-tags documentation page</see>.
		/// </para>
		/// <para>
		/// Cache-Tag purging has a rate limit of 30_000 purge API calls in every 24 hour period.
		/// You may purge up to 30 tags in one API call.
		/// </para>
		/// <para>
		/// <em>This rate limit can be raised for customers who need to purge at higher volume.</em>
		/// </para>
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="tags">List of tags to purge.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IdResponse>> PurgeCachedContentByTag(this ICloudflareClient client, string zoneId, IReadOnlyList<string> tags, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			if (tags == null)
				throw new ArgumentNullException(nameof(tags));

			var req = new InternalPurgeCacheRequest
			{
				Tags = tags.Where(t => !string.IsNullOrWhiteSpace(t)).ToList()
			};
			return client.PostAsync<IdResponse, InternalPurgeCacheRequest>($"zones/{zoneId}/purge_cache", req, cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Purges cached contents by hosts.
		/// </summary>
		/// <remarks>
		/// <para>
		/// For more information purging by hostnames, please refer to
		/// <see href="https://developers.cloudflare.com/cache/how-to/purge-cache/purge-by-hostname/">purge by hostname documentation page</see>.
		/// </para>
		/// <para>
		/// Host purging has a rate limit of 30_000 purge API calls in every 24 hour period.
		/// You may purge up to 30 hosts in one API call.
		/// </para>
		/// <para>
		/// <em>This rate limit can be raised for customers who need to purge at higher volume.</em>
		/// </para>
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="hosts">List of hostnames to purge.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IdResponse>> PurgeCachedContentByHost(this ICloudflareClient client, string zoneId, IReadOnlyList<string> hosts, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			if (hosts == null)
				throw new ArgumentNullException(nameof(hosts));

			var req = new InternalPurgeCacheRequest
			{
				Hostnames = hosts.Where(h => !string.IsNullOrWhiteSpace(h)).ToList()
			};
			return client.PostAsync<IdResponse, InternalPurgeCacheRequest>($"zones/{zoneId}/purge_cache", req, cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Purges cached contents by prefixes.
		/// </summary>
		/// <remarks>
		/// <para>
		/// For more information purging by prefixes, please refer to
		/// <see href="https://developers.cloudflare.com/cache/how-to/purge-cache/purge_by_prefix/">purge by prefix documentation page</see>.
		/// </para>
		/// <para>
		/// Prefix purging has a rate limit of 30_000 purge API calls in every 24 hour period.
		/// You may purge up to 30 prefixes in one API call.
		/// </para>
		/// <para>
		/// <em>This rate limit can be raised for customers who need to purge at higher volume.</em>
		/// </para>
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="prefixes">List of prefixes to purge.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IdResponse>> PurgeCachedContentByPrefix(this ICloudflareClient client, string zoneId, IReadOnlyList<string> prefixes, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			if (prefixes == null)
				throw new ArgumentNullException(nameof(prefixes));

			var req = new InternalPurgeCacheRequest
			{
				Prefixes = prefixes.Where(h => !string.IsNullOrWhiteSpace(h)).ToList()
			};
			return client.PostAsync<IdResponse, InternalPurgeCacheRequest>($"zones/{zoneId}/purge_cache", req, cancellationToken: cancellationToken);
		}
	}
}
