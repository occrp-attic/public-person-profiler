using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Contracts
{
    public class RelationAttribute:Attribute
    {
        public long RelationId { set; get; }
        [ForeignKey("RelationId")]
        public Relation Relation { set; get; }
    }
}
