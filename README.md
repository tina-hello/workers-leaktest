# workers-leaktest
Test if Workers actually leaking queries when used to proxy against NextDNS.

* Create a new Workers using [doh-cf-workers](https://github.com/tina-hello/doh-cf-workers), set the upstream to your own NextDNS profile that **includes** the OISD list, with **Block Page enabled** in the setting.
* Install [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* Clone this project, change the `proxydomain` inside [Program.cs](/Program.cs) with your own Worker address
* Run with `dotnet run` from the terminal

A normal run should only output `All 400 succeeded` (you can change `testcount` from 400 if you want, but a too large number might trigger Cloudflare/NextDNS DDoS protection), if you get a different result, open an issue.
