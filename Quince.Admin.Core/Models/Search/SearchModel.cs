using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.Search
{
   public class SearchModel
    {
       public string Query { set; get; }
       public int? Page { set; get; }
       public int TotalResults { set; get; }
    }
}
