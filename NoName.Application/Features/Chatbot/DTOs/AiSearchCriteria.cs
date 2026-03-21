using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Chatbot.DTOs
{
    public class AiSearchCriteria
    {
        public string LanguageId { get; set; } = "vi-VN";
        public string? Category { get; set; }
        public List<string> Colors { get; set; } = new(); // Khởi tạo sẵn list rỗng
        public List<string> Tags { get; set; } = new();
        public List<string> Materials { get; set; } = new();
        public decimal? MaxPrice { get; set; }
    }
}
