using Microsoft.AspNetCore.Http;
using Project_PRN222_G5.BusinessLogic.Extensions;
using Project_PRN222_G5.BusinessLogic.Interfaces.Service;

namespace Project_PRN222_G5.BusinessLogic.Services;

public class MediaService(IStorageService storageService) : IMediaService
{
    private const string RootFolder = "uploads";
    public async Task<string> UploadImageAsync(IFormFile file, string folder = RootFolder)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is invalid");

        var encryptedFileName = file.FileName.ToHashedFileName();

        var fullFolderPath = folder.StartsWith($"{RootFolder}/") ? folder : $"{RootFolder}/{folder.TrimStart('/')}";

        await using var stream = file.OpenReadStream();
        return await storageService.SaveFileAsync(stream, encryptedFileName, fullFolderPath);
    }

    public async Task DeleteImageAsync(string imagePath)
    {
        await storageService.DeleteFileAsync(imagePath);
    }
}