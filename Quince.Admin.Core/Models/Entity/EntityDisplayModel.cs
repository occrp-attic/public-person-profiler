using Quince.Admin.Core.Models.Relation;
using Quince.Admin.Core.Models.RelationType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.Entity
{
    public class EntityDisplayModel
    {
        public long Id { set; get; }
        public string Name { set; get; }
        public string Type { set; get; }
        public string Description { set; get; }
        public string Image { set; get; }

        public IEnumerable<AttributeDisplayModel> Attributes { set; get; }
        public IEnumerable<ReferenceDisplayModel> References { set; get; }

        public IEnumerable<SiteRelationTypeDisplayModel> RelationTypes { set; get; }
    }
}
