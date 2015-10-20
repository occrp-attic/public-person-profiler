using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.Search
{
   public class SearchResultItemModel
    {
       public string Title { set; get; }
       public long EntityId { set; get; }
       public string Type { set; get; }
       public string Image { set; get; }
       public string Description { set; get; }
    }
}
