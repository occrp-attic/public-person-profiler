using Quince.Admin.Core.Models.Entity;
using Quince.Admin.Core.Models.Relation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.Pages
{
    public class EntityRelationsPageModel:EntityModel
    {
        public long CurrentRelationTypeId { set; get; }
        public string Type { set; get; }
        public string RelationType { set; get; }
        public int TotalPages { set; get; }
        public int CurrentPage { set; get; }
        public IEnumerable<RelationCardModel> Relations{set;get;}
    }
}
