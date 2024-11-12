using System;
using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones.Internals.Requests
{
	internal class InternalUpdateSecurityTxtRequest
	{
		[JsonProperty("acknowledgements")]
		public IList<string>? Acknowledgements { get; set; }

		[JsonProperty("canonical")]
		public IList<string>? Canonical { get; set; }

		[JsonProperty("contact")]
		public IList<string>? Contact { get; set; }

		[JsonProperty("enabled")]
		public bool? Enabled { get; set; }

		[JsonProperty("encryption")]
		public IList<string>? Encryption { get; set; }

		[JsonProperty("expires")]
		public DateTime? Expires { get; set; }

		[JsonProperty("hiring")]
		public IList<string>? Hiring { get; set; }

		[JsonProperty("policy")]
		public IList<string>? Policy { get; set; }

		[JsonProperty("preferredLanguages")]
		public string? PreferredLanguages { get; set; }
	}
}
