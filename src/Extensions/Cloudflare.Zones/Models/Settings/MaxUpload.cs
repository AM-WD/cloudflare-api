namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// Maximum size of an allowable upload.
	/// </summary>
	public class MaxUpload : ZoneSettingBase
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="MaxUpload"/> class.
		/// </summary>
		public MaxUpload()
		{
			Id = ZoneSettingId.MaxUpload;
		}

		/// <summary>
		/// Current value of the zone setting.
		/// </summary>
		[JsonProperty("value")]
		public MaxUploadSize Value { get; set; }
	}

	/// <summary>
	/// Maximum size of an allowable upload.
	/// <see href="https://github.com/cloudflare/cloudflare-typescript/blob/v4.4.1/src/resources/zones/settings.ts#L1980">Soruce</see>
	/// </summary>
	public enum MaxUploadSize : int
	{
		/// <summary>
		/// 100 MB.
		/// </summary>
		M100 = 100,

		/// <summary>
		/// 125 MB.
		/// </summary>
		M125 = 125,

		/// <summary>
		/// 150 MB.
		/// </summary>
		M150 = 150,

		/// <summary>
		/// 175 MB.
		/// </summary>
		M175 = 175,

		/// <summary>
		/// 200 MB.
		/// </summary>
		M200 = 200,

		/// <summary>
		/// 225 MB.
		/// </summary>
		M225 = 225,

		/// <summary>
		/// 250 MB.
		/// </summary>
		M250 = 250,

		/// <summary>
		/// 275 MB.
		/// </summary>
		M275 = 275,

		/// <summary>
		/// 300 MB.
		/// </summary>
		M300 = 300,

		/// <summary>
		/// 325 MB.
		/// </summary>
		M325 = 325,

		/// <summary>
		/// 350 MB.
		/// </summary>
		M350 = 350,

		/// <summary>
		/// 375 MB.
		/// </summary>
		M375 = 375,

		/// <summary>
		/// 400 MB.
		/// </summary>
		M400 = 400,

		/// <summary>
		/// 425 MB.
		/// </summary>
		M425 = 425,

		/// <summary>
		/// 450 MB.
		/// </summary>
		M450 = 450,

		/// <summary>
		/// 475 MB.
		/// </summary>
		M475 = 475,

		/// <summary>
		/// 500 MB.
		/// </summary>
		M500 = 500,

		/// <summary>
		/// 1000 MB (1 GB).
		/// </summary>
		G1 = 1000
	}
}
