using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
   public class DashBoardDisplayModel
    {
        public int Yvalue { get; set; }
        public string Label { get; set; }
        public int[] Deviceid { get; set; }
       
        public List<Devices> DeviceList { get; set; }
    }
}
