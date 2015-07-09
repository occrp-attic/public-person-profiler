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
        public string Description { set; get; }
        public string Image { set; get; }
        public long TypeId { set; get; }
        [ForeignKey("TypeId")]
        public virtual EntityType Type { set; get; }

        private List<RelationEntity> _relationEntities;
        [InverseProperty("Entity")]
        public virtual List<RelationEntity> RelationEntities
        {
            set { _relationEntities = value; }
            get { if (_relationEntities != null)return _relationEntities; else { _relationEntities = new List<RelationEntity>(); return _relationEntities; } }
        }

        private List<EntityReference> _entityReferences;
        [InverseProperty("Entity")]
        public virtual List<EntityReference> References
        {
            set { _entityReferences = value; }
            get { if (_entityReferences != null)return _entityReferences; else { _entityReferences = new List<EntityReference>(); return _entityReferences; } }
        }
    }
}
