using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
   
    public class PlayerGroupViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public int CreatedBy { get; set; }
        public int AccountID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
