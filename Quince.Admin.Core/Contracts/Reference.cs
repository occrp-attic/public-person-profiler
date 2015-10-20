using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Contracts
{
    public class Reference
    {
        public long Id { set; get; }
        [NotMapped]
        public long Type { set; get; }

        public string Title { set; get; }
        public string Document { set;get;}
        public string Link { set; get; }
        public string Description { set; get; }
    }
}
