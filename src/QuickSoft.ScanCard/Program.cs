using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using QuickSoft.ScanCard;

var config = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();
    
var host = new WebHostBuilder()
    .UseConfiguration(config)
    .UseKestrel()
    .UseUrls("http://+:5000")
    .UseStartup<Startup>()
    .Build();
    
await host.RunAsync();