using Tick.Core.Exceptions;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace Tick.Core.Storage
{
    public class LocalStorageService : IStorageService
    {
        private const string MediaRootFolder = "content";
        private readonly IWebHostEnvironment _env;

        public LocalStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<byte[]> DownloadMediaAsync(string fileName, string folderName)
        {
            string filePath = Path.Combine(_env.WebRootPath, MediaRootFolder, folderName, fileName);

            if (!File.Exists(filePath))
            {
                throw new ApiException("No media found.");
            }

            return await File.ReadAllBytesAsync(filePath); ;
        }

        public string GetMediaUrl(string fileName, string folderName)
        {
            return $"/{MediaRootFolder}/{folderName}/{fileName}";
        }

        public async Task SaveMediaAsync(Stream mediaBinaryStream, string fileName, string folderName, string mimeType = null)
        {
            string filePath = Path.Combine(_env.WebRootPath, MediaRootFolder, folderName, fileName);
            using (var output = new FileStream(filePath, FileMode.Create))
            {
                await mediaBinaryStream.CopyToAsync(output);
            }
        }

        public async Task DeleteMediaAsync(string fileName, string folderName)
        {
            var filePath = Path.Combine(_env.WebRootPath, MediaRootFolder, folderName, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
