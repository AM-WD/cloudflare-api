using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace AMWD.Net.Api.Cloudflare.Zones
{
	/// <summary>
	/// The zone setting ID.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum ZoneSettingId
	{
		/// <summary>
		/// Advanced protection from Distributed Denial of Service (DDoS) attacks on your
		/// website.
		/// </summary>
		[EnumMember(Value = "advanced_ddos")]
		AdvancedDDoS = 1,

		/// <summary>
		/// Aegis provides dedicated egress IPs (from Cloudflare to your origin) for your
		/// layer 7 WAF and CDN services.
		/// </summary>
		[EnumMember(Value = "aegis")]
		Aegis = 2,

		/// <summary>
		/// When enabled, Cloudflare serves limited copies of web pages available from the
		/// Internet Archive's Wayback Machine if your server is offline.
		/// </summary>
		[EnumMember(Value = "always_online")]
		AlwaysOnline = 3,

		/// <summary>
		/// If enabled, any <c>http://</c> URL is converted to <c>https://</c> through a 301
		/// redirect.
		/// </summary>
		[EnumMember(Value = "always_use_https")]
		AlwaysUseHTTPS = 4,

		/// <summary>
		/// Turn on or off Automatic HTTPS Rewrites.
		/// </summary>
		[EnumMember(Value = "automatic_https_rewrites")]
		AutomaticHTTPSRewrites = 5,

		/// <summary>
		/// Brotli Compression.
		/// </summary>
		[EnumMember(Value = "brotli")]
		Brotli = 6,

		/// <summary>
		/// Control how long resources cached by client browsers remain valid.
		/// </summary>
		[EnumMember(Value = "browser_cache_ttl")]
		BrowserCacheTTL = 7,

		/// <summary>
		/// Inspect the visitor's browser for headers commonly associated with spammers and
		/// certain bots.
		/// </summary>
		[EnumMember(Value = "browser_check")]
		BrowserCheck = 8,

		/// <summary>
		/// Apply custom caching based on the option selected.
		/// </summary>
		[EnumMember(Value = "cache_level")]
		CacheLevel = 9,

		/// <summary>
		/// Specify how long a visitor is allowed access to your site after successfully completing a challenge.
		/// </summary>
		[EnumMember(Value = "challenge_ttl")]
		ChallengeTTL = 10,

		/// <summary>
		/// An allowlist of ciphers for TLS termination.
		/// </summary>
		[EnumMember(Value = "ciphers")]
		Ciphers = 11,

		/// <summary>
		/// Development Mode temporarily allows you to enter development mode for your
		/// websites if you need to make changes to your site.
		/// </summary>
		[EnumMember(Value = "development_mode")]
		DevelopmentMode = 12,

		/// <summary>
		/// When enabled, Cloudflare will attempt to speed up overall page loads.
		/// </summary>
		[EnumMember(Value = "early_hints")]
		EarlyHints = 13,

		/// <summary>
		/// Email obfuscation.
		/// </summary>
		[EnumMember(Value = "email_obfuscation")]
		EmailObfuscation = 14,

		/// <summary>
		/// Enhance your website's font delivery with Cloudflare Fonts.
		/// </summary>
		[EnumMember(Value = "fonts")]
		FontSettings = 15,

		/// <summary>
		/// HTTP/2 Edge Prioritization optimises the delivery of resources served through
		/// HTTP/2 to improve page load performance.
		/// </summary>
		[EnumMember(Value = "h2_prioritization")]
		H2Prioritization = 16,

		/// <summary>
		/// Hotlink Protection option ensures that other sites cannot suck
		/// up your bandwidth by building pages that use images hosted on your site.
		/// </summary>
		[EnumMember(Value = "hotlink_protection")]
		HotlinkProtection = 17,

		/// <summary>
		/// HTTP/2 option.
		/// </summary>
		[EnumMember(Value = "http2")]
		HTTP2 = 18,

		/// <summary>
		/// HTTP/3 option.
		/// </summary>
		[EnumMember(Value = "http3")]
		HTTP3 = 19,

		/// <summary>
		/// Image Transformations provides on-demand resizing, conversion and optimization.
		/// </summary>
		[EnumMember(Value = "image_resizing")]
		ImageResizing = 20,

		/// <summary>
		/// Cloudflare adds a header with the visitor's country.
		/// </summary>
		[EnumMember(Value = "ip_geolocation")]
		IPGeolocation = 21,

		/// <summary>
		/// Enable IPv6 on all subdomains that are Cloudflare enabled.
		/// </summary>
		[EnumMember(Value = "ipv6")]
		IPV6 = 22,

		/// <summary>
		/// Only accepts HTTPS requests that use at least the TLS protocol version specified.
		/// </summary>
		[EnumMember(Value = "min_tls_version")]
		MinTLSVersion = 23,

		/// <summary>
		/// Cloudflare Mirage reduces bandwidth used by images in mobile browsers.
		/// </summary>
		[EnumMember(Value = "mirage")]
		Mirage = 24,

		/// <summary>
		/// Network Error Logging.
		/// </summary>
		[EnumMember(Value = "nel")]
		NEL = 25,

		/// <summary>
		/// Opportunistic Encryption allows browsers to access HTTP URIs over an encrypted TLS channel.
		/// </summary>
		[EnumMember(Value = "opportunistic_encryption")]
		OpportunisticEncryption = 26,

		/// <summary>
		/// Add an Alt-Svc header to all legitimate requests from Tor.
		/// </summary>
		[EnumMember(Value = "opportunistic_onion")]
		OpportunisticOnion = 27,

		/// <summary>
		/// Orange to Orange option.
		/// </summary>
		[EnumMember(Value = "orange_to_orange")]
		OrangeToOrange = 28,

		/// <summary>
		/// Cloudflare error pages generated from issues sent from the origin server.
		/// </summary>
		[EnumMember(Value = "origin_error_page_pass_thru")]
		OriginErrorPagePassThru = 29,

		/// <summary>
		/// Only accepts HTTP requests that use at least the HTTP protocol version specified.
		/// </summary>
		[EnumMember(Value = "origin_max_http_version")]
		OriginMaxHTTPVersion = 30,

		/// <summary>
		/// Polish feature of the Cloudflare Speed app.
		/// </summary>
		[EnumMember(Value = "polish")]
		Polish = 31,

		/// <summary>
		/// Cloudflare will prefetch any URLs that are included in the response headers.
		/// </summary>
		[EnumMember(Value = "prefetch_preload")]
		PrefetchPreload = 32,

		/// <summary>
		/// Proxy Read Timeout.
		/// </summary>
		[EnumMember(Value = "proxy_read_timeout")]
		ProxyReadTimeout = 33,

		/// <summary>
		/// Pseudo IPv4.
		/// </summary>
		[EnumMember(Value = "pseudo_ipv4")]
		PseudoIPV4 = 34,

		/// <summary>
		/// Turn on or off whether Cloudflare should wait for an entire file from the origin.
		/// </summary>
		[EnumMember(Value = "response_buffering")]
		ResponseBuffering = 35,

		/// <summary>
		/// Rocket Loader in the Cloudflare Speed app.
		/// </summary>
		[EnumMember(Value = "rocket_loader")]
		RocketLoader = 36,

		/// <summary>
		/// Security Headers.
		/// </summary>
		[EnumMember(Value = "security_header")]
		SecurityHeaders = 37,

		/// <summary>
		/// Security Level feature from the Security app.
		/// </summary>
		[EnumMember(Value = "security_level")]
		SecurityLevel = 38,

		/// <summary>
		/// Server-Side Excludes.
		/// </summary>
		[EnumMember(Value = "server_side_excludes")]
		ServerSideExcludes = 39,

		/// <summary>
		/// Sort Query String for Cache.
		/// </summary>
		[EnumMember(Value = "sort_query_string_for_cache")]
		SortQueryStringForCache = 40,

		/// <summary>
		/// SSL.
		/// </summary>
		[EnumMember(Value = "ssl")]
		SSL = 41,

		/// <summary>
		/// Enrollment value for SSL/TLS Recommender.
		/// </summary>
		[EnumMember(Value = "ssl_recommender")]
		SSLRecommender = 42,

		/// <summary>
		/// TLS 1.3.
		/// </summary>
		[EnumMember(Value = "tls_1_3")]
		TLS1_3 = 43,

		/// <summary>
		/// TLS Client Authentication.
		/// </summary>
		[EnumMember(Value = "tls_client_auth")]
		TLSClientAuth = 44,

		/// <summary>
		/// True Client IP Header.
		/// </summary>
		[EnumMember(Value = "true_client_ip_header")]
		TrueClientIPHeader = 45,

		/// <summary>
		/// Web Application Firewall.
		/// </summary>
		[EnumMember(Value = "waf")]
		WAF = 46,

		/// <summary>
		/// Cloudflare will serve a WebP version of the original image.
		/// </summary>
		[EnumMember(Value = "webp")]
		WebP = 47,

		/// <summary>
		/// WebSockets settings.
		/// </summary>
		[EnumMember(Value = "websockets")]
		Websocket = 48,

		/// <summary>
		/// 0-RTT
		/// </summary>
		[EnumMember(Value = "0rtt")]
		ZeroRTT = 49,

		/// <summary>
		/// Edge Cache TTL.
		/// </summary>
		[EnumMember(Value = "edge_cache_ttl")]
		SchemasEdgeCacheTTL = 501,

		/// <summary>
		/// Maximum size of an allowable upload.
		/// </summary>
		[EnumMember(Value = "max_upload")]
		MaxUpload = 502,

		/// <summary>
		/// Replace insecure JS.
		/// </summary>
		[EnumMember(Value = "replace_insecure_js")]
		ReplaceInsecureJS = 503,

		/// <summary>
		/// Only allows TLS1.2.
		/// </summary>
		[EnumMember(Value = "tls_1_2_only")]
		TLS1_2Only = 504,

		/// <summary>
		/// How to flatten the cname destination.
		/// </summary>
		[Obsolete("Please use the DNS Settings route instead.")]
		[EnumMember(Value = "cname_flattening")]
		CNAMEFlattening = 901,

		/// <summary>
		/// Privacy Pass.
		/// </summary>
		[Obsolete("Privacy Pass v1 was deprecated in 2023.")]
		[EnumMember(Value = "privacy_pass")]
		PrivacyPass = 902,
	}
}
