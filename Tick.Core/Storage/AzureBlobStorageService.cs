using Tick.Core.Exceptions;
using Tick.Domain.Settings;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;
using System.Net;

namespace Tick.Core.Storage
{
    public class AzureBlobStorageService : IStorageService
    {
        private BlobContainerClient _blobContainer;
        private readonly AzureBlobOptions _azureBlobOptions;
        private string _publicEndpoint;

        public AzureBlobStorageService(IOptions<AzureBlobOptions> azureBlobOptions)
        {
            _azureBlobOptions = azureBlobOptions.Value;
            _publicEndpoint = _azureBlobOptions.PublicEndpoint;

            var blobClient = new BlobServiceClient(_azureBlobOptions.StorageConnectionString);
            _blobContainer = blobClient.GetBlobContainerClient(_azureBlobOptions.ContainerName);

            if (string.IsNullOrWhiteSpace(_publicEndpoint))
            {
                _publicEndpoint = _blobContainer.Uri.AbsoluteUri;
            }

        }

        public async Task<byte[]> DownloadMediaAsync(string fileName, string folderName)
        {
            var blockBlob = _blobContainer.GetBlobClient($"{folderName}/{fileName}");
            var content = await blockBlob.DownloadContentAsync();

            if (content == null || content.Value?.Content == null)
            {
                throw new ApiException("No media found.", httpStatusCode: HttpStatusCode.NotFound);
            }

            return content.Value.Content.ToArray();
        }

        public async Task DeleteMediaAsync(string fileName, string folderName)
        {
            var blockBlob = _blobContainer.GetBlobClient($"{folderName}/{fileName}");
            await blockBlob.DeleteIfExistsAsync();
        }

        public string GetMediaUrl(string fileName, string folderName)
        {
            return $"{_publicEndpoint}/{folderName}/{fileName}";
        }

        public async Task SaveMediaAsync(Stream mediaBinaryStream, string fileName, string folderName, string mimeType = null)
        {
            await _blobContainer.CreateIfNotExistsAsync();
            //await _blobContainer.SetAccessPolicyAsync(accessType: PublicAccessType.None);

            var blockBlob = _blobContainer.GetBlobClient($"{folderName}/{fileName}");

            var blobHttpHeader = mimeType != null ? new BlobHttpHeaders { ContentType = mimeType } : null;

            if (await blockBlob.ExistsAsync())
            {
                if (blobHttpHeader != null)
                {
                    await blockBlob.SetHttpHeadersAsync(blobHttpHeader);
                }

                await blockBlob.UploadAsync(mediaBinaryStream, overwrite: true);
            }
            else
            {
                await blockBlob.UploadAsync(mediaBinaryStream, blobHttpHeader);
            }
        }
    }
}
