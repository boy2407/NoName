using Microsoft.AspNetCore.Http;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string DefaultLang = "vi-VN";
        
        public LanguageService(IHttpContextAccessor httpContextAccessor, ILanguageRepository languageRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _languageRepository = languageRepository;
  
        }

        public async Task<string> GetCurrentLanguage()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return DefaultLang;

            var supportedLanguages = await _languageRepository.GetAllLanguageIdsAsync();

            var userLangs = request.GetTypedHeaders().AcceptLanguage;

            if (userLangs != null && userLangs.Any())
            {
                foreach (var lang in userLangs.OrderByDescending(x => x.Quality ?? 1))
                {
                    var langCode = lang.Value.ToString();

                    var matched = supportedLanguages.FirstOrDefault(s =>
                        s.StartsWith(langCode, StringComparison.OrdinalIgnoreCase));

                    if (matched != null) return matched;
                }
            }

            return DefaultLang;
        }


    }
}
