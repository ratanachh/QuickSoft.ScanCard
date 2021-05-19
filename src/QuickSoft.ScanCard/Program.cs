using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QuickSoft.ScanCard.Infrastructure;

namespace QuickSoft.ScanCard
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args);
        }
        
        private static async Task CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                // .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.Development.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseStartup<Startup>()
                .Build();
            await host.RunAsync();
        }

    }
}