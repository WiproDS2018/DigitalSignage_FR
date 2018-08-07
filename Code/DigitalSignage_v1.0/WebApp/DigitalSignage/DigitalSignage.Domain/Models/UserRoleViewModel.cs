using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitalSignage.Domain
{
    public class UserRoleViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
        public List<RoleView> RoleList { get; set; }
    }
}
