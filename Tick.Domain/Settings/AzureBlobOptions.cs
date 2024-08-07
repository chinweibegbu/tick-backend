namespace Tick.Domain.Settings
{
    public class AzureBlobOptions
    {
        public string StorageConnectionString { get; init; }
        public string ContainerName { get; init; }
        public string PublicEndpoint { get; init; }
    }
}