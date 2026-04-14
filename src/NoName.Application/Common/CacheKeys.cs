using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Common
{
    public static class CacheKeys
    {

        private const string Product = "product";

        public static string ProductDetail(int id,string lang ,string role = "Public")
            => $"{Product}:{id}:{lang}:{role}";

        public static string ProductList(string role, int page, int size)
            => $"{Product}:list:{role}:{page}:{size}";

        public static string ProductPrefix(int id) => $"{Product}:{id}:";
        public static string ProductAllListPrefix() => $"{Product}:list:";
    }
}
