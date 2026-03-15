using MediatR;
using Microsoft.AspNetCore.Http;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Common;
using NoName.Application.Features.Products.DTOs.Guest;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Commands.Update.common
{
    public class UpdateProduct : IRequest<ApiResult<bool>>
    {
        public int Id { get; set; }
      
        public List<ProductTranslationViewModel> Translations { get; set; } = new();
        public List<int> CategoryIds { get; set; } = new();
        public bool IsActive { get; set; }
    }

}
