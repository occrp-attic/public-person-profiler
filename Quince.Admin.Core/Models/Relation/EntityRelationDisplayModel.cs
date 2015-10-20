using Quince.Admin.Core.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.Relation
{
    public class SiteRelationDisplayModel
    {
        public long TypeId { set; get; }
        public string Type { set; get; }
        public IEnumerable<AttributeDisplayModel> Attributes { set; get; }
        public IEnumerable<EntityTableModel> Entities { set; get; }
    }
}
