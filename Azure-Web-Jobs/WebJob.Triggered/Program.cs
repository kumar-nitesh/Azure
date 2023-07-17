using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.WebJob.Triggered
{
    internal class Program
    {
        private static IConfiguration ExtensionConfiguration { get; set; }

        static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder();

            hostBuilder.ConfigureWebJobs(builder =>
            {
                builder.AddAzureStorageCoreServices();
                builder.AddAzureStorageBlobs();
                builder.AddTimers();
            });

            hostBuilder.ConfigureServices((context, services) =>
            {
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                ExtensionConfiguration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    //.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    //.AddAzureKeyVault()
                    .Build();

                services.AddMemoryCache();
                services.AddSingleton(ExtensionConfiguration);
                services.AddScoped<Function, Function>();
                services.BuildServiceProvider();
            });

            hostBuilder.ConfigureLogging(logBuilder =>
            {
                logBuilder.AddConsole();
                string key = ExtensionConfiguration["APPINSIGHTS_INSTRUMENTATIONKEY"];
                if (!string.IsNullOrEmpty(key))
                {
                    logBuilder.AddApplicationInsights(key).SetMinimumLevel(LogLevel.Information);
                    logBuilder.AddApplicationInsightsWebJobs(o => { o.InstrumentationKey = key; });
                }
            });

            var cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            var buildHost = hostBuilder.Build();

            using (buildHost)
            {
                await buildHost.RunAsync(cancellationToken);
                cancellationTokenSource.Dispose();
            }
        }
    }
}
