using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
    public class SceneViewModel
    {
        public int SceneId { get; set; }
        public string SceneName { get; set; }
        public string SceneUrl { get; set; }
        public string SceneType { get; set; }
        public string SceneContent { get; set; }
        public bool IsActive { get; set; }
        public bool IsPrimaryApproved { get; set; }
        public string Status { get; set; }
        public int Approver { get; set; }
        public List<Approver> ApproverList { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Comments { get; set; }
        public string LocalFilePath { get; set; }
        public string TemplateId { get; set; }
        public string imgString { get; set; }
        public int CreatedBy { get; set; }
        public int AccountID { get; set; }
        public string TemplateType { get; set; }
        public string IconPosition { get; set; }
    }
}
