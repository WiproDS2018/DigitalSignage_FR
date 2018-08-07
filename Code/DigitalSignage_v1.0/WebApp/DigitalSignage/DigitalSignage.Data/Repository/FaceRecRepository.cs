using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignage.Data.EF;
using DigitalSignage.Domain;
using System.Web;

namespace DigitalSignage.Data
{
    public class FaceRecRepository:IFaceRecRepository
    {
         SignageDBContext dbContext;
        string sceneUrl = "";

        public FaceRecRepository()
        {
            dbContext = new SignageDBContext();
        }

        public int SaveCatalogScene(FaceRecCatalog faceSceneVm)
        {
            int t = 0;
            try
            {
                var faceRecCatalogmodel = ToCatalogModel(faceSceneVm, false);

                dbContext.FaceRecogSignages.Add(faceRecCatalogmodel);
                dbContext.SaveChanges();
                //Newly created player id
                t = 1;
                //t = dbContext.FaceRecogSignages.FirstOrDefault(p => p.SceneName.Equals(scene.SceneName)).SceneId;
            }
            catch (Exception ex)
            {
                t = 0;
                throw ex;
            }
            return t;
        }

        public string UploadCatalog(HttpPostedFileBase attachment, string sceneName)
        {
            string imageBloburl = "";
            FileUploadService fileService = new FileUploadService();
            try
            {
                imageBloburl = fileService.UploadImage(attachment, sceneName, SignageConstants.IMAGEUPLOAD);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            //Task<string> objsceneUrl = fileService.UploadImageAsync(uploadScene.Attachment);

            return imageBloburl;
        }

        public List<FaceRecCatalog> GetAllCatalog()
        {
            var faceRecList = (from x in dbContext.FaceRecogSignages.AsNoTracking()
                               where x.IsActive == true
                             orderby x.AgeLowerLimit descending 
                             select x).ToList();
            return ToViewModelListCatalog(faceRecList);
            
        }

        public void Dispose()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        private FaceRecogSignage ToCatalogModel(FaceRecCatalog faceRecVm, bool modified)
        {
            FaceRecogSignage faceRec = new FaceRecogSignage();

            if (modified)
            {
                faceRec = dbContext.FaceRecogSignages.Where(c => c.Id == faceRecVm.Id).FirstOrDefault();
            }
            faceRec.AgeLowerLimit = faceRecVm.AgeLower;
            faceRec.AgeUpperLimit = faceRecVm.AgeUpper;
            faceRec.Gender = faceRecVm.Gender;
            faceRec.Signage = faceRecVm.UploadUrl.Trim();
            faceRec.Duration = 3000;
            faceRec.SceneType = faceRecVm.SceneType;
            faceRec.Title = faceRecVm.Title;
            faceRec.IsActive = true;
            return faceRec;
        }
        private List<FaceRecCatalog> ToViewModelListCatalog(List<FaceRecogSignage> catalogList)
        {

            List<FaceRecCatalog> catalogViewList = new List<FaceRecCatalog>();

            foreach (FaceRecogSignage catalog  in catalogList)
            {
                FaceRecCatalog vmCatalog = new FaceRecCatalog();

                vmCatalog.Id = catalog.Id;
                vmCatalog.Title = catalog.Title;
                vmCatalog.Gender = catalog.Gender;
                vmCatalog.AgeRange = catalog.AgeLowerLimit.ToString() + "-" + catalog.AgeUpperLimit.ToString() + " Yrs";
                vmCatalog.UploadUrl = catalog.Signage;
                vmCatalog.SceneType = catalog.SceneType;
                vmCatalog.Duration = catalog.Duration.Value;
                catalogViewList.Add(vmCatalog);
            }

            return catalogViewList;
        }
    }
}
