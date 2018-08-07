using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignageDps
{
    public class CloudMessage
    {
        public int sid { get; set; }
        public string ContentUrl { get; set;}
        public string Start { get; set; }
        public string End { get; set; }
        public string Duration { get; set; }
        public string ContentType { get; set; }
        public string show { get; set; }

        public string Frequency { get; set; }

        public string DaysOfWeek { get; set; }
        public string IconPosition { get; set; }

    }
}
