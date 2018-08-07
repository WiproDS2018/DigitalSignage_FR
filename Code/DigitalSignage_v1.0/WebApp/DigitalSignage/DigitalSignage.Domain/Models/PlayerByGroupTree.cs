using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
    public class PlayerByGroupTree
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool Indicator { get; set; }
        public List<PlayerTree> Players{ get; set; }
    }
}
