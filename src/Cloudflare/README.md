# Cloudflare API for .NET | Core

This is the core package for all extensions of the Cloudflare API implemented by [AM.WD].

## Contents

- The `(I)CloudflareClient` with base calls for `GET`, `POST`, `PUT`, `PATCH` and `DELETE` requests.
- Base classes to receive responses.
- `CloudflareException` to specify some errors.
- `IAuthentication` implementations to allow API-Token and API-Key (legacy) authentication.

Any specific request will be defined via extension packages.

---

Published under MIT License (see [choose a license])



[AM.WD]: https://www.nuget.org/packages?q=AMWD.&sortby=created-desc
[choose a license]: https://choosealicense.com/licenses/mit/
