namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Request to create a new zone.
	/// </summary>
	/// <remarks>
	/// Request to create a new zone.
	/// </remarks>
	/// <param name="accountId">The account identifier.</param>
	/// <param name="name">The domain name.</param>
	public class CreateZoneRequest(string accountId, string name)
	{
		/// <summary>
		/// The account identifier.
		/// </summary>
		public string AccountId { get; set; } = accountId;

		/// <summary>
		/// The domain name.
		/// </summary>
		public string Name { get; set; } = name;

		/// <summary>
		/// The zone type.
		/// </summary>
		/// <remarks>
		/// <para>
		/// A full zone implies that DNS is hosted with Cloudflare.
		/// A partial zone is typically a partner-hosted zone or a CNAME setup.
		/// </para>
		/// <para>
		/// If not set, Cloudflare will use <see cref="ZoneType.Full"/> as default.
		/// </para>
		/// </remarks>
		public ZoneType? Type { get; set; }
	}
}
