using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NoName.Application.Abstractions.Persistence;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Command.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
   

        public UpdateCategoryCommandHandler(ICategoryRepository repo, IMapper mapper)
        {
            _categoryRepository = repo;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken ct)
        {

            var category = await _categoryRepository.GetByIdAsync(request.Id, ct);
            if (category == null) return false;


            _mapper.Map(request, category);

            foreach (var dto in request.Translations)
            { 
                var existing = category.CategoryTranslations.FirstOrDefault(x => x.LanguageId == dto.LanguageId);
                if (existing != null)
                {
                    _mapper.Map(dto, existing);
                }
                else // Insert new translates 
                {
                   
                    var newEntity = _mapper.Map<CategoryTranslation>(dto);
                    //Relationship 
                    newEntity.CategoryId = category.Id;
                    category.CategoryTranslations.Add(newEntity);
                }
            }
            // Handle translations
            var requestLangIds = request.Translations.Select(x => x.LanguageId).ToList();
            var toDelete = category.CategoryTranslations
                .Where(x => !requestLangIds.Contains(x.LanguageId)).ToList();

            foreach (var item in toDelete)
            {
                category.CategoryTranslations.Remove(item);
            }

            await _categoryRepository.SaveChangesAsync(ct);
            return true;
        }
    }
}
