using System;

namespace Azure.IoT.SendMessage
{
    public class DeviceMessage
    {
        public string DeviceId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
