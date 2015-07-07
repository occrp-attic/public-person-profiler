using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Contracts
{
    public class RelationEntity
    {
        public long Id { set; get; }
        public long RelationId { set; get; }
        public long EntityId { set; get; }
        
        [ForeignKey("EntityId")]
        public virtual Entity Entity { set; get; }

        [ForeignKey("RelationId")]
        public virtual Relation Relation { set; get; }
    }
}
