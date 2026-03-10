
using Microsoft.AspNetCore.Http;
using NoName.Application.Abstractions.Services;
using NoName.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoName.Application.Services
{
    public class ProductAppService : IProductAppService
    {
        private readonly IMediaService _mediaService;
        private const string FOLDER = "products";

        public ProductAppService(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        public async Task HandleUploadImagesAsync(Product product, IFormFile? thumb, List<IFormFile>? gallery)
        {
            if (thumb != null)
            {
                var path = await _mediaService.UploadFileAsync(thumb, FOLDER);
                product.AddImage(path, thumb.Length, true);
            }

            if (gallery?.Any() == true)
            {
                foreach (var file in gallery)
                {
                    var path = await _mediaService.UploadFileAsync(file, FOLDER);
                    product.AddImage(path, file.Length, false);
                }
            }
        }
    }
}