using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.DataTables
{
    public class DTResponse
    {
        public int draw { set; get; }
        public int recordsTotal { set; get; }
        public int recordsFiltered { set; get; }
        public string error { set; get; }
        public object data { set; get; }
    }
}
