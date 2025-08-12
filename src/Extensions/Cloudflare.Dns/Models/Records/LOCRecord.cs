using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Location record.
	/// </summary>
	public class LOCRecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LOCRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public LOCRecord(string name)
			: base(name)
		{
			Type = DnsRecordType.LOC;
		}

		/// <summary>
		/// Components of a LOC record.
		/// </summary>
		[JsonProperty("data")]
		public LOCRecordData? Data { get; set; }

		/// <inheritdoc/>
		[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
		public override string ToString()
		{
			return $"{Name}  {TimeToLive}  IN  LOC  {Data?.LatitudeDegrees} {Data?.LatitudeMinutes} {Data?.LatitudeSeconds} {Data?.LatitudeDirection} {Data?.LongitudeDegrees} {Data?.LongitudeMinutes} {Data?.LongitudeSeconds} {Data?.LongitudeDirection} {Data?.Altitude} {Data?.Size} {Data?.PrecisionHorizontal} {Data?.PrecisionVertical}";
		}
	}

	/// <summary>
	/// Components of a LOC record.
	/// </summary>
	public class LOCRecordData
	{
		/// <summary>
		/// Altitude of location in meters.
		/// </summary>
		[JsonProperty("altitude")]
		public double? Altitude { get; set; }

		/// <summary>
		/// Degrees of latitude.
		/// </summary>
		[JsonProperty("lat_degrees")]
		public double? LatitudeDegrees { get; set; }

		/// <summary>
		/// Latitude direction.
		/// </summary>
		[JsonProperty("lat_direction")]
		public LOCRecordLatitudeDirection? LatitudeDirection { get; set; }

		/// <summary>
		/// Minutes of latitude.
		/// </summary>
		[JsonProperty("lat_minutes")]
		public double? LatitudeMinutes { get; set; }

		/// <summary>
		/// Seconds of latitude.
		/// </summary>
		[JsonProperty("lat_seconds")]
		public double? LatitudeSeconds { get; set; }

		/// <summary>
		/// Degrees of longitude.
		/// </summary>
		[JsonProperty("long_degrees")]
		public double? LongitudeDegrees { get; set; }

		/// <summary>
		/// Longitude direction.
		/// </summary>
		[JsonProperty("long_direction")]
		public LOCRecordLongitudeDirection? LongitudeDirection { get; set; }

		/// <summary>
		/// Minutes of longitude.
		/// </summary>
		[JsonProperty("long_minutes")]
		public double? LongitudeMinutes { get; set; }

		/// <summary>
		/// Seconds of longitude.
		/// </summary>
		[JsonProperty("long_seconds")]
		public double? LongitudeSeconds { get; set; }

		/// <summary>
		/// Horizontal precision of location.
		/// </summary>
		[JsonProperty("precision_horz")]
		public double? PrecisionHorizontal { get; set; }

		/// <summary>
		/// Vertical precision of location.
		/// </summary>
		[JsonProperty("precision_vert")]
		public double? PrecisionVertical { get; set; }

		/// <summary>
		/// Size of location in meters.
		/// </summary>
		[JsonProperty("size")]
		public double? Size { get; set; }
	}

	/// <summary>
	/// Location record latitude direction.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum LOCRecordLatitudeDirection
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
	/// Location record longitude direction.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum LOCRecordLongitudeDirection
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
}
