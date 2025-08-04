using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AMWD.Net.Api.Cloudflare.Dns.Internals;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Extensions for <see href="https://developers.cloudflare.com/api/resources/dns/subresources/records/">DNS Records</see>.
	/// </summary>
	public static class DnsRecordsExtensions
	{
		private static readonly IReadOnlyCollection<DnsRecordType> _dataComponentTypes = [
			DnsRecordType.CAA,
			DnsRecordType.CERT,
			DnsRecordType.DNSKEY,
			DnsRecordType.DS,
			DnsRecordType.HTTPS,
			DnsRecordType.LOC,
			DnsRecordType.NAPTR,
			DnsRecordType.SMIMEA,
			DnsRecordType.SRV,
			DnsRecordType.SSHFP,
			DnsRecordType.SVCB,
			DnsRecordType.TLSA,
			DnsRecordType.URI,
		];

		private static readonly IReadOnlyCollection<DnsRecordType> _priorityTypes = [
			DnsRecordType.MX,
			DnsRecordType.URI,
		];

		/// <summary>
		/// Send a Batch of DNS Record API calls to be executed together.
		/// </summary>
		/// <remarks>
		/// Notes:
		/// <list type="bullet">
		/// <item>
		/// Although Cloudflare will execute the batched operations in a single database
		/// transaction, Cloudflare's distributed KV store must treat each record change
		/// as a single key-value pair. This means that the propagation of changes is not
		/// atomic. See
		/// <see href="https://developers.cloudflare.com/dns/manage-dns-records/how-to/batch-record-changes/">the documentation</see>
		/// for more information.
		/// </item>
		/// <item>
		/// The operations you specify within the /batch request body are always executed
		/// in the following order:
		/// <list type="number">
		/// <item>Deletes (DELETE)</item>
		/// <item>Updates (PATCH)</item>
		/// <item>Overwrites (PUT)</item>
		/// <item>Creates (POST)</item>
		/// </list>
		/// </item>
		/// </list>
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<BatchDnsRecordsResponse>> BatchDnsRecords(this ICloudflareClient client, BatchDnsRecordsRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			// Deletes (DELETE)
			var deletes = new List<Identifier>();
			foreach (string delete in request.Deletes)
			{
				delete.ValidateCloudflareId();
				deletes.Add(new Identifier { Id = delete });
			}

			// Updates (PATCH)
			var patches = new List<InternalBatchUpdateRequest>();
			foreach (var patch in request.Updates)
			{
				patch.Id.ValidateCloudflareId();

				var req = ValidateRequest(patch);
				req.Id = patch.Id;

				patches.Add(req);
			}

			// Creates (POST)
			var posts = new List<InternalDnsRecordRequest>();
			foreach (var post in request.Creates)
			{
				var req = (InternalDnsRecordRequest)ValidateRequest(post);
				posts.Add(req);
			}

			// Overwrites (PUT)
			var puts = new List<InternalBatchUpdateRequest>();
			foreach (var put in request.Overwrites)
			{
				put.Id.ValidateCloudflareId();

				var req = ValidateRequest(put);
				req.Id = put.Id;

				puts.Add(req);
			}

			var batchReq = new InternalBatchRequest();

			if (deletes.Count > 0)
				batchReq.Deletes = deletes;

			if (patches.Count > 0)
				batchReq.Patches = patches;

			if (posts.Count > 0)
				batchReq.Posts = posts;

			if (puts.Count > 0)
				batchReq.Puts = puts;

			return client.PostAsync<BatchDnsRecordsResponse, InternalBatchRequest>($"/zones/{request.ZoneId}/dns_records/batch", batchReq, null, cancellationToken);
		}

		/// <summary>
		/// Create a new DNS record for a zone.
		/// </summary>
		/// <remarks>
		/// Notes:
		/// <list type="bullet">
		/// <item>A/AAAA records cannot exist on the same name as CNAME records.</item>
		/// <item>NS records cannot exist on the same name as any other record type.</item>
		/// <item>Domain names are always represented in Punycode, even if Unicode characters were used when creating the record.</item>
		/// </list>
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsRecord>> CreateDnsRecord(this ICloudflareClient client, CreateDnsRecordRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			var req = ValidateRequest(request);

			return client.PostAsync<DnsRecord, InternalDnsRecordRequest>($"/zones/{request.ZoneId}/dns_records", req, null, cancellationToken);
		}

		/// <summary>
		/// Delete DNS Record.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="recordId">The record identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<Identifier>> DeleteDnsRecord(this ICloudflareClient client, string zoneId, string recordId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			recordId.ValidateCloudflareId();

			return client.DeleteAsync<Identifier>($"/zones/{zoneId}/dns_records/{recordId}", null, cancellationToken);
		}

		/// <summary>
		/// Update an existing DNS record.
		/// </summary>
		/// <remarks>
		/// Notes:
		/// <list type="bullet">
		/// <item>A/AAAA records cannot exist on the same name as CNAME records.</item>
		/// <item>NS records cannot exist on the same name as any other record type.</item>
		/// <item>Domain names are always represented in Punycode, even if Unicode characters were used when creating the record.</item>
		/// </list>
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsRecord>> UpdateDnsRecord(this ICloudflareClient client, UpdateDnsRecordRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();
			request.RecordId.ValidateCloudflareId();

			var req = ValidateRequest(request);

			return client.PatchAsync<DnsRecord, InternalDnsRecordRequest>($"/zones/{request.ZoneId}/dns_records/{request.RecordId}", req, cancellationToken);
		}

		/// <summary>
		/// You can export your <see href="https://en.wikipedia.org/wiki/Zone_file">BIND config</see> through this endpoint.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<string>> ExportDnsRecords(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.GetAsync<string>($"/zones/{zoneId}/dns_records/export", null, cancellationToken);
		}

		/// <summary>
		/// DNS Record Details.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="recordId">The record identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsRecord>> DnsRecordDetails(this ICloudflareClient client, string zoneId, string recordId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();
			recordId.ValidateCloudflareId();

			return client.GetAsync<DnsRecord>($"/zones/{zoneId}/dns_records/{recordId}", null, cancellationToken);
		}

		/// <summary>
		/// You can upload your <see href="https://en.wikipedia.org/wiki/Zone_file">BIND config</see> through this endpoint.
		/// It assumes that cURL is called from a location with bind_config.txt (valid BIND config) present.
		/// </summary>
		/// <remarks>
		/// See <see href="https://developers.cloudflare.com/dns/manage-dns-records/how-to/import-and-export/">the documentation</see> for more information.
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<RecordImportResponse>> ImportDnsRecords(this ICloudflareClient client, ImportDnsRecordsRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			if (string.IsNullOrWhiteSpace(request.File))
				throw new ArgumentNullException(nameof(request.File));

			var req = new MultipartFormDataContent();

			if (request.Proxied.HasValue)
				req.Add(new StringContent(request.Proxied.Value.ToString().ToLowerInvariant()), "proxied");

			byte[] fileBytes = File.Exists(request.File)
				? File.ReadAllBytes(request.File)
				: Encoding.UTF8.GetBytes(request.File);

			req.Add(new ByteArrayContent(fileBytes), "file");

			return client.PostAsync<RecordImportResponse, MultipartFormDataContent>($"/zones/{request.ZoneId}/dns_records/import", req, null, cancellationToken);
		}

		/// <summary>
		/// List, search, sort, and filter a zones' DNS records.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="options">Filter options (optional).</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<IReadOnlyCollection<DnsRecord>>> ListDnsRecords(this ICloudflareClient client, string zoneId, ListDnsRecordsFilter? options = null, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.GetAsync<IReadOnlyCollection<DnsRecord>>($"/zones/{zoneId}/dns_records", options, cancellationToken);
		}

		/// <summary>
		/// Scan for common DNS records on your domain and automatically add them to your zone.
		/// Useful if you haven't updated your nameservers yet.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<RecordScanResponse>> ScanDnsRecords(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.PostAsync<RecordScanResponse, object>($"/zones/{zoneId}/dns_records/scan", null, null, cancellationToken);
		}

		/// <summary>
		/// Overwrite an existing DNS record.
		/// </summary>
		/// <remarks>
		/// Notes:
		/// <list type="bullet">
		/// <item>A/AAAA records cannot exist on the same name as CNAME records.</item>
		/// <item>NS records cannot exist on the same name as any other record type.</item>
		/// <item>Domain names are always represented in Punycode, even if Unicode characters were used when creating the record.</item>
		/// </list>
		/// </remarks>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<DnsRecord>> OverwriteDnsRecord(this ICloudflareClient client, OverwriteDnsRecordRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();
			request.RecordId.ValidateCloudflareId();

			var req = ValidateRequest(request);

			return client.PutAsync<DnsRecord, InternalDnsRecordRequest>($"/zones/{request.ZoneId}/dns_records/{request.RecordId}", req, cancellationToken);
		}

		private static InternalDnsRecordRequest ValidateRequest(CreateDnsRecordRequest request)
		{
			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ArgumentNullException(nameof(request.Name));

			if (!Enum.IsDefined(typeof(DnsRecordType), request.Type))
				throw new ArgumentOutOfRangeException(nameof(request.Type), request.Type, $"The value '{request.Type}' is not valid. Valid value are in {nameof(DnsRecordType)}.");

			var req = new InternalDnsRecordRequest
			{
				Name = request.Name.Trim(),
				Type = request.Type,
				Comment = request.Comment?.Trim(),
				Proxied = request.Proxied,
				Tags = request.Tags?
					.Where(t => !string.IsNullOrWhiteSpace(t))
					.Select(t => t.Trim())
					.ToList()
			};

			if (!_dataComponentTypes.Contains(request.Type) && string.IsNullOrWhiteSpace(request.Content))
			{
				throw new ArgumentNullException(nameof(request.Content));
			}
			else
			{
				req.Content = request.Content?.Trim();
			}

			if (request.Type == DnsRecordType.CNAME && request.Settings != null && request.Settings is CNAMERecordSettings cnameSettings)
				req.Settings = cnameSettings;

			if (_dataComponentTypes.Contains(request.Type) && request.Data == null)
				throw new ArgumentNullException(nameof(request.Data));

			if (_priorityTypes.Contains(request.Type))
			{
				if (!request.Priority.HasValue)
					throw new ArgumentNullException(nameof(request.Priority));

				req.Priority = request.Priority.Value;
			}

			if (request.TimeToLive.HasValue)
			{
				if (request.TimeToLive == 1)
				{
					req.Ttl = 1;
				}
				else if (request.TimeToLive < 30 || 86400 < request.TimeToLive)
				{
					throw new ArgumentOutOfRangeException(nameof(request.TimeToLive), request.TimeToLive, $"The value '{request.TimeToLive}' is not valid. Valid value are between 60 (30 for Enterprise) and 86400.");
				}
				else
				{
					req.Ttl = request.TimeToLive.Value;
				}
			}

			switch (request.Type)
			{
				case DnsRecordType.CAA:
					if (request.Data is CAARecordData caaData)
					{
						if (string.IsNullOrWhiteSpace(caaData.Tag))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(caaData.Tag)}");

						if (string.IsNullOrWhiteSpace(caaData.Value))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(caaData.Value)}");

						req.Data = caaData;
					}
					else
					{
						throw new ArgumentException($"The type of the value '{nameof(request.Data)}' has to be '{nameof(CAARecordData)}'", nameof(request.Data));
					}
					break;

				case DnsRecordType.CERT:
					if (request.Data is CERTRecordData certData)
					{
						if (string.IsNullOrWhiteSpace(certData.Certificate))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(certData.Certificate)}");

						req.Data = certData;
					}
					else
					{
						throw new ArgumentException($"The type of the value '{nameof(request.Data)}' has to be '{nameof(CAARecordData)}'", nameof(request.Data));
					}
					break;

				case DnsRecordType.DNSKEY:
					if (request.Data is DNSKEYRecordData dnskeyData)
					{
						if (string.IsNullOrWhiteSpace(dnskeyData.PublicKey))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(dnskeyData.PublicKey)}");

						req.Data = dnskeyData;
					}
					else
					{
						throw new ArgumentException($"The type of the value '{nameof(request.Data)}' has to be '{nameof(DNSKEYRecordData)}'", nameof(request.Data));
					}
					break;

				case DnsRecordType.DS:
					if (request.Data is DSRecordData dsData)
					{
						if (string.IsNullOrWhiteSpace(dsData.Digest))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(dsData.Digest)}");

						req.Data = dsData;
					}
					else
					{
						throw new ArgumentException($"The type of the value '{nameof(request.Data)}' has to be '{nameof(DSRecordData)}'", nameof(request.Data));
					}
					break;

				case DnsRecordType.HTTPS:
					if (request.Data is HTTPSRecordData httpsData)
					{
						if (string.IsNullOrWhiteSpace(httpsData.Target))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(httpsData.Target)}");

						if (string.IsNullOrWhiteSpace(httpsData.Value))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(httpsData.Value)}");

						req.Data = httpsData;
					}
					else
					{
						throw new ArgumentException($"The type of the value '{nameof(request.Data)}' has to be '{nameof(HTTPSRecordData)}'", nameof(request.Data));
					}
					break;

				case DnsRecordType.LOC:
					if (request.Data is LOCRecordData locData)
					{
						if (locData.LatitudeDegrees.HasValue && (locData.LatitudeDegrees < 0 || 90 < locData.LatitudeDegrees))
							throw new ArgumentOutOfRangeException($"{nameof(request.Data)}.{nameof(locData.LatitudeDegrees)}", locData.LatitudeDegrees, $"The value '{locData.LatitudeDegrees}' is not valid. Valid value are between {0} and {90}.");

						if (locData.LatitudeMinutes.HasValue && (locData.LatitudeMinutes < 0 || 59 < locData.LatitudeMinutes))
							throw new ArgumentOutOfRangeException($"{nameof(request.Data)}.{nameof(locData.LatitudeMinutes)}", locData.LatitudeMinutes, $"The value '{locData.LatitudeMinutes}' is not valid. Valid value are between {0} and {59}.");

						if (locData.LatitudeSeconds.HasValue && (locData.LatitudeSeconds < 0 || 59.999 < locData.LatitudeSeconds))
							throw new ArgumentOutOfRangeException($"{nameof(request.Data)}.{nameof(locData.LatitudeSeconds)}", locData.LatitudeSeconds, $"The value '{locData.LatitudeSeconds}' is not valid. Valid value are between {0} and {59.999}.");

						if (locData.LatitudeDirection.HasValue && !Enum.IsDefined(typeof(LOCRecordLatitudeDirection), locData.LatitudeDirection))
							throw new ArgumentOutOfRangeException($"{nameof(request.Data)}.{nameof(locData.LatitudeDirection)}", locData.LatitudeDirection, $"The value '{locData.LatitudeDirection}' is not valid.");

						if (locData.LongitudeDegrees.HasValue && (locData.LongitudeDegrees < 0 || 180 < locData.LongitudeDegrees))
							throw new ArgumentOutOfRangeException($"{nameof(request.Data)}.{nameof(locData.LongitudeDegrees)}", locData.LongitudeDegrees, $"The value '{locData.LongitudeDegrees}' is not valid. Valid value are between {0} and {180}.");

						if (locData.LongitudeMinutes.HasValue && (locData.LongitudeMinutes < 0 || 59 < locData.LongitudeMinutes))
							throw new ArgumentOutOfRangeException($"{nameof(request.Data)}.{nameof(locData.LongitudeMinutes)}", locData.LongitudeMinutes, $"The value '{locData.LongitudeMinutes}' is not valid. Valid value are between {0} and {59}.");

						if (locData.LongitudeSeconds.HasValue && (locData.LongitudeSeconds < 0 || 59.999 < locData.LongitudeSeconds))
							throw new ArgumentOutOfRangeException($"{nameof(request.Data)}.{nameof(locData.LongitudeSeconds)}", locData.LongitudeSeconds, $"The value '{locData.LongitudeSeconds}' is not valid. Valid value are between {0} and {59.999}.");

						if (locData.LongitudeDirection.HasValue && (!Enum.IsDefined(typeof(LOCRecordLongitudeDirection), locData.LongitudeDirection)))
							throw new ArgumentOutOfRangeException($"{nameof(request.Data)}.{nameof(locData.LongitudeDirection)}", locData.LongitudeDirection, $"The value '{locData.LongitudeDirection}' is not valid.");

						if (locData.Altitude.HasValue && (locData.Altitude < -100_000 || 42_849_672.95 < locData.Altitude))
							throw new ArgumentOutOfRangeException($"{nameof(request.Data)}.{nameof(locData.Altitude)}", locData.Size, $"The value '{locData.Altitude}' is not valid. Valid value are between {-100_000} and {42_849_672.95}.");

						if (locData.Size.HasValue && (locData.Size < 0 || 90_000_000 < locData.Size))
							throw new ArgumentOutOfRangeException($"{nameof(request.Data)}.{nameof(locData.Size)}", locData.Size, $"The value '{locData.Size}' is not valid. Valid value are between {0} and {90_000_000}.");

						if (locData.PrecisionHorizontal.HasValue && (locData.PrecisionHorizontal < 0 || 90_000_000 < locData.PrecisionHorizontal))
							throw new ArgumentOutOfRangeException($"{nameof(request.Data)}.{nameof(locData.PrecisionHorizontal)}", locData.Size, $"The value '{locData.PrecisionHorizontal}' is not valid. Valid value are between {0} and {90_000_000}.");

						if (locData.PrecisionVertical.HasValue && (locData.PrecisionVertical < 0 || 90_000_000 < locData.PrecisionVertical))
							throw new ArgumentOutOfRangeException($"{nameof(request.Data)}.{nameof(locData.PrecisionVertical)}", locData.PrecisionVertical, $"The value '{locData.PrecisionVertical}' is not valid. Valid value are between {0} and {90_000_000}.");

						if (locData.LatitudeSeconds.HasValue)
							locData.LatitudeSeconds = Math.Floor(locData.LatitudeSeconds.Value * 1000) / 1000; // Truncate to 3 decimal places

						if (locData.LongitudeSeconds.HasValue)
							locData.LongitudeSeconds = Math.Floor(locData.LongitudeSeconds.Value * 1000) / 1000; // Truncate to 3 decimal places

						if (locData.Altitude.HasValue)
							locData.Altitude = Math.Floor(locData.Altitude.Value * 100) / 100; // Truncate to 2 decimal places

						req.Data = locData;
					}
					else
					{
						throw new ArgumentException($"The type of the value '{nameof(request.Data)}' has to be '{nameof(LOCRecordData)}'", nameof(request.Data));
					}
					break;

				case DnsRecordType.NAPTR:
					if (request.Data is NAPTRRecordData naptrData)
					{
						if (string.IsNullOrWhiteSpace(naptrData.Flags))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(naptrData.Flags)}");

						if (string.IsNullOrWhiteSpace(naptrData.Regex))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(naptrData.Regex)}");

						if (string.IsNullOrWhiteSpace(naptrData.Replacement))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(naptrData.Replacement)}");

						if (string.IsNullOrWhiteSpace(naptrData.Service))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(naptrData.Service)}");

						req.Data = naptrData;
					}
					else
					{
						throw new ArgumentException($"The type of the value '{nameof(request.Data)}' has to be '{nameof(NAPTRRecordData)}'", nameof(request.Data));
					}
					break;

				case DnsRecordType.SMIMEA:
					if (request.Data is SMIMEARecordData smimeaData)
					{
						if (string.IsNullOrWhiteSpace(smimeaData.Certificate))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(smimeaData.Certificate)}");

						req.Data = smimeaData;
					}
					else
					{
						throw new ArgumentException($"The type of the value '{nameof(request.Data)}' has to be '{nameof(SMIMEARecordData)}'", nameof(request.Data));
					}
					break;

				case DnsRecordType.SRV:
					if (request.Data is SRVRecordData srvData)
					{
						if (string.IsNullOrWhiteSpace(srvData.Target))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(srvData.Target)}");

						req.Data = srvData;
					}
					else
					{
						throw new ArgumentException($"The type of the value '{nameof(request.Data)}' has to be '{nameof(SRVRecordData)}'", nameof(request.Data));
					}
					break;

				case DnsRecordType.SSHFP:
					if (request.Data is SSHFPRecordData sshfpData)
					{
						if (string.IsNullOrWhiteSpace(sshfpData.Fingerprint))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(sshfpData.Fingerprint)}");

						req.Data = sshfpData;
					}
					else
					{
						throw new ArgumentException($"The type of the value '{nameof(request.Data)}' has to be '{nameof(SSHFPRecordData)}'", nameof(request.Data));
					}
					break;

				case DnsRecordType.SVCB:
					if (request.Data is SVCBRecordData svcbData)
					{
						if (string.IsNullOrWhiteSpace(svcbData.Target))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(svcbData.Target)}");

						if (string.IsNullOrWhiteSpace(svcbData.Value))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(svcbData.Value)}");

						req.Data = svcbData;
					}
					else
					{
						throw new ArgumentException($"The type of the value '{nameof(request.Data)}' has to be '{nameof(SVCBRecordData)}'", nameof(request.Data));
					}
					break;

				case DnsRecordType.TLSA:
					if (request.Data is TLSARecordData tlsaData)
					{
						if (string.IsNullOrWhiteSpace(tlsaData.Certificate))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(tlsaData.Certificate)}");

						req.Data = tlsaData;
					}
					else
					{
						throw new ArgumentException($"The type of the value '{nameof(request.Data)}' has to be '{nameof(TLSARecordData)}'", nameof(request.Data));
					}
					break;

				case DnsRecordType.URI:
					if (request.Data is URIRecordData uriData)
					{
						if (string.IsNullOrWhiteSpace(uriData.Target))
							throw new ArgumentNullException($"{nameof(request.Data)}.{nameof(uriData.Target)}");

						req.Data = uriData;
					}
					else
					{
						throw new ArgumentException($"The type of the value '{nameof(request.Data)}' has to be '{nameof(URIRecordData)}'", nameof(request.Data));
					}
					break;
			}

			return req;
		}

		private static InternalBatchUpdateRequest ValidateRequest(BatchDnsRecordsRequest.Post request)
		{
			var req = ValidateRequest(new CreateDnsRecordRequest("", request.Name)
			{
				Comment = request.Comment,
				Content = request.Content,
				Data = request.Data,
				Priority = request.Priority,
				Proxied = request.Proxied,
				Settings = request.Settings,
				Tags = request.Tags,
				TimeToLive = request.TimeToLive,
				Type = request.Type
			});

			return new InternalBatchUpdateRequest
			{
				Comment = req.Comment,
				Content = req.Content,
				Data = req.Data,
				Name = req.Name,
				Priority = req.Priority,
				Proxied = req.Proxied,
				Settings = req.Settings,
				Tags = req.Tags,
				Ttl = req.Ttl,
				Type = req.Type
			};
		}
	}
}
