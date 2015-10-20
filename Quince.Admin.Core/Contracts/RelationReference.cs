using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quince.Admin.Core.Contracts
{
    public class RelationReference:Reference
    {
        public long RelationId { set; get; }
        [ForeignKey("RelationId")]
        public Relation Relation { set; get; }
    }
}
