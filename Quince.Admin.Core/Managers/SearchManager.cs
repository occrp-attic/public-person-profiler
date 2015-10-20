using Quince.Admin.Core.Contexes;
using Quince.Admin.Core.Models;
using Quince.Admin.Core.Models.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Managers
{
    public static class SearchManager
    {
        public static IEnumerable<SearchResultItemModel> Search(SearchModel search)
        {
            var page = (search.Page??1) - 1;
            var skip = page*20;
            var context = new AdminDbContext();
            var totalResult = context.Entities.Where(e => e.Name.Contains(search.Query ?? string.Empty));
            var totalResults = totalResult.Count();
            var result = totalResult.OrderBy(e=>e.Name).Skip(skip).Take(20).Select(e => new SearchResultItemModel { Title = e.Name, EntityId = e.Id, Type = e.Type.Name, Image = e.Image??e.Type.DefaultImage });
            return result;
        }
    }
}
