# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/)
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Upcoming - 0000-00-00

###### Diffs

- [AMWD.Net.API.Cloudflare](https://github.com/AM-WD/cloudflare-api/compare/v0.1.1...HEAD)
- [AMWD.Net.API.Cloudflare.Dns](https://github.com/AM-WD/cloudflare-api/compare/dns/v0.2.0...HEAD)
- [AMWD.Net.API.Cloudflare.Zones](https://github.com/AM-WD/cloudflare-api/compare/zones/v0.1.1...HEAD)

### Changed

- Updated tests to .NET 10
- Updated solution to VS 2026
- Migrated main repository from Gitlab to Gitea (CI/CD)


## v0.1.1, dns/v0.2.0, zones/v0.1.1 - 2025-11-06

###### Diffs

- [AMWD.Net.API.Cloudflare](https://github.com/AM-WD/cloudflare-api/compare/v0.1.0...v0.1.1)
- [AMWD.Net.API.Cloudflare.Dns](https://github.com/AM-WD/cloudflare-api/compare/dns/v0.1.0...dns/v0.2.0)
- [AMWD.Net.API.Cloudflare.Zones](https://github.com/AM-WD/cloudflare-api/compare/zones/v0.1.0...zones/v0.1.1)

### Added

- (General) New automatic documentation generation using docfx
- (General) Additional articles for the documentation
- (Core) `DateTime` extensions for ISO 8601 formatting
- (DNS) DNS Analytics
- (DNS) Zone Transfers
- (DNS) DNS Firewall

### Changed

- (Zone) Use the new `DateTime` extensions for ISO 8601 formatting


## v0.1.0, dns/v0.1.0, zones/v0.1.0 - 2025-08-05

###### Diffs

- [AMWD.Net.API.Cloudflare](https://github.com/AM-WD/cloudflare-api/commits/v0.1.0)
- [AMWD.Net.API.Cloudflare.Dns](https://github.com/AM-WD/cloudflare-api/commits/dns/v0.1.0)
- [AMWD.Net.API.Cloudflare.Zones](https://github.com/AM-WD/cloudflare-api/commits/zones/v0.1.0)

### Added

- `Cloudflare` with the core functionality to communicate with Cloudflare's API endpoint
- `Cloudflare.Dns` extending the core package with specific methods to manage Cloudflare's DNS settings (_still WIP_)
- `Cloudflare.Zones` extending the core package with specific methods to manage Cloudflare's Domain/Zone Management
