using Ae.Dns.Client;
using Ae.Dns.Protocol;
using static System.Console;


var proxydomain = "https://REPLACE_ME.workers.dev/";
var testCount = 400;


string[] oisdbig;
using (HttpClient client = new HttpClient())
{
    oisdbig = client.GetStringAsync(
        "https://raw.githubusercontent.com/sjhgvr/oisd/main/domainswild_big.txt").Result
        .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
        .Where(x=>x.StartsWith("*"))
        .Select(x=>x.Substring(2)).ToArray();
}

using var httpClient = new HttpClient { BaseAddress = new Uri(proxydomain) };
using var dnsClient = new DnsHttpClient(httpClient);
var failCount = 0;
string domain = "";
for (int i = 0; i < testCount; i++)
{
    
    try
    {
        domain = oisdbig[Random.Shared.Next(oisdbig.Length)];
        var query = DnsQueryFactory.CreateQuery(domain);
        var answers = dnsClient.Query(query, CancellationToken.None).Result.Answers;
        var result = ((Ae.Dns.Protocol.Records.DnsDomainResource)answers[0].Resource).Domain;
        if (result != "blockpage.nextdns.io")
        {
            throw new Exception("Invalid result");
        }
    }
    catch (Exception ex)
    {
        WriteLine($"Failure on {domain} due to {ex.Message}");
        failCount++;
    }
    
}
if (failCount==0)
{
    WriteLine($"All {testCount} succeeded");
}
else
{
    WriteLine($"{failCount} failure out of {testCount}");
}


