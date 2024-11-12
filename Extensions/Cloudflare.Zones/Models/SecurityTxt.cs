using System;
using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// The security TXT record.
	/// </summary>
	public class SecurityTxt
	{
		/// <summary>
		/// The acknowledgements.
		/// </summary>
		[JsonProperty("acknowledgements")]
		public IList<string>? Acknowledgements { get; set; }

		/// <summary>
		/// The canonical.
		/// </summary>
		[JsonProperty("canonical")]
		public IList<string>? Canonical { get; set; }

		/// <summary>
		/// The contact.
		/// </summary>
		[JsonProperty("contact")]
		public IList<string>? Contact { get; set; }

		/// <summary>
		/// A value indicating whether this security.txt is enabled.
		/// </summary>
		[JsonProperty("enabled")]
		public bool? Enabled { get; set; }

		/// <summary>
		///The encryption.
		/// </summary>
		[JsonProperty("encryption")]
		public IList<string>? Encryption { get; set; }

		/// <summary>
		/// The expiry.
		/// </summary>
		[JsonProperty("expires")]
		public DateTime? Expires { get; set; }

		/// <summary>
		/// The hiring.
		/// </summary>
		[JsonProperty("hiring")]
		public IList<string>? Hiring { get; set; }

		/// <summary>
		/// The policies.
		/// </summary>
		[JsonProperty("policy")]
		public IList<string>? Policy { get; set; }

		/// <summary>
		/// The preferred languages.
		/// </summary>
		[JsonProperty("preferredLanguages")]
		public string? PreferredLanguages { get; set; }
	}
}
