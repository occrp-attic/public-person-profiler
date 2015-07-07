using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Contracts
{
    public class RelationType
    {
        public long Id { set;get;}
        public long Code { set; get; }
        public string Name { set; get; }
        private List<Relation> _relations;
        [InverseProperty("Type")]
        public virtual List<Relation> Relations {
            set { _relations = value; }
            get { if (_relations != null)return _relations; else { _relations = new List<Relation>(); return _relations; } }
        }
    }
}
