# Cloudflare API for .NET

This project aims to implement the [Cloudflare API] in an extensible way.


## Overview

There should be a package for each API section as defined by Cloudflare.


### [Cloudflare]

This is the core. It contains the base client, that will perform the request itself. It contains the base url and holds the credentials.


### [Cloudflare.Dns]

This package contains the feature set of the _DNS_ section of the Cloudflare API.


### [Cloudflare.Zones]

This package contains the feature set of the _Domain/Zone Management_ section of the Cloudflare API.



---

Published under [MIT License] (see [choose a license])

[![Buy me a Coffee](https://shields.am-wd.de/badge/PayPal-Buy_me_a_Coffee-yellow?style=flat&logo=paypal)](https://link.am-wd.de/donate)



[Cloudflare]: src/Cloudflare/README.md
[Cloudflare.Dns]: src/Extensions/Cloudflare.Dns/README.md
[Cloudflare.Zones]: src/Extensions/Cloudflare.Zones/README.md



[Cloudflare API]: https://developers.cloudflare.com/api/
[MIT License]: LICENSE.txt
[choose a license]: https://choosealicense.com/licenses/mit/
