using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DigitalSignage.Domain
{
    public interface IFaceRecRepository : IDisposable
    {
        int SaveCatalogScene(FaceRecCatalog faceScene);
        string UploadCatalog(HttpPostedFileBase uploadScene, string sceneName);
        List<FaceRecCatalog> GetAllCatalog();
    }
}
