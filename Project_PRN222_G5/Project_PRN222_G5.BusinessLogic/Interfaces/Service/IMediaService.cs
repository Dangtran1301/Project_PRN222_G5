using Microsoft.AspNetCore.Http;

namespace Project_PRN222_G5.BusinessLogic.Interfaces.Service;

public interface IMediaService
{
    Task<string> UploadImageAsync(IFormFile file, string folder);
    Task DeleteImageAsync(string imagePath);
}