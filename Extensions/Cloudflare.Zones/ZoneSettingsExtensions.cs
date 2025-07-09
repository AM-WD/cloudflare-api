using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Extensions for <see href="https://developers.cloudflare.com/api/resources/zones/subresources/settings/">Zone Settings</see>.
	/// </summary>
	public static class ZoneSettingsExtensions
	{
		/// <summary>
		/// Edit settings for a zone.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		[Obsolete("This endpoint is deprecated. Zone settings should instead be managed individually.")]
		public static Task<CloudflareResponse<IReadOnlyCollection<ZoneSettingBase>>> EditMultipleZoneSettings(this ICloudflareClient client, EditMultipleZoneSettingsRequest request, CancellationToken cancellationToken = default)
		{
			request.ZoneId.ValidateCloudflareId();

			return client.PatchAsync<IReadOnlyCollection<ZoneSettingBase>, IReadOnlyCollection<ZoneSettingBase>>($"/zones/{request.ZoneId}/settings", request.Settings, cancellationToken);
		}

		/// <summary>
		/// Updates a single zone setting by the identifier.
		/// </summary>
		/// <typeparam name="T">The type of the zone setting.</typeparam>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="request">The request.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<T>> EditZoneSetting<T>(this ICloudflareClient client, EditZoneSettingRequest<T> request, CancellationToken cancellationToken = default)
			where T : ZoneSettingBase
		{
			request.ZoneId.ValidateCloudflareId();
			string settingId = request.Setting.Id?.GetEnumMemberValue()
				?? throw new ArgumentException("The zone setting type is not known.");

			var req = new JObject();

			if (request.Enabled.HasValue)
				req.Add("enabled", request.Enabled.Value);
			else
				req.Add("value", JObject.FromObject(request.Setting)["value"]);

			return client.PatchAsync<T, JObject>($"/zones/{request.ZoneId}/settings/{settingId}", req, cancellationToken);
		}

		/// <summary>
		/// Fetch a single zone setting.
		/// </summary>
		/// <typeparam name="T">The type of the zone setting.</typeparam>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		public static Task<CloudflareResponse<T>> GetZoneSetting<T>(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
			where T : ZoneSettingBase
		{
			zoneId.ValidateCloudflareId();

			string settingId = ((ZoneSettingBase)Activator.CreateInstance(typeof(T))).Id?.GetEnumMemberValue()
				?? throw new ArgumentException("The zone setting type is not known.");

			return client.GetAsync<T>($"/zones/{zoneId}/settings/{settingId}", null, cancellationToken);
		}

		/// <summary>
		/// Available settings for your user in relation to a zone.
		/// </summary>
		/// <param name="client">The <see cref="ICloudflareClient"/> instance.</param>
		/// <param name="zoneId">The zone identifier.</param>
		/// <param name="cancellationToken">A cancellation token used to propagate notification that this operation should be canceled.</param>
		[Obsolete("This endpoint is deprecated. Zone settings should instead be managed individually.")]
		public static Task<CloudflareResponse<IReadOnlyCollection<ZoneSettingBase>>> GetAllZoneSettings(this ICloudflareClient client, string zoneId, CancellationToken cancellationToken = default)
		{
			zoneId.ValidateCloudflareId();

			return client.GetAsync<IReadOnlyCollection<ZoneSettingBase>>($"/zones/{zoneId}/settings", null, cancellationToken);
		}
	}
}
