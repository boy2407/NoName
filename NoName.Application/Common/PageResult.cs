using System;
using System.Collections.Generic;
using System.Text;

namespace NoName.Application.Common
{
    public class PageResult<T>
    {

        public List<T> Items { get; set; }
        public int TotalRecords { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    }
}
