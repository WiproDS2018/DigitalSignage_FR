using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace DigitalSignage.Domain
{
    public class FaceRecCatalog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AgeLower { get; set; }
        public int AgeUpper { get; set; }
        public string Gender { get; set; }
        public string SceneType { get; set; }
        public HttpPostedFileBase Attachment { get; set; }
        public string UploadUrl { get; set; }
        public string AgeRange { get; set; }
        public int Duration { get; set; }
        public bool IsActive { get; set; }
    }
}
