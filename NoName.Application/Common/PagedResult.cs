using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Application.Common
{
    public class PagedResult<T>
    {

        public List<T> Items { get; set; } = new();
        public int TotalRecords { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
