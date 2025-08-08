# Introduction

A small private project was the start for this development.

I wanted to manage several domains registered with different providers together under one interface.

Unfortunately, there is no official Cloudflare library for .NET, so I decided to implement my own...

... and here it is! :D

---

The goal is to allow a modular implementation of the Cloudflare API.

Each NuGet package can stand alone and represents a section of the developer documentation.    
The basics (client, authentication, ...) are implemented in a core package: [AMWD.Net.API.Cloudflare](https://www.nuget.org/packages/AMWD.Net.API.Cloudflare).    
On this base, the other packages can be loaded, which then extend the client with corresponding methods.
