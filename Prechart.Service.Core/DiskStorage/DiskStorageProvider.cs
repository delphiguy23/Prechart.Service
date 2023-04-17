using System.IO;
using System.Threading.Tasks;

namespace Prechart.Service.Core.DiskStorage;

public class DiskStorageProvider : IDiskStorageProvider
    {
        public bool FileExists(string path) => File.Exists(path);
        public Stream GetFile(string path) => File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        public Stream CreateFile(string path) => File.Create(path);
        public void DeleteFile(string path) => File.Delete(path);
        public bool PathExists(string path) => Directory.Exists(path);
        public string[] GetFiles(string path) => Directory.GetFiles(path);
        public void DeleteFolder(string path, bool isrecursive) => Directory.Delete(path, isrecursive);
        public void CreateFolder(string path) => Directory.CreateDirectory(path);
        public async Task<byte[]> GetFileBytes(string path) => await File.ReadAllBytesAsync(path);
        public async Task<string> GetFileText(string path) => await File.ReadAllTextAsync(path);
        public void MoveFile(string pathFrom, string pathTo) => File.Move(pathFrom, pathTo);

        public async Task SetFileBytes(string path, byte[] data) => await File.WriteAllBytesAsync(path, data);
    }
