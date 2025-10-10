# Cloudflare API for .NET | Zones

This package contains the feature set of the _Domain/Zone Management_ section of the Cloudflare API.

## Implemented Methods

### [Registrar]

- [Get Domain](https://developers.cloudflare.com/api/resources/registrar/subresources/domains/methods/get/)
- [List Domains](https://developers.cloudflare.com/api/resources/registrar/subresources/domains/methods/list/)
- [Update Domain](https://developers.cloudflare.com/api/resources/registrar/subresources/domains/methods/update/)


### [Zones]

- [Create Zone](https://developers.cloudflare.com/api/resources/zones/methods/create/)
- [Delete Zone](https://developers.cloudflare.com/api/resources/zones/methods/delete/)
- [Edit Zone](https://developers.cloudflare.com/api/resources/zones/methods/edit/)
- [Zone Details](https://developers.cloudflare.com/api/resources/zones/methods/get/)
- [List Zones](https://developers.cloudflare.com/api/resources/zones/methods/list/)


#### [Activation Check]

- [Rerun The Activation Check](https://developers.cloudflare.com/api/resources/zones/subresources/activation_check/methods/trigger/)


#### Custom Nameservers

_This part of the API is marked as deprecated and therefore not implemented._


#### [Holds]

- [Create Zone Hold](https://developers.cloudflare.com/api/resources/zones/subresources/holds/methods/create/)
- [Remove Zone Hold](https://developers.cloudflare.com/api/resources/zones/subresources/holds/methods/delete/)
- [Update Zone Hold](https://developers.cloudflare.com/api/resources/zones/subresources/holds/methods/edit/)
- [Get Zone Hold](https://developers.cloudflare.com/api/resources/zones/subresources/holds/methods/get/)


#### [Plans]

- [Available Plan Details](https://developers.cloudflare.com/api/resources/zones/subresources/plans/methods/get/)
- [List Available Plans](https://developers.cloudflare.com/api/resources/zones/subresources/plans/methods/list/)


#### [Rate Plans]

- [List Available Rate Plans](https://developers.cloudflare.com/api/resources/zones/subresources/rate_plans/methods/get/)


#### [Settings]

- **DEPRECATED** [Edit Multiple Zone Settings](https://developers.cloudflare.com/api/resources/zones/subresources/settings/methods/bulk_edit/)
- [Edit Zone Setting](https://developers.cloudflare.com/api/resources/zones/subresources/settings/methods/edit/)
- [Get Zone Setting](https://developers.cloudflare.com/api/resources/zones/subresources/settings/methods/get/)
- **DEPRECATED** [Get All Zone Settings](https://developers.cloudflare.com/api/resources/zones/subresources/settings/methods/list/)


#### [Subscriptions]

- [Create Zone Subscription](https://developers.cloudflare.com/api/resources/zones/subresources/subscriptions/methods/create/)
- [Zone Subscription Details](https://developers.cloudflare.com/api/resources/zones/subresources/subscriptions/methods/get/)
- [Update Zone Subscription](https://developers.cloudflare.com/api/resources/zones/subresources/subscriptions/methods/update/)



---

Published under MIT License (see [choose a license])



[choose a license]: https://choosealicense.com/licenses/mit/

[Registrar]: https://developers.cloudflare.com/api/resources/registrar/
[Zones]: https://developers.cloudflare.com/api/resources/zones/
[Activation Check]: https://developers.cloudflare.com/api/resources/zones/subresources/activation_check/
[Holds]: https://developers.cloudflare.com/api/resources/zones/subresources/holds/
[Plans]: https://developers.cloudflare.com/api/resources/zones/subresources/plans/
[Rate Plans]: https://developers.cloudflare.com/api/resources/zones/subresources/rate_plans/
[Settings]: https://developers.cloudflare.com/api/resources/zones/subresources/settings/
[Subscriptions]: https://developers.cloudflare.com/api/resources/zones/subresources/subscriptions/
