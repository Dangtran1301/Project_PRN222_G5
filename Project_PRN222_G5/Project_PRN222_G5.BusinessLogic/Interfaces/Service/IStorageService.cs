namespace Project_PRN222_G5.BusinessLogic.Interfaces.Service;

public interface IStorageService
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string folder);

    Task DeleteFileAsync(string relativePath);

    Task<Stream> GetFileAsync(string relativePath);
}