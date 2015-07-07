using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.Entity
{
    public class AddEntityRelationModel:EntityModel
    {
        public long RelationTypeId { set; get; }
        public long OtherEntityId { set; get; }
    }
}
