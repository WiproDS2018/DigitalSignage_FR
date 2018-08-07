using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
    public class StationViewModel
    {
        public int DisplayStationid { get; set; }
        public string DisplayStationName { get; set; }
        public string DisplayStationLocation { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int AccountID { get; set; }
        public int UpdatedBy { get; set; }
    }
}
