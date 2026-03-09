using NoName.Application.Abstractions.Persistence;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NoName.Application.Abstractions.Services;
namespace NoName.Application.Features.Product.Commands.Create
{
    public class CreateProductHandler : IRequestHandler<CreateProduct, int>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediaService _mediaService;
        private readonly IProductAppService _productDomainService; 
        private readonly IMapper _mapper; 
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public CreateProductHandler(IProductRepository productRepository, IMediaService mediaService)
        {
            _productRepository = productRepository;
            _mediaService = mediaService;
        }


        public async Task<int> Handle(CreateProduct request, CancellationToken ct)
        {
            // 1. Map tự động từ Command sang Entity
            var product = _mapper.Map<NoName.Domain.Entities.Product>(request);

            // 2. Xử lý ảnh (Vẫn dùng Domain Service vì nó chứa logic I/O)
            await _productDomainService.HandleUploadImagesAsync(
                product,
                request.ThumbnailImage,
                request.GalleryImages
            );

            // 3. Lưu Database
            await _productRepository.AddAsync(product, ct);
            await _productRepository.SaveChangesAsync(ct);

            return product.Id;
        }


    }
}
