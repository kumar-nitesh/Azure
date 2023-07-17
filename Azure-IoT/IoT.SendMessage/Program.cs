using System;
using System.Threading.Tasks;

namespace Azure.IoT.SendMessage
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            Console.WriteLine("IoT Hub - Sending Messages");

            var appSettings = new AppSettingsHandler("appsettings.json");
            var deviceConnection = appSettings.GetDeviceConnection();

            var deviceMessage = new DeviceMessage
            {
                DeviceId = "device",
                Timestamp = DateTime.Now
            };

            var deviceClient = new IotDevice(deviceConnection);
            await deviceClient.SendMessageAsync(deviceMessage);
        }
    }
}
