using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Contracts
{
    public class EntityType
    {
        public long Id { set; get; }
        public long Code { set; get; }
        public string Name { set; get; }
        public string DefaultImage { set; get; }

        private List<Entity> _entities;
        [InverseProperty("Type")]
        public virtual List<Entity> Entities
        {
            set { _entities = value; }
            get { if (_entities != null) { return _entities; } else { _entities = new List<Entity>(); return _entities; } }
        }
    }
}
