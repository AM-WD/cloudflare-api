using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// A zone status.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ZoneStatus
	{
		/// <summary>
		/// Zone is initializing.
		/// </summary>
		[EnumMember(Value = "initializing")]
		Initializing = 1,

		/// <summary>
		/// Zone is pending.
		/// </summary>
		[EnumMember(Value = "pending")]
		Pending = 2,

		/// <summary>
		/// Zone is active.
		/// </summary>
		[EnumMember(Value = "active")]
		Active = 3,

		/// <summary>
		/// Zone has been moved.
		/// </summary>
		[EnumMember(Value = "moved")]
		Moved = 4,
	}
}
