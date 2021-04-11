using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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