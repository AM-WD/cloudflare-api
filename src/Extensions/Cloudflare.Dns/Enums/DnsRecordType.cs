using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// DNS record types.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/dns/records.ts">Source</see>
	/// </summary>
	/// <remarks>
	/// A list with short description can be found <see href="https://en.wikipedia.org/wiki/List_of_DNS_record_types">@wikipedia</see>.
	/// </remarks>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum DnsRecordType : int
	{
		/// <summary>
		/// Address record.
		/// </summary>
		/// <remarks>
		/// Returns a 32-bit IPv4 address, most commonly used to map hostnames to an IP address of the host, but it is also used for DNSBLs, storing subnet masks in RFC 1101, etc.
		/// <code>
		/// example.com.		3600	IN	A	96.7.128.175
		/// </code>
		/// </remarks>
		[EnumMember(Value = "A")]
		A = 1,

		/// <summary>
		/// IPv6 address record.
		/// </summary>
		/// <remarks>
		/// Returns a 128-bit IPv6 address, most commonly used to map hostnames to an IP address of the host.
		/// <code>
		/// example.com.		3600	IN	AAAA	2600:1408:ec00:36::1736:7f31
		/// </code>
		/// </remarks>
		[EnumMember(Value = "AAAA")]
		AAAA = 28,

		/// <summary>
		/// Certification Authority Authorization.
		/// </summary>
		/// <remarks>
		/// DNS Certification Authority Authorization, constraining acceptable CAs for a host/domain.
		/// <code>
		/// example.com.		604800	IN	CAA	0 issue "letsencrypt.org"
		/// </code>
		/// </remarks>
		[EnumMember(Value = "CAA")]
		CAA = 257,

		/// <summary>
		/// Certificate record.
		/// </summary>
		/// <remarks>
		/// Stores PKIX, SPKI, PGP, etc.
		/// <code>
		/// example.com.		86400	IN	CERT	2 77 2 TUlJQ1l6Q0NBY3lnQXdJQkFnSUJBREFOQmdrcWh
		/// </code>
		/// </remarks>
		[EnumMember(Value = "CERT")]
		CERT = 37,

		/// <summary>
		/// Canonical name record.
		/// </summary>
		/// <remarks>
		/// Alias of one name to another: the DNS lookup will continue by retrying the lookup with the new name.
		/// <code>
		/// autodiscover.example.com.		86400	IN	CNAME	mail.example.com.
		/// </code>
		/// </remarks>
		[EnumMember(Value = "CNAME")]
		CNAME = 5,

		/// <summary>
		/// DNS Key record.
		/// </summary>
		/// <remarks>
		/// The key record used in DNSSEC. Uses the same format as the KEY record.
		/// <code>
		/// example.com.		3600	IN	DNSKEY	256 3 13 OtuN/SL9sE+SDQ0tOLeezr1KzUNi77FflTjxQylUhm3V7m13Vz9tYQuc SGK0pyxISo9CQsszubAwJSypq3li3g==
		/// </code>
		/// </remarks>
		[EnumMember(Value = "DNSKEY")]
		DNSKEY = 48,

		/// <summary>
		/// Delegation signer.
		/// </summary>
		/// <remarks>
		/// The record used to identify the DNSSEC signing key of a delegated zone.
		/// <code>
		/// example.com.		86400	IN	DS	370 13 2 BE74359954660069D5C63D200C39F5603827D7DD02B56F120EE9F3A8 6764247C
		/// </code>
		/// </remarks>
		[EnumMember(Value = "DS")]
		DS = 43,

		/// <summary>
		/// HTTPS Binding.
		/// </summary>
		/// <remarks>
		/// RR that improves performance for clients that need to resolve many resources to access a domain.
		/// <code>
		/// example.com.		3600	IN	HTTPS	1 svc.example.com. alpn=h2
		/// </code>
		/// </remarks>
		[EnumMember(Value = "HTTPS")]
		HTTPS = 65,

		/// <summary>
		/// Location record.
		/// </summary>
		/// <remarks>
		/// Specifies a geographical location associated with a domain name.
		/// <code>
		/// SW1A2AA.find.me.uk.		2592000	IN	LOC	51 30 12.748 N 0 7 39.611 W 0.00m 0.00m 0.00m 0.00m
		/// </code>
		/// </remarks>
		[EnumMember(Value = "LOC")]
		LOC = 29,

		/// <summary>
		/// Mail exchange record.
		/// </summary>
		/// <remarks>
		/// List of mail exchange servers that accept email for a domain.
		/// <code>
		/// example.com.		43200	IN	MX	0 mail.example.com.
		/// </code>
		/// </remarks>
		[EnumMember(Value = "MX")]
		MX = 15,

		/// <summary>
		/// Naming Authority Pointer.
		/// </summary>
		/// <remarks>
		/// Allows regular-expression-based rewriting of domain names which can then be used as URIs, further domain names to lookups, etc.
		/// <code>
		/// example.com.		86400	IN	NAPTR	100 10 "S" "SIP+D2T" "" _sip._tcp.example.com.
		/// </code>
		/// </remarks>
		[EnumMember(Value = "NAPTR")]
		NAPTR = 35,

		/// <summary>
		/// Name server record.
		/// </summary>
		/// <remarks>
		/// Delegates a DNS zone to use the given authoritative name servers.
		/// <code>
		/// example.com.		86400	IN	NS	a.iana-servers.net.
		/// </code>
		/// </remarks>
		[EnumMember(Value = "NS")]
		NS = 2,

		/// <summary>
		/// OpenPGP public key record.
		/// </summary>
		/// <remarks>
		/// A DNS-based Authentication of Named Entities (DANE) method for publishing and locating OpenPGP
		/// public keys in DNS for a specific email address using an OPENPGPKEY DNS resource record.
		/// <code>
		/// 00d8d3f11739d2f3537099982b4674c29fc59a8fda350fca1379613a._openpgpkey.example.com.		3600	IN	OPENPGPKEY	a2V5S0VZMTIzNGtleUtFWQ==
		/// </code>
		/// </remarks>
		[EnumMember(Value = "OPENPGPKEY")]
		OPENPGPKEY = 61,

		/// <summary>
		/// PTR Resource Record.
		/// </summary>
		/// <remarks>
		/// Pointer to a canonical name.
		/// Unlike a CNAME, DNS processing stops and just the name is returned.
		/// The most common use is for implementing reverse DNS lookups, but other uses include such things as DNS-SD.
		/// <code>
		/// 14.215.184.93.in-addr.arpa.		86400	IN	PTR	example.com.
		/// </code>
		/// </remarks>
		[EnumMember(Value = "PTR")]
		PTR = 12,

		/// <summary>
		/// S/MIME cert association.
		/// </summary>
		/// <remarks>
		/// Associates an S/MIME certificate with a domain name for sender authentication.
		/// <code>
		/// example.com.		3600	IN	SMIMEA	0 0 0 keyKEY1234keyKEY
		/// </code>
		/// </remarks>
		[EnumMember(Value = "SMIMEA")]
		SMIMEA = 53,

		/// <summary>
		/// Service locator.
		/// </summary>
		/// <remarks>
		/// Generalized service location record, used for newer protocols instead of creating protocol-specific records such as MX.
		/// <code>
		/// _autodiscover._tcp.example.com.		604800	IN	SRV	1 0 443 mail.example.com.
		/// </code>
		/// </remarks>
		[EnumMember(Value = "SRV")]
		SRV = 33,

		/// <summary>
		/// SSH Public Key Fingerprint.
		/// </summary>
		/// <remarks>
		/// Resource record for publishing SSH public host key fingerprints in the DNS, in order to aid in verifying the authenticity of the host.
		/// RFC 6594 defines ECC SSH keys and SHA-256 hashes. See the IANA SSHFP RR parameters registry for details.
		/// <code>
		/// example.com.		600	IN	SSHFP	2 1 123456789abcdef67890123456789abcdef67890
		/// </code>
		/// </remarks>
		[EnumMember(Value = "SSHFP")]
		SSHFP = 44,

		/// <summary>
		/// Service Binding.
		/// </summary>
		/// <remarks>
		/// RR that improves performance for clients that need to resolve many resources to access a domain.
		/// <code>
		/// example.com.		3600	IN	SVCB	1 . alpn="h2,http/1.1"
		/// </code>
		/// </remarks>
		[EnumMember(Value = "SVCB")]
		SVCB = 64,

		/// <summary>
		/// TLSA certificate association.
		/// </summary>
		/// <remarks>
		/// A record for DANE. RFC 6698 defines "The TLSA DNS resource record is used to associate a TLS server certificate
		/// or public key with the domain name where the record is found, thus forming a 'TLSA certificate association'".
		/// <code>
		/// _443._tcp.example.com.		3600	IN	TLSA	3 0 18cb0fc6c527506a053f4f14c8464bebbd6dede2738d11468dd953d7d6a3021f1
		/// </code>
		/// </remarks>
		[EnumMember(Value = "TLSA")]
		TLSA = 52,

		/// <summary>
		/// Text record.
		/// </summary>
		/// <remarks>
		/// Originally for arbitrary human-readable text in a DNS record.
		/// Since the early 1990s, however, this record more often carries machine-readable data, such as specified by RFC 1464,
		/// opportunistic encryption, Sender Policy Framework, DKIM, DMARC, DNS-SD, etc.
		/// <br/>
		/// More information about TXT records on <see href="https://www.cloudflare.com/learning/dns/dns-records/dns-txt-record/">Cloudflare</see>.
		/// <code>
		/// example.com.		86400	IN	TXT	"v=spf1 -all"
		/// </code>
		/// </remarks>
		[EnumMember(Value = "TXT")]
		TXT = 16,

		/// <summary>
		/// Uniform Resource Identifier.
		/// </summary>
		/// <remarks>
		/// Can be used for publishing mappings from hostnames to URIs.
		/// <code>
		/// _ftp._tcp.example.com.		3600	IN	URI	10 1 "ftp://ftp.example.com/public"
		/// </code>
		/// </remarks>
		[EnumMember(Value = "URI")]
		URI = 256,
	}
}
