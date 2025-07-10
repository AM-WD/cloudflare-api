using System.Net.Http;

namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// Defines the interface to add authentication information.
	/// </summary>
	public interface IAuthentication
	{
		/// <summary>
		/// Adds authentication headers to the given <see cref="HttpClient"/>.
		/// </summary>
		/// <param name="httpClient">The <see cref="HttpClient"/> to add the headers to.</param>
		void AddHeader(HttpClient httpClient);
	}
}
