using System;
using System.Net.Http;

namespace Azure.IoT.Edge.Deploy.Module
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iot Edge - Module Deployment");

            var appSettings = new AppSettingsHandler("appsettings.json");
            var iotEdgeModuleInfo = appSettings.GetIotEdgeModuleInfo();
            var iotHubDetails = appSettings.GetIotHubDetails();

            var deploymentId = Guid.NewGuid().ToString();

            var manifestContent = IotAssistant.GetDeploymentManifest(deploymentId, iotEdgeModuleInfo);
            var iothubToken = IotAssistant.GenerateIotHubToken(iotHubDetails.IotHubName, iotHubDetails.IotHubKey, iotHubDetails.PolicyName);

            AddModule(deploymentId, iotHubDetails.IotHubName, iothubToken, manifestContent);
            DeleteModule(deploymentId, iotHubDetails.IotHubName, iothubToken);
        }

        private static void AddModule(string deploymentId, string iotHubName, string iothubToken, StringContent manifestContent)
        {
            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("Authorization", iothubToken);
            string deployModuleUri = $"https://{iotHubName}/configurations/{deploymentId}?api-version=2020-05-31-preview";
            HttpResponseMessage httpResponseMessage = httpClient.PutAsync(deployModuleUri, manifestContent).Result;
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Console.WriteLine("Module is successfully added to the IoT Edge.");
            }
        }

        private static void DeleteModule(string deploymentId, string iotHubName, string iothubToken)
        {
            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("Authorization", iothubToken);
            string deleteConfigurationAPI = $"https://{iotHubName}/configurations/{deploymentId}?api-version=2020-05-31-preview";
            HttpResponseMessage httpResponseMessage = httpClient.DeleteAsync(deleteConfigurationAPI).Result;

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Console.WriteLine("Module is successfully deleted from the IoT Edge.");
            }
        }
    }
}
