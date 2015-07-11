using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.Relation
{
    public class RelationCardEntity
    {
        public long Id { set; get; }
        public string Name { set; get; }
        public string Type { set; get; }
        public string EntityType { set; get; }
    }
}
