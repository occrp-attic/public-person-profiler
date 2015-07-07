using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.DataTables
{
    public class DTColumn
    {
        public string data { set; get; }
        public string name { set; get; }
        public bool searchable { set; get; }
        public bool orderable { set; get; }
        public DTSearch search { set; get; }

    }
}
