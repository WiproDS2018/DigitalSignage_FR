using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
    public class PlayerTree
    {
        public int PlayerId { get; set; }
        public string PlayerSerialNo { get; set; }
        public string PlayerName { get; set; }
        public int ParentId { get; set; }
    }
}
