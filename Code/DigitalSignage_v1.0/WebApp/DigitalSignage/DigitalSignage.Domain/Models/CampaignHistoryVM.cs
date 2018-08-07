using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
    public class CampaignHistoryVM
    {
        public int PlayerId { get; set; }
        public string PlayerSerialNo { get; set; }
        public string PlayerName { get; set; }
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public int DisplayId { get; set; }
        public string DisplayName { get; set; }
        public int SceneId { get; set; }
        public string SceneName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string StartTimeVal { get; set; }
        public string EndTimeVal { get; set; }
        public string Status { get; set; }
        public int Interval { get; set; }
    }
}
