using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Zones.Internals.Requests;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Extends the <see cref="ICloudflareClient"/> with methods for working with zones.
	/// </summary>
	public static class DnsRecordsExtensions
	{
		private static readonly IReadOnlyCollection<DnsRecordType> _dataComponentTypes = [
			DnsRecordType.Caa,
			DnsRecordType.Cert,
			DnsRecordType.DnsKey,
			DnsRecordType.Ds,
			DnsRecordType.Https,
			DnsRecordType.Loc,
			DnsRecordType.NaPtr,
			DnsRecordType.SMimeA,
			DnsRecordType.Srv,
			DnsRecordType.SshFp,
			DnsRecordType.SvcB,
			DnsRecordType.TlsA,
			DnsRecordType.Uri,
		];

		private static readonly IReadOnlyCollection<DnsRecordType> _priorityTypes = [
			DnsRecordType.Mx,
			DnsRecordType.Srv,
			DnsRecordType.Uri,
		];

		/// <summary>
		/// List, search, sort, and filter a zones' DNS records.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="options">Filter options (optional).</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IReadOnlyList<DnsRecord>>> ListDnsRecords(this ICloudflareClient client, string zoneId, ListDnsRecordsFilter? options = null, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			return client.GetAsync<IReadOnlyList<DnsRecord>>($"zones/{zoneId}/dns_records", options, cancellationToken);
		}

		/// <summary>
		/// Create a new DNS record for a zone.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>A/AAAA records cannot exist on the same name as CNAME records.</item>
		/// <item>NS records cannot exist on the same name as any other record type.</item>
		/// <item>Domain names are always represented in Punycode, even if Unicode characters were used when creating the record.</item>
		/// </list>
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="request">The request information.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsRecord>> CreateDnsRecord(this ICloudflareClient client, CreateDnsRecordRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			var req = ValidateDnsRecordRequest(request);

			return client.PostAsync<DnsRecord, InternalDnsRecordRequest>($"zones/{request.ZoneId}/dns_records", req, cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Send a Batch of DNS Record API calls to be executed together.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>
		/// Although Cloudflare will execute the batched operations in a single database transaction,
		/// Cloudflare's distributed KV store must treat each record change as a single key-value pair.
		/// This means that the propagation of changes is not atomic.
		/// See <see href="https://developers.cloudflare.com/dns/manage-dns-records/how-to/batch-record-changes/">the documentation</see> for more information.
		/// </item>
		/// <item>
		/// The operations you specify within the batch request body are always executed in the following order
		/// <list type="number">
		/// <item>Deletes (delete)</item>
		/// <item>Updates (patch)</item>
		/// <item>Overwrites (put)</item>
		/// <item>Creates (post)</item>
		/// </list>
		/// </item>
		/// </list>
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="request">The request information.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<BatchDnsRecordsResult>> BatchDnsRecords(this ICloudflareClient client, BatchDnsRecordsRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			// Validate Deletes
			var deletes = new List<InternalDnsRecordId>();
			foreach (string recordId in request.DnsRecordIdsToDelete)
			{
				recordId.ValidateCloudflareId();
				deletes.Add(new InternalDnsRecordId { Id = recordId });
			}

			// Validate Updates
			var updates = new List<InternalBatchUpdateRequest>();
			foreach (var update in request.DnsRecordsToUpdate)
			{
				update.Id.ValidateCloudflareId();
				if (update.ZoneId != request.ZoneId)
					throw new ArgumentException($"The ZoneId of the update request ({update.ZoneId}) does not match the ZoneId of the batch request ({request.ZoneId}).");

				var baseReq = ValidateDnsRecordRequest(update);
				updates.Add(new InternalBatchUpdateRequest(update.Id, baseReq));
			}

			// Validate Overwrites
			var overwrites = new List<InternalBatchUpdateRequest>();
			foreach (var overwrite in request.DnsRecordsToOverwrite)
			{
				overwrite.Id.ValidateCloudflareId();
				if (overwrite.ZoneId != request.ZoneId)
					throw new ArgumentException($"The ZoneId of the overwrite request ({overwrite.ZoneId}) does not match the ZoneId of the batch request ({request.ZoneId}).");

				var baseReq = ValidateDnsRecordRequest(overwrite);
				overwrites.Add(new InternalBatchUpdateRequest(overwrite.Id, baseReq));
			}

			// Validate Creates
			var creates = new List<InternalDnsRecordRequest>();
			foreach (var create in request.DnsRecordsToCreate)
				creates.Add(ValidateDnsRecordRequest(create));

			var req = new InternalBatchRequest();

			if (deletes.Count > 0)
				req.Deletes = deletes;

			if (updates.Count > 0)
				req.Patches = updates;

			if (overwrites.Count > 0)
				req.Puts = overwrites;

			if (creates.Count > 0)
				req.Posts = creates;

			return client.PostAsync<BatchDnsRecordsResult, InternalBatchRequest>($"zones/{request.ZoneId}/dns_records/batch", req, cancellationToken: cancellationToken);
		}

		/// <summary>
		/// You can export your <see href="https://en.wikipedia.org/wiki/Zone_file">BIND config</see> through this endpoint.
		/// </summary>
		/// <remarks>
		/// See <see href="https://developers.cloudflare.com/dns/manage-dns-records/how-to/import-and-export/">the documentation</see> for more information.
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<string>> ExportDnsRecords(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			return client.GetAsync<string>($"zones/{zoneId}/dns_records/export", cancellationToken: cancellationToken);
		}

		/// <summary>
		/// You can upload your BIND config through this endpoint.
		/// </summary>
		/// <remarks>
		/// See the documentation for more information.
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="request">The request information.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ImportDnsRecordsResult>> ImportDnsRecords(this ICloudflareClient client, ImportDnsRecordsRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(request.File))
				throw new ArgumentNullException(nameof(request.File));

			var req = new MultipartFormDataContent();

			if (request.Proxied.HasValue)
				req.Add(new StringContent(request.Proxied.Value.ToString().ToLowerInvariant()), "proxied");

			byte[] content = File.Exists(request.File)
				? File.ReadAllBytes(request.File)
				: Encoding.UTF8.GetBytes(request.File);

			req.Add(new ByteArrayContent(content), "file");

			return client.PostAsync<ImportDnsRecordsResult, MultipartFormDataContent>($"zones/{request.ZoneId}/dns_records/import", req, cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Scan for common DNS records on your domain and automatically add them to your zone.
		/// <br/>
		/// Useful if you haven't updated your nameservers yet.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<ImportDnsRecordsResult>> ScanDnsRecords(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			return client.PostAsync<ImportDnsRecordsResult, object>($"zones/{zoneId}/dns_records/scan", null, cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Deletes a DNS record.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="dnsRecordId">The record ID.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IdResponse>> DeleteDnsRecord(this ICloudflareClient client, string zoneId, string dnsRecordId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			dnsRecordId.ValidateCloudflareId();
			return client.DeleteAsync<IdResponse>($"zones/{zoneId}/dns_records/{dnsRecordId}", cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Returns details for a DNS record.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="zoneId">The zone ID.</param>
		/// <param name="dnsRecordId">The record ID.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsRecord>> DnsRecordDetails(this ICloudflareClient client, string zoneId, string dnsRecordId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			dnsRecordId.ValidateCloudflareId();
			return client.GetAsync<DnsRecord>($"zones/{zoneId}/dns_records/{dnsRecordId}", cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Update an existing DNS record.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>A/AAAA records cannot exist on the same name as CNAME records.</item>
		/// <item>NS records cannot exist on the same name as any other record type.</item>
		/// <item>Domain names are always represented in Punycode, even if Unicode characters were used when creating the record.</item>
		/// </list>
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="request">The request information.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsRecord>> UpdateDnsRecord(this ICloudflareClient client, UpdateDnsRecordRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();
			request.Id.ValidateCloudflareId();

			var req = ValidateDnsRecordRequest(request);

			return client.PatchAsync<DnsRecord, InternalDnsRecordRequest>($"zones/{request.ZoneId}/dns_records/{request.Id}", req, cancellationToken: cancellationToken);
		}

		/// <summary>
		/// Overwrite an existing DNS record.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>A/AAAA records cannot exist on the same name as CNAME records.</item>
		/// <item>NS records cannot exist on the same name as any other record type.</item>
		/// <item>Domain names are always represented in Punycode, even if Unicode characters were used when creating the record.</item>
		/// </list>
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/>.</param>
		/// <param name="request">The request information.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsRecord>> OverwriteDnsRecord(this ICloudflareClient client, OverwriteDnsRecordRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();
			request.Id.ValidateCloudflareId();

			var req = ValidateDnsRecordRequest(request);

			return client.PutAsync<DnsRecord, InternalDnsRecordRequest>($"zones/{request.ZoneId}/dns_records/{request.Id}", req, cancellationToken: cancellationToken);
		}

		private static InternalDnsRecordRequest ValidateDnsRecordRequest(CreateDnsRecordRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ArgumentNullException(nameof(request.Name));

			if (!Enum.IsDefined(typeof(DnsRecordType), request.Type))
				throw new ArgumentOutOfRangeException(nameof(request.Type), request.Type, "Value must be one of the 'ZoneDnsRecordType' enum values.");

			var req = new InternalDnsRecordRequest(request.Type, request.Name)
			{
				Comment = request.Comment?.Trim(),
				Proxied = request.Proxied,
				Tags = request.Tags?.Where(t => !string.IsNullOrWhiteSpace(t)).ToList()
			};

			if (!_dataComponentTypes.Contains(request.Type) && string.IsNullOrWhiteSpace(request.Content))
				throw new ArgumentNullException(nameof(request.Content));
			else
				req.Content = request.Content?.Trim();

			if (request.Type == DnsRecordType.Cname && request.Settings != null && request.Settings is DnsRecord.CnameSettings cnameSettings)
				req.Settings = new DnsRecord.CnameSettings(cnameSettings.FlattenCname);

			if (_dataComponentTypes.Contains(request.Type) && request.Data == null)
				throw new ArgumentNullException(nameof(request.Data));

			if (_priorityTypes.Contains(request.Type))
			{
				if (!request.Priority.HasValue)
					throw new ArgumentNullException(nameof(request.Priority));

				req.Priority = request.Priority.Value;
			}

			if (request.Ttl.HasValue)
			{
				if (request.Ttl == 1)
				{
					req.Ttl = 1;
				}
				else if (request.Ttl < 30 || 86400 < request.Ttl)
				{
					throw new ArgumentOutOfRangeException(nameof(request.Ttl), request.Ttl, "Value must be between 60 (Enterprise: 30) and 86400.");
				}
				else
				{
					req.Ttl = request.Ttl;
				}
			}

			switch (request.Type)
			{
				case DnsRecordType.Caa:
					if (request.Data is DnsRecord.CaaData caaData)
					{
						if (string.IsNullOrWhiteSpace(caaData.Tag))
							throw new ArgumentNullException(nameof(caaData.Tag));

						if (string.IsNullOrWhiteSpace(caaData.Value))
							throw new ArgumentNullException(nameof(caaData.Value));

						req.Data = new DnsRecord.CaaData(caaData.Flags, caaData.Tag, caaData.Value);
					}
					else
					{
						throw new ArgumentException("Value must be of type 'CaaData'.", nameof(request.Data));
					}
					break;

				case DnsRecordType.Cert:
					if (request.Data is DnsRecord.CertData certData)
					{
						if (string.IsNullOrWhiteSpace(certData.Certificate))
							throw new ArgumentNullException(nameof(certData.Certificate));

						req.Data = new DnsRecord.CertData(certData.Algorithm, certData.Certificate, certData.KeyTag, certData.Type);
					}
					else
					{
						throw new ArgumentException("Value must be of type 'CertData'.", nameof(request.Data));
					}
					break;

				case DnsRecordType.DnsKey:
					if (request.Data is DnsRecord.DnsKeyData dnsKeyData)
					{
						if (string.IsNullOrWhiteSpace(dnsKeyData.PublicKey))
							throw new ArgumentNullException(nameof(dnsKeyData.PublicKey));

						req.Data = new DnsRecord.DnsKeyData(dnsKeyData.Algorithm, dnsKeyData.Flags, dnsKeyData.Protocol, dnsKeyData.PublicKey);
					}
					else
					{
						throw new ArgumentException("Value must be of type 'DnsKeyData'.", nameof(request.Data));
					}
					break;

				case DnsRecordType.Ds:
					if (request.Data is DnsRecord.DsData dsData)
					{
						if (string.IsNullOrWhiteSpace(dsData.Digest))
							throw new ArgumentNullException(nameof(dsData.Digest));

						req.Data = new DnsRecord.DsData(dsData.Algorithm, dsData.Digest, dsData.DigestType, dsData.KeyTag);
					}
					else
					{
						throw new ArgumentException("Value must be of type 'DsData'.", nameof(request.Data));
					}
					break;

				case DnsRecordType.Https:
					if (request.Data is DnsRecord.HttpsData httpsData)
					{
						if (string.IsNullOrWhiteSpace(httpsData.Target))
							throw new ArgumentNullException(nameof(httpsData.Target));

						if (string.IsNullOrWhiteSpace(httpsData.Value))
							throw new ArgumentNullException(nameof(httpsData.Value));

						req.Data = new DnsRecord.HttpsData(httpsData.Priority, httpsData.Target, httpsData.Value);
					}
					else
					{
						throw new ArgumentException("Value must be of type 'HttpsData'.", nameof(request.Data));
					}
					break;

				case DnsRecordType.Loc:
					if (request.Data is DnsRecord.LocData locData)
					{
						if (locData.LatitudeDegrees < 0 || 90 < locData.LatitudeDegrees)
							throw new ArgumentOutOfRangeException(nameof(locData.LatitudeDegrees), locData.LatitudeDegrees, "Value must be between 0 and 90.");

						if (locData.LatitudeMinutes < 0 || 59 < locData.LatitudeMinutes)
							throw new ArgumentOutOfRangeException(nameof(locData.LatitudeMinutes), locData.LatitudeMinutes, "Value must be between 0 and 59.");

						if (locData.LatitudeSeconds < 0 || 59.999 < locData.LatitudeSeconds)
							throw new ArgumentOutOfRangeException(nameof(locData.LatitudeSeconds), locData.LatitudeSeconds, "Value must be between 0 and 59.999.");

						if (!Enum.IsDefined(typeof(DnsRecord.LatitudeDirection), locData.LatitudeDirection))
							throw new ArgumentOutOfRangeException(nameof(locData.LatitudeDirection), locData.LatitudeDirection, "Value must be one of the 'ZoneDnsRecord.LatitudeDirection' enum values.");

						if (locData.LongitudeDegrees < 0 || 180 < locData.LongitudeDegrees)
							throw new ArgumentOutOfRangeException(nameof(locData.LongitudeDegrees), locData.LongitudeDegrees, "Value must be between 0 and 180.");

						if (locData.LongitudeMinutes < 0 || 59 < locData.LongitudeMinutes)
							throw new ArgumentOutOfRangeException(nameof(locData.LongitudeMinutes), locData.LongitudeMinutes, "Value must be between 0 and 59.");

						if (locData.LongitudeSeconds < 0 || 59.999 < locData.LongitudeSeconds)
							throw new ArgumentOutOfRangeException(nameof(locData.LongitudeSeconds), locData.LongitudeSeconds, "Value must be between 0 and 59.999.");

						if (!Enum.IsDefined(typeof(DnsRecord.LongitudeDirection), locData.LongitudeDirection))
							throw new ArgumentOutOfRangeException(nameof(locData.LongitudeDirection), locData.LongitudeDirection, "Value must be one of the 'ZoneDnsRecord.LongitudeDirection' enum values.");

						if (locData.Altitude < -100_000 || 42_849_672.95 < locData.Altitude)
							throw new ArgumentOutOfRangeException(nameof(locData.Altitude), locData.Altitude, "Value must be between -100,000.00 and 42,849,672.95.");

						if (locData.Size < 0 || 90_000_000 < locData.Size)
							throw new ArgumentOutOfRangeException(nameof(locData.Size), locData.Size, "Value must be between 0 and 90,000,000.");

						if (locData.PrecisionHorizontal < 0 || 90_000_000 < locData.PrecisionHorizontal)
							throw new ArgumentOutOfRangeException(nameof(locData.PrecisionHorizontal), locData.PrecisionHorizontal, "Value must be between 0 and 90,000,000.");

						if (locData.PrecisionVertical < 0 || 90_000_000 < locData.PrecisionVertical)
							throw new ArgumentOutOfRangeException(nameof(locData.PrecisionVertical), locData.PrecisionVertical, "Value must be between 0 and 90,000,000.");

						req.Data = new DnsRecord.LocData(
							latitudeDegrees: locData.LatitudeDegrees,
							latitudeMinutes: locData.LatitudeMinutes,
							latitudeSeconds: Math.Floor(locData.LatitudeSeconds * 1000) / 1000,   // Truncate to 3 decimal places
							latitudeDirection: locData.LatitudeDirection,
							longitudeDegrees: locData.LongitudeDegrees,
							longitudeMinutes: locData.LongitudeMinutes,
							longitudeSeconds: Math.Floor(locData.LongitudeSeconds * 1000) / 1000, // Truncate to 3 decimal places
							longitudeDirection: locData.LongitudeDirection,
							altitude: Math.Floor(locData.Altitude * 100) / 100,                   // Truncate to 2 decimal places
							size: locData.Size,
							precisionHorizontal: locData.PrecisionHorizontal,
							precisionVertical: locData.PrecisionVertical
						);
					}
					else
					{
						throw new ArgumentException("Value must be of type 'LocData'.", nameof(request.Data));
					}
					break;

				case DnsRecordType.NaPtr:
					if (request.Data is DnsRecord.NaPtrData naPtrData)
					{
						if (string.IsNullOrWhiteSpace(naPtrData.Flags))
							throw new ArgumentNullException(nameof(naPtrData.Flags));

						if (string.IsNullOrWhiteSpace(naPtrData.Regex))
							throw new ArgumentNullException(nameof(naPtrData.Regex));

						if (string.IsNullOrWhiteSpace(naPtrData.Replacement))
							throw new ArgumentNullException(nameof(naPtrData.Replacement));

						if (string.IsNullOrWhiteSpace(naPtrData.Service))
							throw new ArgumentNullException(nameof(naPtrData.Service));

						req.Data = new DnsRecord.NaPtrData(
							naPtrData.Flags,
							naPtrData.Order,
							naPtrData.Preference,
							naPtrData.Regex,
							naPtrData.Replacement,
							naPtrData.Service
						);
					}
					else
					{
						throw new ArgumentException("Value must be of type 'NaPtrData'.", nameof(request.Data));
					}
					break;

				case DnsRecordType.SMimeA:
					if (request.Data is DnsRecord.SMimeAData sMimeAData)
					{
						if (string.IsNullOrWhiteSpace(sMimeAData.Certificate))
							throw new ArgumentNullException(nameof(sMimeAData.Certificate));

						req.Data = new DnsRecord.SMimeAData(sMimeAData.Certificate, sMimeAData.MatchingType, sMimeAData.Selector, sMimeAData.Usage);
					}
					else
					{
						throw new ArgumentException("Value must be of type 'SMimeAData'.", nameof(request.Data));
					}
					break;

				case DnsRecordType.Srv:
					if (request.Data is DnsRecord.SrvData srvData)
					{
						if (string.IsNullOrWhiteSpace(srvData.Target))
							throw new ArgumentNullException(nameof(srvData.Target));

						req.Data = new DnsRecord.SrvData(srvData.Port, srvData.Priority, srvData.Target, srvData.Weight);
					}
					else
					{
						throw new ArgumentException("Value must be of type 'SrvData'.", nameof(request.Data));
					}
					break;

				case DnsRecordType.SshFp:
					if (request.Data is DnsRecord.SshFpData sshFpData)
					{
						if (string.IsNullOrWhiteSpace(sshFpData.Fingerprint))
							throw new ArgumentNullException(nameof(sshFpData.Fingerprint));

						req.Data = new DnsRecord.SshFpData(sshFpData.Algorithm, sshFpData.Fingerprint, sshFpData.Type);
					}
					else
					{
						throw new ArgumentException("Value must be of type 'SshFpData'.", nameof(request.Data));
					}
					break;

				case DnsRecordType.SvcB:
					if (request.Data is DnsRecord.SvcBData svcBData)
					{
						if (string.IsNullOrWhiteSpace(svcBData.Target))
							throw new ArgumentNullException(nameof(svcBData.Target));

						if (string.IsNullOrWhiteSpace(svcBData.Value))
							throw new ArgumentNullException(nameof(svcBData.Value));

						req.Data = new DnsRecord.SvcBData(svcBData.Priority, svcBData.Target, svcBData.Value);
					}
					else
					{
						throw new ArgumentException("Value must be of type 'SvcBData'.", nameof(request.Data));
					}
					break;

				case DnsRecordType.TlsA:
					if (request.Data is DnsRecord.TlsAData tlsAData)
					{
						if (string.IsNullOrWhiteSpace(tlsAData.Certificate))
							throw new ArgumentNullException(nameof(tlsAData.Certificate));

						req.Data = new DnsRecord.TlsAData(tlsAData.Certificate, tlsAData.MatchingType, tlsAData.Selector, tlsAData.Usage);
					}
					else
					{
						throw new ArgumentException("Value must be of type 'TlsAData'.", nameof(request.Data));
					}
					break;

				case DnsRecordType.Uri:
					if (request.Data is DnsRecord.UriData uriData)
					{
						if (string.IsNullOrWhiteSpace(uriData.Target))
							throw new ArgumentNullException(nameof(uriData.Target));

						req.Data = new DnsRecord.UriData(uriData.Target, uriData.Weight);
					}
					else
					{
						throw new ArgumentException("Value must be of type 'UriData'.", nameof(request.Data));
					}
					break;
			}

			return req;
		}
	}
}
