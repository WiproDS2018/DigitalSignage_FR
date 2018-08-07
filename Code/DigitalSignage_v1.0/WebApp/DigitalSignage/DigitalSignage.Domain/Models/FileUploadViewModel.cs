using System.Web;

namespace DigitalSignage.Domain
{


    public class FileUploadViewModel
    {
        public int SceneId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SceneType { get; set; }
        public HttpPostedFileBase Attachment { get; set; }
        public string UploadUrl { get; set; }
        public string IconPosition { get; set; }
    }
}
