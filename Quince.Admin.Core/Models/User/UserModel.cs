using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quince.Admin.Core.Models.User
{
    public class UserModel
    {
        public long Id { set; get; }
        public string Email { set; get; }
        public DateTime RegisterDate { set; get; }
        public bool Active { set; get; }
    }
}
