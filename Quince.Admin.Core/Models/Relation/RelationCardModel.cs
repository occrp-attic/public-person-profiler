using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.Relation
{
    public class RelationCardModel
    {
        public long Id { set; get; }
        public IEnumerable<RelationCardEntity> Entities { set; get; }
        public IEnumerable<AttributeDisplayModel> Attributes { set; get; }
    }
}
