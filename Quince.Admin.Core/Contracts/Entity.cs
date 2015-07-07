using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Contracts
{
    public class Entity
    {
        public long Id { set; get; }
        public string Name { set; get; }
        public long TypeId { set; get; }
        [ForeignKey("TypeId")]
        public virtual EntityType Type { set; get; }
    }
}
