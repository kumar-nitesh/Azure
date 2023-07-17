namespace Azure.IoT.Edge.Deploy.Module
{
    public class IotEdgeModuleInfo
    {
        public string ContainerRegistryName { get; set; }
        public string ContainerRegistryAddress { get; set; }
        public string ContainerRegistryUsername { get; set; }
        public string ContainerRegistryPassword { get; set; }
        public string DeviceId { get; set; }
        public string ModuleName { get; set; }
        public string ImageUri { get; set; }
        public string Version { get; set; }
    }
}
