namespace AMWD.Net.Api.Cloudflare.Dns
{
	/// <summary>
	/// SSH Public Key Fingerprint record.
	/// </summary>
	public class SSHFPRecord : DnsRecord
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SSHFPRecord"/> class.
		/// </summary>
		/// <param name="name">DNS record name (or @ for the zone apex) in Punycode.</param>
		public SSHFPRecord(string name)
			: base(name)
		{
			Type = DnsRecordType.SSHFP;
		}

		/// <summary>
		/// Components of a SSHFP record.
		/// </summary>
		[JsonProperty("data")]
		public SSHFPRecordData? Data { get; set; }
	}

	/// <summary>
	/// Components of a SSHFP record.
	/// </summary>
	public class SSHFPRecordData
	{
		/// <summary>
		/// Algorithm.
		/// </summary>
		[JsonProperty("algorithm")]
		public int? Algorithm { get; set; }

		/// <summary>
		/// Fingerprint.
		/// </summary>
		[JsonProperty("fingerprint")]
		public string? Fingerprint { get; set; }

		/// <summary>
		/// Type.
		/// </summary>
		[JsonProperty("type")]
		public int? Type { get; set; }
	}
}
