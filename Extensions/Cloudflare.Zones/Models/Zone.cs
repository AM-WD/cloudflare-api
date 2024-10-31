using System;
using System.Collections.Generic;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// A DNS Zone.
	/// </summary>
	public class Zone
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		// <= 32 characters
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// The account the zone belongs to.
		/// </summary>
		[JsonProperty("account")]
		public AccountBase? Account { get; set; }

		/// <summary>
		/// The last time proof of ownership was detected and the zone was made active.
		/// </summary>
		[JsonProperty("activated_on")]
		public DateTime ActivatedOn { get; set; }

		/// <summary>
		/// When the zone was created.
		/// </summary>
		[JsonProperty("created_on")]
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// The interval (in seconds) from when development mode expires (positive integer)
		/// or last expired (negative integer) for the domain.
		/// If development mode has never been enabled, this value is 0.
		/// </summary>
		[JsonProperty("development_mode")]
		public int DevelopmentMode { get; set; }

		/// <summary>
		/// Metadata about the zone.
		/// </summary>
		[JsonProperty("meta")]
		public ZoneMetaData? Meta { get; set; }

		/// <summary>
		/// When the zone was last modified.
		/// </summary>
		[JsonProperty("modified_on")]
		public DateTime ModifiedOn { get; set; }

		/// <summary>
		/// The domain name.
		/// </summary>
		// <= 253 characters
		[JsonProperty("name")]
		public string? Name { get; set; }

		/// <summary>
		/// The name servers Cloudflare assigns to a zone.
		/// </summary>
		[JsonProperty("name_servers")]
		public IReadOnlyList<string>? NameServers { get; set; }

		/// <summary>
		/// DNS host at the time of switching to Cloudflare.
		/// </summary>
		[JsonProperty("original_dnshost")]
		public string? OriginalDnshost { get; set; }

		/// <summary>
		/// Original name servers before moving to Cloudflare.
		/// </summary>
		[JsonProperty("original_name_servers")]
		public IReadOnlyList<string>? OriginalNameServers { get; set; }

		/// <summary>
		/// Registrar for the domain at the time of switching to Cloudflare.
		/// </summary>
		[JsonProperty("original_registrar")]
		public string? OriginalRegistrar { get; set; }

		/// <summary>
		/// The owner of the zone.
		/// </summary>
		[JsonProperty("owner")]
		public OwnerBase? Owner { get; set; }

		/// <summary>
		/// Indicates whether the zone is only using Cloudflare DNS services.
		/// A <see langword="true"/> value means the zone will not receive security or performance benefits.
		/// </summary>
		[JsonProperty("paused")]
		public bool Paused { get; set; }

		/// <summary>
		/// The zone status on Cloudflare.
		/// </summary>
		[JsonProperty("status")]
		public ZoneStatus Status { get; set; }

		/// <summary>
		/// A full zone implies that DNS is hosted with Cloudflare.
		/// A partial zone is typically a partner-hosted zone or a CNAME setup..
		/// </summary>
		[JsonProperty("type")]
		public ZoneType Type { get; set; }

		/// <summary>
		/// An array of domains used for custom name servers.
		/// <em>This is only available for Business and Enterprise plans.</em>
		/// </summary>
		[JsonProperty("vanity_name_servers")]
		public IReadOnlyList<string>? VanityNameServers { get; set; }
	}
}
