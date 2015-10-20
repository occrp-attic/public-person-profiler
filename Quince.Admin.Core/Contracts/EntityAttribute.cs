using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Contracts
{
    public class EntityAttribute:Attribute
    {
        public long EntityId { set; get; }
        [ForeignKey("EntityId")]
        public Entity Entity { set; get; }
    }
}
