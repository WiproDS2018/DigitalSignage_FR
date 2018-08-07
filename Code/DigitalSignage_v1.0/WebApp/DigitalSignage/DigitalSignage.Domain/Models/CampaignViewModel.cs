using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
    public class CampaignViewModel
    {
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
        public bool IsActive { get; set; }       
        public string Status { get; set; }
        public string Frequency { get; set; }
        public bool IsPublished { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string StartDateAndTime { get; set; }
        public string EndDateAndTime { get; set; }
        public int Interval { get; set; }
        public string OffsetTime { get; set; }
        public string Zone { get; set; }
        public string MultiSceneIds { get; set; }
        public string Copy { get; set; }
        public int AccountID { get; set; }
        public int CreatedBy { get; set; }
        public string Type { get; set; }
        public string DaysOfWeek { get; set; }



    }
}
