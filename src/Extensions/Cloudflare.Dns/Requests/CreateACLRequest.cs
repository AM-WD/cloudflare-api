using System.Net;
using System.Net.Sockets;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// Represents a request to create a DNS zone transfer access control list (ACL).
	/// </summary>
	public class CreateACLRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CreateACLRequest"/> class.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="ipRange">Allowed IPv4/IPv6 address range of primary or secondary nameservers (CIDR).</param>
		/// <param name="name">The name of the ACL.</param>
		public CreateACLRequest(string accountId, string ipRange, string name)
		{
			IpRangeBaseAddress = IPAddress.None;
			IpRangeSubnet = 0;

			AccountId = accountId;
			IpRange = ipRange;
			Name = name;
		}

		/// <summary>
		/// The account identifier.
		/// </summary>
		public string AccountId { get; set; }

		/// <summary>
		/// Allowed IPv4/IPv6 address range of primary or secondary nameservers.
		/// This will be applied for the entire account.
		/// </summary>
		/// <remarks>
		/// The IP range is used to allow additional NOTIFY IPs for secondary zones and IPs Cloudflare allows AXFR/IXFR requests from for primary zones.
		/// CIDRs are limited to a maximum of /24 for IPv4 and /64 for IPv6 respectively.
		/// </remarks>
		public string IpRange
		{
			get => $"{IpRangeBaseAddress}/{IpRangeSubnet}";
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException(nameof(value), $"{nameof(IpRange)} cannot be null or empty.");

				string[] parts = value.Split('/');
				if (parts.Length != 2)
					throw new FormatException("Invalid IP range format.");

				var prefix = IPAddress.Parse(parts[0]);

				if (!int.TryParse(parts[1], out int prefixLength))
					throw new FormatException("Invalid IP range subnet format.");

				if (prefix.AddressFamily == AddressFamily.InterNetwork && (prefixLength < 0 || 32 < prefixLength))
					throw new FormatException("Invalid subnet length for IPv4.");

				if (prefix.AddressFamily == AddressFamily.InterNetworkV6 && (prefixLength < 0 || 128 < prefixLength))
					throw new FormatException("Invalid subnet length for IPv6.");

				IpRangeBaseAddress = prefix;
				IpRangeSubnet = prefixLength;
			}
		}

		/// <summary>
		/// The name of the ACL.
		/// </summary>
		public string Name { get; set; }

		internal IPAddress IpRangeBaseAddress { get; set; }

		internal int IpRangeSubnet { get; set; }
	}
}
