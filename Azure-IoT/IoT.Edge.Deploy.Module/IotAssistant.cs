using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Azure.IoT.Edge.Deploy.Module
{
    public static class IotAssistant
    {
        public static string GenerateIotHubToken(string iotHubName, string key, string policyName, int expiryInSeconds = 3600)
        {
            var fromEpochStart = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var expiry = Convert.ToString((int)fromEpochStart.TotalSeconds + expiryInSeconds);

            var stringToSign = $"{WebUtility.UrlEncode(iotHubName)}\n{expiry}";

            var hmac = new HMACSHA256(Convert.FromBase64String(key));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));

            var token = $"SharedAccessSignature sr={WebUtility.UrlEncode(iotHubName)}&sig={WebUtility.UrlEncode(signature)}&se={expiry}";

            if (!string.IsNullOrEmpty(policyName))
            {
                token += "&skn=" + policyName;
            }

            return token;
        }

        public static StringContent GetDeploymentManifest(string deploymentId, IotEdgeModuleInfo iotEdgeModuleInfo)
        {
            const string edgeAgent = "$edgeAgent";
            const string registryProperty = "properties.desired.runtime.settings.registryCredentials.acrname";
            string targetDevice = string.Format("deviceId='{0}'", iotEdgeModuleInfo.DeviceId);
            string iotEdgeModuleName = "properties.desired.modules." + iotEdgeModuleInfo.ModuleName;
            string iotEdgeContainerRegistryName = "properties.desired.runtime.settings.registryCredentials." + iotEdgeModuleInfo.ContainerRegistryName;

            var deploymentManifestTemplate = File.ReadAllText("deployment.manifest.json");
            dynamic deploymentManifest = JObject.Parse(deploymentManifestTemplate);
            deploymentManifest.id = deploymentId;
            deploymentManifest.targetCondition = targetDevice;
            deploymentManifest.content.modulesContent[edgeAgent][registryProperty]["address"] = iotEdgeModuleInfo.ContainerRegistryAddress;
            deploymentManifest.content.modulesContent[edgeAgent][registryProperty]["username"] = iotEdgeModuleInfo.ContainerRegistryUsername;
            deploymentManifest.content.modulesContent[edgeAgent][registryProperty]["password"] = iotEdgeModuleInfo.ContainerRegistryPassword;
            deploymentManifest.content.modulesContent[edgeAgent]["properties.desired.modules.appname"]["settings"]["image"] = iotEdgeModuleInfo.ImageUri;

            string manifestPayload = JsonConvert.SerializeObject(deploymentManifest);
            manifestPayload = manifestPayload.Replace("\"properties.desired.modules.appname\":", "\"" + iotEdgeModuleName + "\":");
            manifestPayload = manifestPayload.Replace("\"properties.desired.runtime.settings.registryCredentials.acrname\":", "\"" + iotEdgeContainerRegistryName + "\":");

            return new StringContent(manifestPayload, Encoding.UTF8, "application/json");
        }
    }
}
