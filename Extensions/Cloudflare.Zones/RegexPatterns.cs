using System.Text.RegularExpressions;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	internal static class RegexPatterns
	{
		public static readonly Regex ZoneName = new(@"^([a-zA-Z0-9][\-a-zA-Z0-9]*\.)+[\-a-zA-Z0-9]{2,20}$", RegexOptions.Compiled);
	}
}
