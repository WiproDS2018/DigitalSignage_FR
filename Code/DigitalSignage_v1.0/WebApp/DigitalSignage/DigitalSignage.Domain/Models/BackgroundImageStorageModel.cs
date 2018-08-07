using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DigitalSignage.Domain
{
    public class BackgroundImageStorageModel
    {
        public int ImageId { get; set; }
        public string ImageName { get; set; }
        public string Opacity { get; set; }
        public bool ImageStatus { get; set; }
        public string ImageUrl { get; set; }
        public string DisplayContent { get; set; }
        public HttpPostedFileBase Attachment { get; set; }
    }
}
