using Microsoft.AspNetCore.Hosting;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service;

namespace Project_PRN222_G5.BusinessLogic.Services;

public class DiskStorageService(IWebHostEnvironment env) : IStorageService
{
    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string folder)
    {
        var folderPath = Path.Combine(env.WebRootPath, folder);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);
        await using var stream = new FileStream(filePath, FileMode.Create);
        await fileStream.CopyToAsync(stream);

        return Path.Combine(folder, fileName).Replace("\\", "/");
    }

    public Task DeleteFileAsync(string relativePath)
    {
        var fullPath = Path.Combine(env.WebRootPath, relativePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
        if (File.Exists(fullPath))
            File.Delete(fullPath);
        return Task.CompletedTask;
    }

    public Task<Stream> GetFileAsync(string relativePath)
    {
        var fullPath = Path.Combine(env.WebRootPath, relativePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        return Task.FromResult<Stream>(stream);
    }
}