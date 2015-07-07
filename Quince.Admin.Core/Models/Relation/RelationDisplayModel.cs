using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.Relation
{
    public class RelationDisplayModel:RelationModel
    {
        public string Type { set; get; }
        public string[] Entities { set; get; }

    }
}
