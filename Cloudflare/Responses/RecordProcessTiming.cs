using System;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// Information about the processing time of a file.
	/// </summary>
	public class RecordProcessTiming
	{
		/// <summary>
		/// When the file parsing ended.
		/// </summary>
		[JsonProperty("end_time")]
		public DateTime? EndTime { get; set; }

		/// <summary>
		/// Processing time of the file in seconds.
		/// </summary>
		[JsonProperty("process_time")]
		public int? ProcessTime { get; set; }

		/// <summary>
		/// When the file parsing started.
		/// </summary>
		[JsonProperty("start_time")]
		public DateTime? StartTime { get; set; }
	}
}
