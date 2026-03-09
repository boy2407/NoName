
using Microsoft.AspNetCore.Http;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Abstractions.Services
{
     public interface IProductAppService
    {
        Task HandleUploadImagesAsync(Product product, IFormFile? thumb, List<IFormFile>? gallery);
    }
}
