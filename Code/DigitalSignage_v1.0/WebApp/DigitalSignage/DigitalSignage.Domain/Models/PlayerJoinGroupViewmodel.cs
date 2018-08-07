using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
   public class PlayerJoinGroupViewmodel
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string PlayerserialNo { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
