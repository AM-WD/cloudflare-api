using System;
using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Request to update security.txt
	/// </summary>
	public class UpdateSecurityTxtRequest(string zoneId)
	{
		/// <summary>
		/// The zone identifier.
		/// </summary>
		public string ZoneId { get; set; } = zoneId;

		/// <summary>
		/// Gets or sets the acknowledgements.
		/// </summary>
		/// <remarks>
		/// Example: <c>https://example.com/hall-of-fame.html</c>
		/// </remarks>
		public IList<string>? Acknowledgements { get; set; }

		/// <summary>
		/// Gets or sets the canonical.
		/// </summary>
		/// <remarks>
		/// Example: <c>https://www.example.com/.well-known/security.txt</c>
		/// </remarks>
		public IList<string>? Canonical { get; set; }

		/// <summary>
		/// Gets or sets the contact.
		/// </summary>
		/// <remarks>
		/// Examples:
		/// <list type="bullet">
		/// <item>mailto:security@example.com</item>
		/// <item>tel:+1-201-555-0123</item>
		/// <item>https://example.com/security-contact.html</item>
		/// </list>
		/// </remarks>
		public IList<string>? Contact { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this security.txt is enabled.
		/// </summary>
		public bool? Enabled { get; set; }

		/// <summary>
		/// Gets or sets the encryption.
		/// </summary>
		/// <remarks>
		/// Examples:
		/// <list type="bullet">
		/// <item>https://example.com/pgp-key.txt</item>
		/// <item>dns:5d2d37ab76d47d36._openpgpkey.example.com?type=OPENPGPKEY</item>
		/// <item>openpgp4fpr:5f2de5521c63a801ab59ccb603d49de44b29100f</item>
		/// </list>
		/// </remarks>
		public IList<string>? Encryption { get; set; }

		/// <summary>
		/// Gets or sets the expires.
		/// </summary>
		/// <remarks>
		/// <strong>NOTE</strong>: The value will be converted to UTC when the <see cref="DateTime.Kind"/> is not <see cref="DateTimeKind.Utc"/>.
		/// </remarks>
		public DateTime? Expires { get; set; }

		/// <summary>
		/// Gets or sets the hiring.
		/// </summary>
		/// <remarks>
		/// Example: <c>https://example.com/jobs.html</c>
		/// </remarks>
		public IList<string>? Hiring { get; set; }

		/// <summary>
		/// Gets or sets the policies.
		/// </summary>
		/// <remarks>
		/// Example: <c>https://example.com/disclosure-policy.html</c>
		/// </remarks>
		public IList<string>? Policy { get; set; }

		/// <summary>
		/// Gets or sets the preferred languages.
		/// </summary>
		/// <remarks>
		/// Examples:
		/// <list type="bullet">
		/// <item>en</item>
		/// <item>es</item>
		/// <item>fr</item>
		/// </list>
		/// </remarks>
		public IList<string>? PreferredLanguages { get; set; }
	}
}
