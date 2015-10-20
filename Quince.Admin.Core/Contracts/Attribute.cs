using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Contracts
{
    public class Attribute
    {
        public long Id { set; get; }
        [NotMapped]
        public long Type { set; get; }
        public string Name { set; get; }
        public string Value { set; get; }
    }
}
