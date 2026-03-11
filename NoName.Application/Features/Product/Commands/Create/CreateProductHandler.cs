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
        private readonly IProductAppService _productAppService; 
        private readonly IMapper _mapper;

        public CreateProductHandler(
             IProductRepository productRepository,IMediaService mediaService,
             IProductAppService productAppService,IMapper mapper)
        {
            _productRepository = productRepository;
            _mediaService = mediaService;
            _productAppService = productAppService;
            _mapper = mapper;
        }
        public async Task<int> Handle(CreateProduct request, CancellationToken ct)
        {
            //auto mapping
            var product = _mapper.Map< NoName.Domain.Entities.Product>(request);

            //handel upload images
            await _productAppService.HandleUploadImagesAsync(
                product,
                request.ThumbnailImage,
                request.GalleryImages
            );

            //Save Database
            await _productRepository.AddAsync(product, ct);
            await _productRepository.SaveChangesAsync(ct);

            return product.Id;
        }


    }
}
