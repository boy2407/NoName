using AutoMapper;
using MediatR;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Features.Categories.DTOs;
using NoName.Application.Features.Languages.DTOs;
using NoName.Application.Features.Languages.Queries.GetLanguage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Queries.GetCategory
{
    public class GetAllCategoriesHandle : IRequestHandler<GetAllCategories, List<CategoryViewModel>>
    {

        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public GetAllCategoriesHandle(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        private void BuildTree(CategoryViewModel parent, List<CategoryViewModel> allItems)
        {
            parent.ChildCategories = allItems.Where(x => x.ParentId == parent.Id).ToList();
            foreach (var child in parent.ChildCategories) { 
                
                BuildTree(child, allItems); 
            }

        }
        public async Task<List<CategoryViewModel>> Handle(GetAllCategories request, CancellationToken ct)
        {
            var categories = await _repository.GetAllAsync(request.LanguageId,ct);
            var allItems = _mapper.Map<List<CategoryViewModel>>(categories);

            var rootNodes = allItems.Where(x => x.ParentId == null).ToList();

            foreach (var root in rootNodes)
            {
                BuildTree(root, allItems);
            }

            return rootNodes;
        } 
   
    }
}
