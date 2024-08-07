using System.IO;
using System.Threading.Tasks;

namespace Tick.Core.Storage
{
    public interface IStorageService
    {
        Task<byte[]> DownloadMediaAsync(string fileName, string folderName);

        string GetMediaUrl(string fileName, string folderName);

        Task SaveMediaAsync(Stream mediaBinaryStream, string fileName, string folderName, string mimeType = null);

        Task DeleteMediaAsync(string fileName, string folderName);
    }
}
