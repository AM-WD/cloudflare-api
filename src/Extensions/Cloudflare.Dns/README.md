# Cloudflare API for .NET | DNS

This package contains the feature set of the _DNS_ section of the Cloudflare API.

## Implemented Methods

### [Account Custom Nameservers]

- [Add Account Custom Nameserver](https://developers.cloudflare.com/api/resources/custom_nameservers/methods/create/)
- [Delete Account Custom Nameserver](https://developers.cloudflare.com/api/resources/custom_nameservers/methods/delete/)
- [List Account Custom Nameservers](https://developers.cloudflare.com/api/resources/custom_nameservers/methods/get/)


### [DNS]

#### [Analytics]

- [Get Report Table](https://developers.cloudflare.com/api/resources/dns/subresources/analytics/subresources/reports/methods/get/)
- [Get Report By Time](https://developers.cloudflare.com/api/resources/dns/subresources/analytics/subresources/reports/subresources/bytimes/methods/get/)


#### [DNSSEC]

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


#### [Zone Transfers]

##### [ACLs]

- [List ACLs](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/acls/methods/list/)
- [ACL Details](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/acls/methods/get/)
- [Create ACL](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/acls/methods/create/)
- [Update ACL](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/acls/methods/update/)
- [Delete ACL](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/acls/methods/delete/)


##### Force AXFR

- [Force AXFR](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/force_axfr/methods/create/)


##### [Incoming]

- [Create Secondary Zone Configuration](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/incoming/methods/create/)
- [Delete Secondary Zone Configuration](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/incoming/methods/delete/)
- [Secondary Zone Configuration Details](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/incoming/methods/get/)
- [Update Secondary Zone Configuration](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/incoming/methods/update/)


##### [Outgoing]

- [Create Primary Zone Configuration](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/outgoing/methods/create/)
- [Delete Primary Zone Configuration](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/outgoing/methods/delete/)
- [Disable Outgoing Zone Transfers](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/outgoing/methods/disable/)
- [Enable Outgoing Zone Transfers](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/outgoing/methods/enable/)
- [Primary Zone Configuration Details](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/outgoing/methods/get/)
- [Update Primary Zone Configuration](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/outgoing/methods/update/)
- [Force DNS Notify](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/outgoing/methods/force_notify/)
- [Get Outgoing Zone Transfer Status](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/outgoing/subresources/status/methods/get/)


##### [Peers]

- [List Peers](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/peers/methods/list/)
- [Peer Details](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/peers/methods/get/)
- [Create Peer](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/peers/methods/create/)
- [Update Peer](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/peers/methods/update/)
- [Delete Peer](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/peers/methods/delete/)


##### [TSIGs] (Transaction SIGnatures)

- [List TSIGs](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/tsigs/methods/list/)
- [TSIG Details](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/tsigs/methods/get/)
- [Create TSIG](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/tsigs/methods/create/)
- [Update TSIG](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/tsigs/methods/update/)
- [Delete TSIG](https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/tsigs/methods/delete/)


### [DNS Firewall]

- [List DNS Firewall Clusters](https://developers.cloudflare.com/api/resources/dns_firewall/methods/list/)
- [DNS Firewall Cluster Details](https://developers.cloudflare.com/api/resources/dns_firewall/methods/get/)
- [Create DNS Firewall Cluster](https://developers.cloudflare.com/api/resources/dns_firewall/methods/create/)
- [Update DNS Firewall Cluster](https://developers.cloudflare.com/api/resources/dns_firewall/methods/update/)
- [Delete DNS Firewall Cluster](https://developers.cloudflare.com/api/resources/dns_firewall/methods/delete/)


#### [Reverse DNS]

- [Show DNS Firewall Cluster Reverse DNS](https://developers.cloudflare.com/api/resources/dns_firewall/subresources/reverse_dns/methods/get/)
- [Update DNS Firewall Cluster Reverse DNS](https://developers.cloudflare.com/api/resources/dns_firewall/subresources/reverse_dns/methods/edit/)



---

Published under MIT License (see [choose a license])



[choose a license]: https://choosealicense.com/licenses/mit/

[Account Custom Nameservers]: https://developers.cloudflare.com/api/resources/custom_nameservers/

[DNS]: https://developers.cloudflare.com/api/resources/dns/
	[Analytics]: https://developers.cloudflare.com/api/resources/dns/subresources/analytics/
	[DNSSEC]: https://developers.cloudflare.com/api/resources/dns/subresources/dnssec/
	[Records]: https://developers.cloudflare.com/api/resources/dns/subresources/records/
	[Settings]: https://developers.cloudflare.com/api/resources/dns/subresources/settings/
		[Account]: https://developers.cloudflare.com/api/resources/dns/subresources/settings/subresources/account/
		[Zone]: https://developers.cloudflare.com/api/resources/dns/subresources/settings/subresources/zone/
	[Zone Transfers]: https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/
		[ACLs]: https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/acls/
		[Incoming]: https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/incoming/
		[Outgoing]: https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/outgoing/
		[Peers]: https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/peers/
		[TSIGs]: https://developers.cloudflare.com/api/resources/dns/subresources/zone_transfers/subresources/tsigs/
[DNS Firewall]: https://developers.cloudflare.com/api/resources/dns_firewall/
	[Reverse DNS]: https://developers.cloudflare.com/api/resources/dns_firewall/subresources/reverse_dns/
