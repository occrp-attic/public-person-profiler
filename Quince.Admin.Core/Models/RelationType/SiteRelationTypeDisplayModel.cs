using Quince.Admin.Core.Models.Relation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.RelationType
{
    public class SiteRelationTypeDisplayModel
    {
        public SiteRelationTypeDisplayModel() {
            Relations = new List<SiteRelationDisplayModel>();
        }
        public long Id { set; get; }
        public string Name { set; get; }
        public int TotalRelations { set; get; }
        public List<SiteRelationDisplayModel> Relations { set; get; }
    }
}
