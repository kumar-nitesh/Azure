using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Azure.IoT.SendMessage
{
    internal class IotDevice
    {
        private DeviceClient _deviceClient;

        public IotDevice(string deviceConnection)
        {
            _deviceClient = DeviceClient.CreateFromConnectionString(deviceConnection, TransportType.Mqtt);
        }

        public async Task SendMessageAsync(DeviceMessage deviceMessage)
        {
            var messageString = JsonConvert.SerializeObject(deviceMessage);

            var message = new Message(Encoding.ASCII.GetBytes(messageString));
            message.MessageId = Guid.NewGuid().ToString();
            message.Properties.Add("vin", "VehicleId");

            await _deviceClient.SendEventAsync(message);
        }
    }
}
