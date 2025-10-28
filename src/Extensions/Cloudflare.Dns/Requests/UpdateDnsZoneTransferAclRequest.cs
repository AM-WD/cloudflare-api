namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to update an existing DNS zone transfer access control list (ACL).
	/// </summary>
	public class UpdateDnsZoneTransferAclRequest : CreateACLRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="UpdateDnsZoneTransferAclRequest"/> class.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="aclId">The access control list identifier.</param>
		/// <param name="ipRange">Allowed IPv4/IPv6 address range of primary or secondary nameservers (CIDR).</param>
		/// <param name="name">The name of the ACL.</param>
		public UpdateDnsZoneTransferAclRequest(string accountId, string aclId, string ipRange, string name)
			: base(accountId, ipRange, name)
		{
			AclId = aclId;
		}

		/// <summary>
		/// The access control list identifier.
		/// </summary>
		public string AclId { get; set; }
	}
}
