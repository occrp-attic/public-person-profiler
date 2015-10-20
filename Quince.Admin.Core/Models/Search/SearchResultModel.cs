using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.Search
{
    public class SearchResultModel
    {
        public int CurrentPage { set; get; }
        public int TotalPages { set; get; }
        public IEnumerable<SearchResultItemModel> Result { set; get; }
    }
}
