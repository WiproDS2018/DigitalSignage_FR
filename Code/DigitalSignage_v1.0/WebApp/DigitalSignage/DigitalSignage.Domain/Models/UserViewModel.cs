using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DigitalSignage.Domain
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Role { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
    }
}