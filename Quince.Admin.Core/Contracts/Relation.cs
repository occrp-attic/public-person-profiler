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
    }
}
