using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Contracts
{
    public class UserLogin
    {
        public DateTime Date { set;get; }
        public long UserId { set; get; }
        public string Ip { set; get; }
    }
}
