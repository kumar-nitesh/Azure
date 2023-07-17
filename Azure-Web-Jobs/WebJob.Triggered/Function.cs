using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Azure.WebJob.Triggered
{
    public class Function
    {
        private readonly IConfiguration _config;
        private readonly ILogger<Function> _logger;

        public Function(IConfiguration config, ILogger<Function> logger)
        {
            _config = config;
            _logger = logger;
        }

        [FunctionName("Function")]
        public async Task RunAsync([TimerTrigger("%CRONTIME%", RunOnStartup = false)] TimerInfo timerInfo, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Web Job has Triggered !!!");
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            var keyVaultName = _config["KEY_VAULT_NAME"];
            _logger.LogInformation("The Key Vault Name is - " + keyVaultName);
            _logger.LogInformation(timerInfo.GetType().Name);
        }
    }
}
