using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.Specefication.ProductSpecs
{
    public class ProductSpecefication
    {

        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Sort { get; set; }
        public int PageIndex { get; set; } = 1;

        private const int MAXPAGESIZE = 50;
        private int _pagesize  = 6;

        public int PageSize
        {
            get=>_pagesize;
            set=>_pagesize = (value > MAXPAGESIZE) ? int.MaxValue : value;
        }
        private string? _search;
        public string? Search
        {
            get => _search;
            set => _search = value?.Trim().ToLower();
        }

    }
}
