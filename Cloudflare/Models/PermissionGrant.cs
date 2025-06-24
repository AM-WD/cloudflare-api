namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// A permission grant.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L488">Source</see>
	/// </summary>
	public class PermissionGrant
	{
		/// <summary>
		/// Whether the member can read the resource.
		/// </summary>
		[JsonProperty("read")]
		public bool? CanRead { get; set; }

		/// <summary>
		/// Whether the member can write to the resource.
		/// </summary>
		[JsonProperty("write")]
		public bool? CanWrite { get; set; }
	}
}
