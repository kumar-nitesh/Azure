using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.WebJob.Continuous
{
    public class Function
    {
        private readonly IConfiguration _config;

        public Function(IConfiguration config)
        {
            _config = config;
        }

        [NoAutomaticTrigger]
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Continuous Web Job has Started !!!");
            var key = _config.GetSection("APPINSIGHTS_INSTRUMENTATIONKEY").Value;
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            Console.WriteLine("The App Insight Key is - " + key);
        }
    }
}
