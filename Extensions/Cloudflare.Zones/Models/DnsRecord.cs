using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// DNS record.
	/// </summary>
	public class DnsRecord
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// Identifier of the zone.
		/// </summary>
		[JsonProperty("zone_id")]
		public string? ZoneId { get; set; }

		/// <summary>
		/// Name of the zone.
		/// </summary>
		[JsonProperty("zone_name")]
		public string? ZoneName { get; set; }

		/// <summary>
		/// DNS record name (or @ for the zone apex) in Punycode.
		/// </summary>
		[JsonProperty("name")]
		public string? Name { get; set; }

		/// <summary>
		/// Record type.
		/// </summary>
		[JsonProperty("type")]
		public DnsRecordType Type { get; set; }

		/// <summary>
		/// A valid record content.
		/// </summary>
		[JsonProperty("content")]
		public string? Content { get; set; }

		/// <summary>
		/// The priority.
		/// </summary>
		/// <remarks>
		/// Required for <see cref="DnsRecordType.Mx"/>, <see cref="DnsRecordType.Srv"/> and <see cref="DnsRecordType.Uri"/> records; unused by other record types.
		/// Records with lower priorities are preferred.
		/// </remarks>
		[JsonProperty("priority")]
		public ushort? Priority { get; set; }

		/// <summary>
		/// Components associated with the record.
		/// </summary>
		[JsonProperty("data")]
		public object? Data { get; set; }

		/// <summary>
		/// Whether the record can be proxied by Cloudflare or not.
		/// </summary>
		[JsonProperty("proxiable")]
		public bool Proxiable { get; set; }

		/// <summary>
		/// Whether the record is receiving the performance and security benefits of Cloudflare.
		/// </summary>
		[JsonProperty("proxied")]
		public bool Proxied { get; set; }

		/// <summary>
		/// Time To Live (TTL) of the DNS record in seconds. Setting to 1 means 'automatic'.
		/// Value must be between 60 and 86400, <em>with the minimum reduced to 30 for Enterprise zones</em>.
		/// </summary>
		/// <value>Unit: seconds. Range: <c>60 &lt;=</c> X <c>&lt;= 86400</c></value>
		[JsonProperty("ttl")]
		public int Ttl { get; set; }

		/// <summary>
		/// Settings for the DNS record.
		/// </summary>
		[JsonProperty("settings")]
		public object? Settings { get; set; }

		/// <summary>
		/// Extra Cloudflare-specific information about the record.
		/// </summary>
		[JsonProperty("meta")]
		public DnsRecordMeta? Meta { get; set; }

		/// <summary>
		/// Comments or notes about the DNS record. This field has no effect on DNS responses.
		/// </summary>
		[JsonProperty("comment")]
		public string? Comment { get; set; }

		/// <summary>
		/// Custom tags for the DNS record. This field has no effect on DNS responses.
		/// </summary>
		[JsonProperty("tags")]
		public IList<string>? Tags { get; set; }

		/// <summary>
		/// When the record was created.
		/// </summary>
		[JsonProperty("created_on")]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// When the record was last modified.
		/// </summary>
		[JsonProperty("modified_on")]
		public DateTime ModifiedOn { get; set; }

		/// <summary>
		/// When the record comment was last modified. Omitted if there is no comment.
		/// </summary>
		[JsonProperty("comment_modified_on")]
		public DateTime? CommentModifiedOn { get; set; }

		/// <summary>
		/// When the record tags were last modified. Omitted if there are no tags.
		/// </summary>
		[JsonProperty("tags_modified_on")]
		public DateTime? TagsModifiedOn { get; set; }

		#region Type definitions for data fields

		/// <summary>
		/// Components of a CAA record.
		/// </summary>
		public class CaaData(byte flags, string tag, string value)
		{
			/// <summary>
			/// Flags for the CAA record
			/// </summary>
			/// <value>Range: <c>0 &lt;=</c> X <c>&lt;= 255</c></value>
			[JsonProperty("flags")]
			public byte Flags { get; set; } = flags;

			/// <summary>
			/// Name of the property controlled by this record (e.g.: issue, issuewild, iodef).
			/// </summary>
			[JsonProperty("tag")]
			public string Tag { get; set; } = tag;

			/// <summary>
			/// Value of the record. This field's semantics depend on the chosen tag.
			/// </summary>
			[JsonProperty("value")]
			public string Value { get; set; } = value;
		}

		/// <summary>
		/// Components of a CERT record.
		/// </summary>
		public class CertData(byte algorithm, string certificate, ushort keyTag, ushort type)
		{
			/// <summary>
			/// Algorithm of the certificate
			/// </summary>
			[JsonProperty("algorithm")]
			public byte Algorithm { get; set; } = algorithm;

			/// <summary>
			/// Base64 encoded certificate.
			/// </summary>
			[JsonProperty("certificate")]
			public string Certificate { get; set; } = certificate;

			/// <summary>
			/// Key tag.
			/// </summary>
			[JsonProperty("key_tag")]
			public ushort KeyTag { get; set; } = keyTag;

			/// <summary>
			/// Type.
			/// </summary>
			[JsonProperty("type")]
			public ushort Type { get; set; } = type;
		}

		/// <summary>
		/// Components of a DNSKEY record.
		/// </summary>
		public class DnsKeyData(byte algorithm, ushort flags, byte protocol, string publicKey)
		{
			/// <summary>
			/// Algorithm of the certificate
			/// </summary>
			[JsonProperty("algorithm")]
			public byte Algorithm { get; set; } = algorithm;

			/// <summary>
			/// Algorithm of the certificate
			/// </summary>
			[JsonProperty("flags")]
			public ushort Flags { get; set; } = flags;

			/// <summary>
			/// Algorithm of the certificate
			/// </summary>
			[JsonProperty("protocol")]
			public byte Protocol { get; set; } = protocol;

			/// <summary>
			/// Public key.
			/// </summary>
			[JsonProperty("public_key")]
			public string PublicKey { get; set; } = publicKey;
		}

		/// <summary>
		/// Components of a DS record.
		/// </summary>
		public class DsData(byte algorithm, string digest, byte digestType, ushort keyTag)
		{
			/// <summary>
			/// Algorithm of the certificate
			/// </summary>
			[JsonProperty("algorithm")]
			public byte Algorithm { get; set; } = algorithm;

			/// <summary>
			/// Digest.
			/// </summary>
			[JsonProperty("digest")]
			public string Digest { get; set; } = digest;

			/// <summary>
			/// Digest type.
			/// </summary>
			[JsonProperty("digest_type")]
			public byte DigestType { get; set; } = digestType;

			/// <summary>
			/// Key tag.
			/// </summary>
			[JsonProperty("key_tag")]
			public ushort KeyTag { get; set; } = keyTag;
		}

		/// <summary>
		/// Components of an HTTPS record.
		/// </summary>
		public class HttpsData(ushort priority, string target, string value)
		{
			/// <summary>
			/// Priority.
			/// </summary>
			[JsonProperty("priority")]
			public ushort Priority { get; set; } = priority;

			/// <summary>
			/// Target.
			/// </summary>
			[JsonProperty("target")]
			public string Target { get; set; } = target;

			/// <summary>
			/// Value.
			/// </summary>
			[JsonProperty("Value")]
			public string Value { get; set; } = value;
		}

		/// <summary>
		/// Components of a LOC record.
		/// </summary>
		public class LocData(int latitudeDegrees, int latitudeMinutes, double latitudeSeconds, LatitudeDirection latitudeDirection,
			int longitudeDegrees, int longitudeMinutes, double longitudeSeconds, LongitudeDirection longitudeDirection,
			double altitude, int size, int precisionHorizontal, int precisionVertical)
		{
			/// <summary>
			/// Degrees of latitude.
			/// </summary>
			/// <value>Unit: degree. Range: <c>0 &lt;= X &lt;= 90</c></value>
			[JsonProperty("lat_degrees")]
			public int LatitudeDegrees { get; set; } = latitudeDegrees;

			/// <summary>
			/// Minutes of latitude.
			/// </summary>
			/// <value>Unit: minute. Range: <c>0 &lt;= X &lt;= 59</c></value>
			[JsonProperty("lat_minutes")]
			public int LatitudeMinutes { get; set; } = latitudeMinutes;

			/// <summary>
			/// Seconds of latitude.
			/// </summary>
			/// <value>Unit: second. Range: <c>0 &lt;= X &lt;= 59.999</c></value>
			[JsonProperty("lat_seconds")]
			public double LatitudeSeconds { get; set; } = latitudeSeconds;

			/// <summary>
			/// Latitude direction.
			/// </summary>
			[JsonProperty("lat_direction")]
			public LatitudeDirection LatitudeDirection { get; set; } = latitudeDirection;

			/// <summary>
			/// Degrees of longitude.
			/// </summary>
			/// <value>Unit: degree. Range: <c>0 &lt;= X &lt;= 180</c></value>
			[JsonProperty("long_degrees")]
			public int LongitudeDegrees { get; set; } = longitudeDegrees;

			/// <summary>
			/// Minutes of longitude.
			/// </summary>
			/// <value>Unit: minute. Range: <c>0 &lt;= X &lt;= 59</c></value>
			[JsonProperty("long_minutes")]
			public int LongitudeMinutes { get; set; } = longitudeMinutes;

			/// <summary>
			/// Seconds of longitude.
			/// </summary>
			/// <value>Unit: second. Range: <c>0 &lt;= X &lt;= 59.999</c></value>
			[JsonProperty("long_seconds")]
			public double LongitudeSeconds { get; set; } = longitudeSeconds;

			/// <summary>
			/// Longitude direction.
			/// </summary>
			[JsonProperty("long_direction")]
			public LongitudeDirection LongitudeDirection { get; set; } = longitudeDirection;

			/// <summary>
			/// Altitude of location in meters.
			/// </summary>
			/// <value>Unit: meter. Range: <c>-100_000.00 &lt;= X &lt;= 42_849_672.95</c></value>
			[JsonProperty("altitude")]
			public double Altitude { get; set; } = altitude;

			/// <summary>
			/// Size of location in meters.
			/// </summary>
			/// <value>Unit: meter. Range: <c>0 &lt;= X &lt;= 90_000_000</c></value>
			[JsonProperty("size")]
			public int Size { get; set; } = size;

			/// <summary>
			/// Horizontal precision of location.
			/// </summary>
			/// <value>Unit: meter. Range: <c>0 &lt;= X &lt;= 90_000_000</c></value>
			[JsonProperty("precision_horz")]
			public int PrecisionHorizontal { get; set; } = precisionHorizontal;

			/// <summary>
			/// Vertical precision of location.
			/// </summary>
			/// <value>Unit: meter. Range: <c>0 &lt;= X &lt;= 90_000_000</c></value>
			[JsonProperty("precision_vert")]
			public int PrecisionVertical { get; set; } = precisionVertical;
		}

		/// <summary>
		/// Components of a NAPTR record.
		/// </summary>
		public class NaPtrData(string flags, ushort order, ushort preference, string regex, string replacement, string service)
		{
			/// <summary>
			/// Flags.
			/// </summary>
			[JsonProperty("flags")]
			public string Flags { get; set; } = flags;

			/// <summary>
			/// Order.
			/// </summary>
			[JsonProperty("order")]
			public ushort Order { get; set; } = order;

			/// <summary>
			/// Preference.
			/// </summary>
			[JsonProperty("preference")]
			public ushort Preference { get; set; } = preference;

			/// <summary>
			/// Service.
			/// </summary>
			[JsonProperty("regex")]
			public string Regex { get; set; } = regex;

			/// <summary>
			/// Replacement.
			/// </summary>
			[JsonProperty("replacement")]
			public string Replacement { get; set; } = replacement;

			/// <summary>
			/// Service.
			/// </summary>
			[JsonProperty("service")]
			public string Service { get; set; } = service;
		}

		/// <summary>
		/// Components of a SMIMEA record.
		/// </summary>
		public class SMimeAData(string certificate, byte matchingType, byte selector, byte usage)
		{
			/// <summary>
			/// Certificate.
			/// </summary>
			[JsonProperty("certificate")]
			public string Certificate { get; set; } = certificate;

			/// <summary>
			/// Matching type.
			/// </summary>
			[JsonProperty("matching_type")]
			public byte MatchingType { get; set; } = matchingType;

			/// <summary>
			/// Selector.
			/// </summary>
			[JsonProperty("selector")]
			public byte Selector { get; set; } = selector;

			/// <summary>
			/// Usage.
			/// </summary>
			[JsonProperty("usage")]
			public byte Usage { get; set; } = usage;
		}

		/// <summary>
		/// Components of a SRV record.
		/// </summary>
		public class SrvData(ushort port, ushort priority, string target, ushort weight)
		{
			/// <summary>
			/// The port of the service.
			/// </summary>
			[JsonProperty("port")]
			public ushort Port { get; set; } = port;

			/// <summary>
			/// The priority of the service.
			/// </summary>
			/// <remarks>
			/// Required for <see cref="DnsRecordType.Mx"/>, <see cref="DnsRecordType.Srv"/> and <see cref="DnsRecordType.Uri"/> records; unused by other record types.
			/// Records with lower priorities are preferred.
			/// </remarks>
			[JsonProperty("priority")]
			public ushort Priority { get; set; } = priority;

			/// <summary>
			/// A valid hostname.
			/// </summary>
			[JsonProperty("target")]
			public string Target { get; set; } = target;

			/// <summary>
			/// The weight of the record.
			/// </summary>
			[JsonProperty("weight")]
			public ushort Weight { get; set; } = weight;
		}

		/// <summary>
		/// Components of a SSHFP record.
		/// </summary>
		public class SshFpData(byte algorithm, string fingerprint, byte type)
		{
			/// <summary>
			/// Algorithm.
			/// </summary>
			[JsonProperty("algorithm")]
			public byte Algorithm { get; set; } = algorithm;

			/// <summary>
			/// Fingerprint.
			/// </summary>
			[JsonProperty("fingerprint")]
			public string Fingerprint { get; set; } = fingerprint;

			/// <summary>
			/// Type.
			/// </summary>
			[JsonProperty("type")]
			public byte Type { get; set; } = type;
		}

		/// <summary>
		/// Components of a SVCB record.
		/// </summary>
		public class SvcBData(ushort priority, string target, string value)
		{
			/// <summary>
			/// The priority of the service binding.
			/// </summary>
			[JsonProperty("priority")]
			public ushort Priority { get; set; } = priority;

			/// <summary>
			/// A valid target.
			/// </summary>
			[JsonProperty("target")]
			public string Target { get; set; } = target;

			/// <summary>
			/// The value of the service binding.
			/// </summary>
			/// <remarks>
			/// e.g. <c>alpn="h3,h2" ipv4hint="127.0.0.1" ipv6hint="::1"</c>
			/// </remarks>
			[JsonProperty("value")]
			public string Value { get; set; } = value;
		}

		/// <summary>
		/// Components of a TLSA record.
		/// </summary>
		public class TlsAData(string certificate, byte matchingType, byte selector, byte usage)
		{
			/// <summary>
			/// Certificate.
			/// </summary>
			[JsonProperty("certificate")]
			public string Certificate { get; set; } = certificate;

			/// <summary>
			/// Matching type.
			/// </summary>
			[JsonProperty("matching_type")]
			public byte MatchingType { get; set; } = matchingType;

			/// <summary>
			/// Selector.
			/// </summary>
			[JsonProperty("selector")]
			public byte Selector { get; set; } = selector;

			/// <summary>
			/// Usage.
			/// </summary>
			[JsonProperty("usage")]
			public byte Usage { get; set; } = usage;
		}

		/// <summary>
		/// Components of a URI record.
		/// </summary>
		public class UriData(string target, ushort weight)
		{
			/// <summary>
			/// A valid target.
			/// </summary>
			[JsonProperty("target")]
			public string Target { get; set; } = target;

			/// <summary>
			/// The weight of the record.
			/// </summary>
			[JsonProperty("weight")]
			public ushort Weight { get; set; } = weight;
		}

		#region Enums for data fields

		/// <summary>
		/// Directions for latitude.
		/// </summary>
		public enum LatitudeDirection
		{
			/// <summary>
			/// North.
			/// </summary>
			[EnumMember(Value = "N")]
			North = 1,

			/// <summary>
			/// South.
			/// </summary>
			[EnumMember(Value = "S")]
			South = 2
		}

		/// <summary>
		/// Directions for longitude.
		/// </summary>
		public enum LongitudeDirection
		{
			/// <summary>
			/// East.
			/// </summary>
			[EnumMember(Value = "E")]
			East = 1,

			/// <summary>
			/// West.
			/// </summary>
			[EnumMember(Value = "W")]
			West = 2
		}

		#endregion Enums for data fields

		#endregion Type definitions for data fields

		#region Type definitions for settings fields

		/// <summary>
		/// Settings for the DNS record.
		/// </summary>
		public class CnameSettings(bool flattenCname)
		{
			/// <summary>
			/// If enabled, causes the CNAME record to be resolved externally and the resulting address records (e.g., A and AAAA) to be returned instead of the CNAME record itself.
			/// This setting has no effect on proxied records, which are always flattened.
			/// </summary>
			[JsonProperty("flatten_cname")]
			public bool FlattenCname { get; set; } = flattenCname;
		}

		#endregion Type definitions for settings fields
	}
}
