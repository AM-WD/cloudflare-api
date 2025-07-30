namespace AMWD.Net.Api.Cloudflare
{
	/// <summary>
	/// Represents filter options defined via query parameters.
	/// </summary>
	public interface IQueryParameterFilter
	{
		/// <summary>
		/// Gets the query parameters.
		/// </summary>
		IReadOnlyDictionary<string, string> GetQueryParameters();
	}
}
