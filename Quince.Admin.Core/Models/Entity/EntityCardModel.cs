using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.Entity
{
    public class EntityCardModel:EntityModel
    {
        public string Image { set; get; }
        public string Type { set; get; }
        public long TypeId { set; get; }
    }
}
