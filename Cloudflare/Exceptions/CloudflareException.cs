using System;
using System.Runtime.Serialization;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// Represents errors that occur during Cloudflare API calls.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
	public class CloudflareException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CloudflareException"/> class.
		/// </summary>
		public CloudflareException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CloudflareException"/> class with a specified error
		/// message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public CloudflareException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CloudflareException"/> class with a specified error
		/// message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception, or a <see langword="null"/> reference
		/// if no inner exception is specified.
		/// </param>
		public CloudflareException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CloudflareException"/> class with serialized data.
		/// </summary>
		/// <param name="info">
		/// The <see cref="SerializationInfo"/> that holds the serialized
		/// object data about the exception being thrown.
		/// </param>
		/// <param name="context">
		/// The <see cref="StreamingContext"/> that contains contextual information
		/// about the source or destination.
		/// </param>
		/// <exception cref="ArgumentNullException">The info parameter is null.</exception>
		/// <exception cref="SerializationException">The class name is <see langword="null"/> or <see cref="Exception.HResult"/> is zero (0).</exception>
		protected CloudflareException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
