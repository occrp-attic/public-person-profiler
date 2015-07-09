using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Contracts
{
    public class Relation
    {
        public long Id { set; get; }
        public long TypeId { set; get; }
        [ForeignKey("TypeId")]
        public virtual RelationType Type {set;get;}

        private List<RelationEntity> _relationEntities;
        [InverseProperty("Relation")]
        public virtual List<RelationEntity> RelationEntities
        {
            set { _relationEntities = value; }
            get { if (_relationEntities != null)return _relationEntities; else { _relationEntities = new List<RelationEntity>(); return _relationEntities; } }
        }

        private List<RelationAttribute> _relationAttributes;
        [InverseProperty("Relation")]
        public virtual List<RelationAttribute> Attributes
        {
            set { _relationAttributes = value; }
            get { if (_relationAttributes != null)return _relationAttributes; else { _relationAttributes = new List<RelationAttribute>(); return _relationAttributes; } }
        }
    }
}
