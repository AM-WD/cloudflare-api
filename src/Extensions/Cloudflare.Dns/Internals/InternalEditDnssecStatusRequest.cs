namespace AMWD.Net.Api.Cloudflare.Dns.Internals
{
	internal class InternalEditDnssecStatusRequest
	{
		[JsonProperty("dnssec_multi_signer")]
		public bool? DnssecMultiSigner { get; set; }

		/// <summary>
		/// If <see langword="true"/>, allows Cloudflare to transfer in a DNSSEC-signed zone including signatures from an external provider, without requiring Cloudflare to sign any records on the fly.
		/// </summary>
		/// <remarks>
		/// Note that this feature has some limitations. See <see href="https://developers.cloudflare.com/dns/zone-setups/zone-transfers/cloudflare-as-secondary/setup/#dnssec">Cloudflare as Secondary</see> for details.
		/// </remarks>
		[JsonProperty("dnssec_presigned")]
		public bool? DnssecPresigned { get; set; }

		/// <summary>
		/// If <see langword="true"/>, enables the use of NSEC3 together with DNSSEC on the zone.
		/// </summary>
		/// <remarks>
		/// Combined with setting <see cref="DnssecPresigned"/> to <see langword="true"/>, this enables the use of NSEC3 records when transferring in from an external provider.
		/// If <see cref="DnssecPresigned"/> is instead set to <see langword="false"/> (default), NSEC3 records will be generated and signed at request time.
		/// </remarks>
		[JsonProperty("dnssec_use_nsec3")]
		public bool? DnssecUseNsec3 { get; set; }

		/// <summary>
		/// Status of DNSSEC, based on user-desired state and presence of necessary records.
		/// </summary>
		[JsonProperty("status")]
		public DnssecEditStatus? Status { get; set; }
	}
}
