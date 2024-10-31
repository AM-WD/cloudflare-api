using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// A request to edit a zone.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the <see cref="EditZoneRequest"/> class.
	/// </remarks>
	/// <param name="id">The zone identifier.</param>
	public class EditZoneRequest(string id)
	{

		/// <summary>
		/// Identifier.
		/// </summary>
		public string Id { get; set; } = id;

		/// <summary>
		/// A full zone implies that DNS is hosted with Cloudflare. A partial zone is typically a partner-hosted zone or a CNAME setup.
		/// <br/>
		/// <em>This parameter is only available to Enterprise customers or if it has been explicitly enabled on a zone.</em>
		/// </summary>
		public ZoneType? Type { get; set; }

		/// <summary>
		/// An array of domains used for custom name servers.
		/// <br/>
		/// <em>This is only available for Business and Enterprise plans.</em>
		/// </summary>
		public IList<string>? VanityNameServers { get; set; }
	}
}
