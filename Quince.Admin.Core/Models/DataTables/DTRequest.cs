using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.DataTables
{
    public class DTRequest
    {
        public int draw { set; get; }
        public int start { set;get; }
        public int length { set; get; }
        public DTSearch search { set; get; }
        public DTColumn[] columns { set; get; }
        public DTOrder[] order { set; get; }
    }
}
