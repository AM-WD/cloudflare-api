# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/)
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased] / [DNS] / [Zones]

### Added

- (General) New automatic documentation generation using docfx
- (General) Additional articles for the documentation
- (Core/Zone) `DateTime` extensions for ISO 8601 formatting
- (DNS) DNS Analytics
- (DNS) Zone Transfers
- (DNS) DNS Firewall

### Changed

- (Core/Zone) Use the new `DateTime` extensions for ISO 8601 formatting


## [v0.1.0], [dns/v0.1.0], [zones/v0.1.0] - 2025-08-05

_Initial Release_

### Added

- `Cloudflare` with the core functionality to communicate with Cloudflare's API endpoint
- `Cloudflare.Dns` extending the core package with specific methods to manage Cloudflare's DNS settings (_still WIP_)
- `Cloudflare.Zones` extending the core package with specific methods to manage Cloudflare's Domain/Zone Management



[Unreleased]: https://github.com/AM-WD/cloudflare-api/compare/v0.1.0...HEAD
[DNS]: https://github.com/AM-WD/cloudflare-api/compare/dns/v0.1.0...HEAD
[Zones]: https://github.com/AM-WD/cloudflare-api/compare/zones/v0.1.0...HEAD

[v0.1.0]: https://github.com/AM-WD/cloudflare-api/commits/v0.1.0

[dns/v0.1.0]: https://github.com/AM-WD/cloudflare-api/commits/dns/v0.1.0

[zones/v0.1.0]: https://github.com/AM-WD/cloudflare-api/commits/zones/v0.1.0
