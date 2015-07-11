using Quince.Admin.Core.Models.EntityType;
using Quince.Admin.Core.Models.RelationType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.Pages
{
    public class HomePageModel
    {
        public IEnumerable<HomePageRelationTypeDisplayModel> RelationTypes { set; get; }
        public IEnumerable<HomePageEntityTypeDisplayModel> EntityTypes { set; get; }
    }
}
