using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// An audit log entry.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L10">Source</see>
	/// </summary>
	public class AuditLog
	{
		/// <summary>
		/// A string that uniquely identifies the audit log.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// The action that was performed.
		/// </summary>
		[JsonProperty("action")]
		public AuditLogAction? Action { get; set; }

		/// <summary>
		/// The actor that performed the action.
		/// </summary>
		[JsonProperty("actor")]
		public AuditLogActor? Actor { get; set; }

		/// <summary>
		/// The source of the event.
		/// </summary>
		[JsonProperty("interface")]
		public string? Interface { get; set; }

		/// <summary>
		/// An object which can lend more context to the action being logged.
		/// This is a flexible value and varies between different actions.
		/// </summary>
		[JsonProperty("metadata")]
		public object? MetaData { get; set; }

		/// <summary>
		/// The new value of the resource that was modified.
		/// </summary>
		[JsonProperty("newValue")]
		public string? NewValue { get; set; }

		/// <summary>
		/// The value of the resource before it was modified.
		/// </summary>
		[JsonProperty("oldValue")]
		public string? OldValue { get; set; }

		/// <summary>
		/// The owner of the resource that was modified.
		/// </summary>
		[JsonProperty("owner")]
		public AuditLogOwner? Owner { get; set; }

		/// <summary>
		/// The resource that was modified.
		/// </summary>
		[JsonProperty("resource")]
		public AuditLogResource? Resource { get; set; }

		/// <summary>
		/// A UTC RFC3339 timestamp that specifies when the action being logged occured.
		/// </summary>
		[JsonProperty("when")]
		public DateTime? When { get; set; }
	}

	/// <summary>
	/// The action that was performed.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L52">Soruce</see>
	/// </summary>
	public class AuditLogAction
	{
		/// <summary>
		/// A boolean that indicates if the action attempted was successful.
		/// </summary>
		[JsonProperty("result")]
		public bool? Result { get; set; }

		/// <summary>
		/// A short string that describes the action that was performed.
		/// </summary>
		[JsonProperty("type")]
		public string? Type { get; set; }
	}

	/// <summary>
	/// The actor that performed the action.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L64">Source</see>
	/// </summary>
	public class AuditLogActor
	{
		/// <summary>
		/// The ID of the actor that performed the action.
		/// If a user performed the action, this will be the user's ID.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// The email of the user that performed the action.
		/// </summary>
		[JsonProperty("email")]
		public string? Email { get; set; }

		/// <summary>
		/// The IP address of the request that performed the action.
		/// </summary>
		[JsonProperty("ip")]
		public string? IpAddress { get; set; }

		/// <summary>
		/// The type of actor, whether a User, Cloudflare Admin, or an Automated System.
		/// </summary>
		[JsonProperty("type")]
		public AuditLogActorType? Type { get; set; }
	}

	/// <summary>
	/// The type of actor.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L84">Source</see>
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum AuditLogActorType
	{
		/// <summary>
		/// User interaction.
		/// </summary>
		[EnumMember(Value = "user")]
		User = 1,

		/// <summary>
		/// Cloudflare admin interaction.
		/// </summary>
		[EnumMember(Value = "admin")]
		Admin = 2,

		/// <summary>
		/// Cloudflare automated system interaction.
		/// </summary>
		[EnumMember(Value = "Cloudflare")]
		Cloudflare = 3
	}

	/// <summary>
	/// The owner of the resource that was modified.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L87">Source</see>
	/// </summary>
	public class AuditLogOwner
	{
		/// <summary>
		/// Identifier.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }
	}

	/// <summary>
	/// The resource that was modified.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/shared.ts#L94">Source</see>
	/// </summary>
	public class AuditLogResource
	{
		/// <summary>
		/// An identifier for the resource that was affected by the action.
		/// </summary>
		[JsonProperty("id")]
		public string? Id { get; set; }

		/// <summary>
		/// A short string that describes the resource that was affected by the action.
		/// </summary>
		[JsonProperty("type")]
		public string? Type { get; set; }
	}
}
