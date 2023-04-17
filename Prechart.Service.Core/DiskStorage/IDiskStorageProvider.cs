using System.IO;
using System.Threading.Tasks;

namespace Prechart.Service.Core.DiskStorage;

public interface IDiskStorageProvider
{
    bool FileExists(string path);
    Stream GetFile(string path);
    Stream CreateFile(string path);
    void DeleteFile(string path);
    bool PathExists(string path);
    string[] GetFiles(string path);
    void DeleteFolder(string path, bool isrecursive);
    void CreateFolder(string path);
    Task<byte[]> GetFileBytes(string path);
    Task<string> GetFileText(string path);
    void MoveFile(string pathFrom, string pathTo);

    Task SetFileBytes(string path, byte[] data);
}