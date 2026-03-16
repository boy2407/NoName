using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Abstractions.Services
{
    public interface ILanguageService
    {
        Task<string> GetCurrentLanguage();
    }
}
