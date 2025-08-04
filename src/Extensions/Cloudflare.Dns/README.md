# Cloudflare API for .NET | DNS

This package contains the feature set of the _DNS_ section of the Cloudflare API.

## Implemented Methods

### [Account Custom Nameservers]

- [Add Account Custom Nameserver](https://developers.cloudflare.com/api/resources/custom_nameservers/methods/create/)
- [Delete Account Custom Nameserver](https://developers.cloudflare.com/api/resources/custom_nameservers/methods/delete/)
- [List Account Custom Nameservers](https://developers.cloudflare.com/api/resources/custom_nameservers/methods/get/)


### [DNS]

### [DNSSEC]

- [Delete DNSSEC Records](https://developers.cloudflare.com/api/resources/dns/subresources/dnssec/methods/delete/)
- [Edit DNSSEC Status](https://developers.cloudflare.com/api/resources/dns/subresources/dnssec/methods/edit/)
- [DNSSEC Details](https://developers.cloudflare.com/api/resources/dns/subresources/dnssec/methods/get/)


#### [Records]

- [Batch DNS Records](https://developers.cloudflare.com/api/resources/dns/subresources/records/methods/batch/)
- [Create DNS Record](https://developers.cloudflare.com/api/resources/dns/subresources/records/methods/create/)
- [Delete DNS Record](https://developers.cloudflare.com/api/resources/dns/subresources/records/methods/delete/)
- [Update DNS Record](https://developers.cloudflare.com/api/resources/dns/subresources/records/methods/edit/)
- [Export DNS Records](https://developers.cloudflare.com/api/resources/dns/subresources/records/methods/export/)
- [DNS Record Details](https://developers.cloudflare.com/api/resources/dns/subresources/records/methods/get/)
- [Import DNS Records](https://developers.cloudflare.com/api/resources/dns/subresources/records/methods/import/)
- [List DNS Records](https://developers.cloudflare.com/api/resources/dns/subresources/records/methods/list/)
- [Scan DNS Records](https://developers.cloudflare.com/api/resources/dns/subresources/records/methods/scan/)
- [Overwrite DNS Record](https://developers.cloudflare.com/api/resources/dns/subresources/records/methods/update/)


#### [Settings]

##### [Account]

- [Update DNS Settings](https://developers.cloudflare.com/api/resources/dns/subresources/settings/subresources/account/methods/edit/)
- [Show DNS Settings](https://developers.cloudflare.com/api/resources/dns/subresources/settings/subresources/account/methods/get/)
- [Create Internal DNS View](https://developers.cloudflare.com/api/resources/dns/subresources/settings/subresources/account/subresources/views/methods/create/)
- [Delete Internal DNS View](https://developers.cloudflare.com/api/resources/dns/subresources/settings/subresources/account/subresources/views/methods/delete/)
- [Update Internal DNS View](https://developers.cloudflare.com/api/resources/dns/subresources/settings/subresources/account/subresources/views/methods/edit/)
- [DNS Internal View Details](https://developers.cloudflare.com/api/resources/dns/subresources/settings/subresources/account/subresources/views/methods/get/)
- [List Internal DNS Views](https://developers.cloudflare.com/api/resources/dns/subresources/settings/subresources/account/subresources/views/methods/list/)


##### [Zone]

- [Update DNS Settings](https://developers.cloudflare.com/api/resources/dns/subresources/settings/subresources/zone/methods/edit/)
- [Show DNS Settings](https://developers.cloudflare.com/api/resources/dns/subresources/settings/subresources/zone/methods/get/)



---

Published under MIT License (see [choose a license])



[choose a license]: https://choosealicense.com/licenses/mit/

[Account Custom Nameservers]: https://developers.cloudflare.com/api/resources/custom_nameservers/

[DNS]: https://developers.cloudflare.com/api/resources/dns/
	[DNSSEC]: https://developers.cloudflare.com/api/resources/dns/subresources/dnssec/
	[Records]: https://developers.cloudflare.com/api/resources/dns/subresources/records/
	[Settings]: https://developers.cloudflare.com/api/resources/dns/subresources/settings/
		[Account]: https://developers.cloudflare.com/api/resources/dns/subresources/settings/subresources/account/
		[Zone]: https://developers.cloudflare.com/api/resources/dns/subresources/settings/subresources/zone/
