using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile file);
    }
}
