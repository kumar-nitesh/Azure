using Microsoft.Extensions.Configuration;
using System;

namespace Azure.IoT.SendMessage
{
    internal class AppSettingsHandler
    {
        private string _filename;
        private IConfigurationRoot _config;
        private string _deviceConnection;

        public AppSettingsHandler(string filename)
        {
            _filename = filename;
            _config = BuildConfiguration();
            _deviceConnection = GetDeviceConnection();
        }

        private IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile(_filename, optional: false, reloadOnChange: true)
               .Build();
        }

        public string GetDeviceConnection()
        {
            return _config.GetConnectionString("DeviceConnectionString");
        }
    }
}