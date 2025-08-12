# Getting Started

If you want to get your DNS settings and records from Cloudflare (e.g. to check your records for a self developed DynDNS), you could do something like that:

```cs
using AMWD.Net.Api.Cloudflare;
using AMWD.Net.Api.Cloudflare.Dns;
using AMWD.Net.Api.Cloudflare.Zones;

string apiToken = "some-api-token-with-dns-and-zone-permission";
string domain = "example.com";

IAuthentication authenticationData = new ApiTokenAuthentication(apiToken);

using var client = new CloudflareClient(authenticationData);

var zoneFilter = new ListZonesFilter
{
	Status = ZoneStatus.Active,
	Name = domain
};

// This request uses the "Zones" package
var zoneResponse = await client.ListZones(zoneFilter);
if (!zoneResponse.Success)
	throw new ApplicationException(string.Join("\n", zoneResponse.Errors.Select(e => "- " + e.Message)));

var cfZone = zoneResponse.Result
	.Where(z => z.Name == zoneName)
	.FirstOrDefault();
if (cfZone == null)
	throw new ApplicationException($"The zone '{zoneName}' does not exist on Cloudflare.");

// This request uses the "Dns" package
var zoneSettingsResponse = await client.ShowDnsZoneSettings(cfZone.Id, cancellationToken);
if (!zoneSettingsResponse.Success)
	throw new ApplicationException(string.Join("\n", zoneSettingsResponse.Errors.Select(e => "- " + e.Message)));

var recordsFilter = new ListDnsRecordsFilter
{
	PerPage = 1000,
};

// This request uses the "Dns" package
var recordsResponse = await client.ListDnsRecords(cfZone.Id, recordsFilter, cancellationToken);
if (!recordsResponse.Success)
	throw new ApplicationException(string.Join("\n", recordsResponse.Errors.Select(e => "- " + e.Message)));

Console.WriteLine("SOA Record:");
Console.WriteLine(zoneSettingsResponse.Result.SOA);

Console.WriteLine();

Console.WriteLine("DNS Records:");
foreach (var record in recordsResponse.Result)
{
	Console.Write("  ");
	Console.WriteLine(record);
}

```