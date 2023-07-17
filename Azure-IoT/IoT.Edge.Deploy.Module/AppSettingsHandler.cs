using Microsoft.Extensions.Configuration;
using System;

namespace Azure.IoT.Edge.Deploy.Module
{
    public class AppSettingsHandler
    {
        private string _filename;
        private IConfigurationRoot _config;
        private IotEdgeModuleInfo _iotEdgeModuleInfo;
        private IotHub _iotHubDetails;

        public AppSettingsHandler(string filename)
        {
            _filename = filename;
            _config = BuildConfiguration();
            _iotEdgeModuleInfo = GetIotEdgeModuleInfo();
            _iotHubDetails = GetIotHubDetails();
        }

        private IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile(_filename, optional: false, reloadOnChange: true)
               .Build();
        }

        public IotEdgeModuleInfo GetIotEdgeModuleInfo()
        {
            return _config.GetSection("IotEdgeModuleInfo").Get<IotEdgeModuleInfo>();
        }

        public IotHub GetIotHubDetails()
        {
            return _config.GetSection("IotHub").Get<IotHub>();
        }
    }
}