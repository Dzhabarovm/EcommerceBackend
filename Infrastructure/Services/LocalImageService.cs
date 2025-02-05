using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Services
{
    public class LocalImageService : IImageService
    {
        private readonly string _imageFolderPath;

        public LocalImageService(IHostEnvironment env)
        {
            _imageFolderPath = Path.Combine(env.ContentRootPath, "wwwroot/images");
            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_imageFolderPath, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/images/{fileName}"; // Относительный URL
        }
    }
}
