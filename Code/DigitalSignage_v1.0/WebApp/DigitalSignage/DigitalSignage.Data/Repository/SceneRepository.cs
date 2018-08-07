using System;
using System.Collections.Generic;
using System.Linq;
using DigitalSignage.Data.EF;
using DigitalSignage.Domain;
using System.Data;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;

namespace DigitalSignage.Data
{
    public class SceneRepository : ISceneRepository
    {

        SignageDBContext dbContext;
        string sceneUrl = "";

        public SceneRepository()
        {
            dbContext = new SignageDBContext();
        }


        #region Public Method

        public int Delete(int sceneid)
        {
            int success = 0;
            try
            {                
                var scene = dbContext.Scenes.Find(sceneid);
                dbContext.Scenes.Remove(scene);
                dbContext.SaveChanges();
                success = 1;
              
            }
            catch (Exception ex)
            {
                success = 0;
                throw ex;
            }
            return success;
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }


        public int EditScene(SceneViewModel scene)
        {
            int success = 0;
            var scenedata = ToSceneModel(scene, true);
            try
            {
                dbContext.Entry(scenedata).State = System.Data.EntityState.Modified;
                dbContext.SaveChanges();
                success = 1;
            }
            catch (Exception ex)
            {
                success = 0;
                throw ex;
            }
            return success;
        }


        public List<SceneViewModel> GetAllScenes()
        {
            string st = SignageConstants.SCENESUBMIT;
            var sceneList = (from x in dbContext.Scenes.AsNoTracking()
                             where x.Status == SignageConstants.SCENESAVE
                             orderby x.SceneName ascending
                             select x).ToList();
            return ToViewModelList(sceneList);
        }

        public List<SceneViewModel> TrackScenes(int userId)
        {           
            var sceneList = (from x in dbContext.Scenes.AsNoTracking()
                             where x.IsActive == true
                             orderby x.SceneName ascending
                             select x).ToList();
            return ToViewModelListTrackScene(sceneList);
        }

        
        public SceneViewModel GetScene(int sceneId)
        {
            var scene = dbContext.Scenes.Where(c => c.SceneId == sceneId).FirstOrDefault();
            return ToViewModel(scene);
        }


        public BackgroundImageStorageModel GetBackImage()
        {

            var image = (from img in dbContext.BackgroundImageStorages select img).SingleOrDefault();
            return ToImageViewModel(image);
        }

        public string UploadFile(FileUploadViewModel uploadScene, string sceneName)
        {
            string imageBloburl = "";
            FileUploadService fileService = new FileUploadService();
            try
            {               
                imageBloburl = fileService.UploadImage(uploadScene.Attachment, sceneName,uploadScene.SceneType);
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //Task<string> objsceneUrl = fileService.UploadImageAsync(uploadScene.Attachment);

            return imageBloburl;
        }

        public string UploadHTMLFile(SceneViewModel scene)
        {
            string htmlFileBloburl = "";
            FileUploadService fileService = new FileUploadService();

            //Task<string> objsceneUrl = fileService.UploadImageAsync(uploadScene.Attachment);
            try
            {
                htmlFileBloburl = fileService.UploadHTMLContent(scene);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return htmlFileBloburl;
        }

        public string DeleteFromBlob(SceneViewModel vmScene)
        {
            string result = "";         
            FileUploadService fileService = new FileUploadService();
            
            try
            {
                result = fileService.DeleteImageFromBlob(vmScene);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }

        public int SaveScene(SceneViewModel scene)
        {
            int t = 0;
            Scene svm = new Scene();
            try
            {


                var objscenemodel = ToSceneModel(scene, false);

                dbContext.Scenes.Add(objscenemodel);

                dbContext.SaveChanges();

                //Newly created player id
                t = dbContext.Scenes.FirstOrDefault(p => p.SceneName.Equals(scene.SceneName)).SceneId;
            }
            catch (Exception ex)
            {
                t = 0;
                throw ex;
            }
            return t;
        }

        public string UploadImageFile(BackgroundImageStorageModel uploadImage, string imageName)
        {
            string imageBloburl = "";
            FileUploadService fileService = new FileUploadService();
            try
            {
                imageBloburl = fileService.UploadImage(uploadImage.Attachment, imageName, uploadImage.ImageName);

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return imageBloburl;
        }


        public int SaveImage(BackgroundImageStorageModel image)
        {
            int t = 0;
            BackgroundImageStorage svm = new BackgroundImageStorage();
            try
            {


                var objimagemodel = ToImageModel(image);

                dbContext.BackgroundImageStorages.Add(objimagemodel);

                dbContext.SaveChanges();
                t = 1;
                //Newly created image id
                // t = dbContext.BackgroundImageStorages.FirstOrDefault(p => p.ImageName.Equals(image.ImageName)).ImageId;
            }
            catch (Exception ex)
            {
                t = 0;
                throw ex;
            }
            return t;
        }

        public int DeleteHomeImageRecord()
        {
            int success = 0;
            var delimg = (from del in dbContext.BackgroundImageStorages select del);

            foreach (var item in delimg)
            {
                dbContext.BackgroundImageStorages.Remove(item);
            }
            try
            {
                dbContext.SaveChanges();
                success = 1;
            }
            catch (Exception e)
            {
                success = 0;
                throw e;
            }
            return success;

        }


        public int SubmitScene(int sceneId, int approverId)
        {
            int success = 0;

            try
            {
                var scene = dbContext.Scenes.Where(c => c.SceneId == sceneId).FirstOrDefault();
                scene.Approver = approverId;
                scene.IsPrimaryApproved = false;
                scene.Status = "Submitted";
                dbContext.Entry(scene).State = System.Data.EntityState.Modified;
                dbContext.SaveChanges();
                success = 1;
            }
            catch (Exception ex)
            {
                success = 0;
                throw ex;
            }
            return success;
        }

    

        #endregion

        #region Private Methods
        private Scene ToSceneModel(SceneViewModel svm, bool modified)
        {
            Scene scene = new Scene();

            if (modified)
            {
                scene = dbContext.Scenes.Where(c => c.SceneId == svm.SceneId).FirstOrDefault();
            }
            scene.IsActive = svm.IsActive;
            scene.IsPrimaryApproved = svm.IsPrimaryApproved;
            scene.SceneContent = svm.SceneContent.Trim();
            scene.SceneId = svm.SceneId;
            scene.SceneName = svm.SceneName;
            scene.SceneType = svm.SceneType;
            scene.SceneUrl = svm.SceneUrl;
            scene.UpdatedBy = svm.UpdatedBy;
            scene.UpdatedTime = DateTime.UtcNow;
            scene.Approver = svm.Approver;
            scene.Status = SignageConstants.SCENESAVE;
            scene.Comments = svm.Comments;
            scene.AccountID = svm.AccountID;
            scene.CreatedBy = svm.CreatedBy;
            scene.TemplateType = svm.TemplateType;
            scene.IconPosition = svm.IconPosition;
            return scene;
        }


        private List<SceneViewModel> ToViewModelList(List<Scene> sceneList)
        {

            List<SceneViewModel> sceneViewList = new List<SceneViewModel>();
            List<Approver> ApproverList = new List<Approver>();
            ApproverList = GetApprovers();
            foreach (Scene scene in sceneList)
            {
                SceneViewModel vmScene = new SceneViewModel();

                // vmScene.Approver = (scene.Approver == null) ? scene.Approver : 0;
                vmScene.Approver = Convert.ToInt32(scene.Approver);
                vmScene.Comments = scene.Comments;
                vmScene.IsActive = Convert.ToBoolean(scene.IsActive);
                vmScene.IsPrimaryApproved = Convert.ToBoolean(scene.IsPrimaryApproved);
                vmScene.SceneContent = scene.SceneContent.Trim();
                vmScene.SceneId = scene.SceneId;
                vmScene.SceneName = scene.SceneName;
                vmScene.SceneType = scene.SceneType;
                vmScene.SceneUrl = scene.SceneUrl;
                vmScene.Status = scene.Status;
                vmScene.UpdatedBy = Convert.ToInt32(scene.UpdatedBy);
                vmScene.UpdatedTime = DateTime.Now;
                vmScene.ApproverList = ApproverList;
                vmScene.IconPosition = scene.IconPosition;
                vmScene.TemplateType = scene.TemplateType;
                //player.CreatedBy = vmPlayer.CreatedBy;
                //player.CreatedDate = DateTime.UtcNow;

                sceneViewList.Add(vmScene);

            }

            return sceneViewList;
        }

        private List<SceneViewModel> ToViewModelListTrackScene(List<Scene> sceneList)
        {

            List<SceneViewModel> sceneViewList = new List<SceneViewModel>();

            foreach (Scene scene in sceneList)
            {
                SceneViewModel vmScene = new SceneViewModel();

                // vmScene.Approver = (scene.Approver == null) ? scene.Approver : 0;
                vmScene.Approver = Convert.ToInt32(scene.Approver);
                vmScene.Comments = scene.Comments;
                vmScene.IsActive = Convert.ToBoolean(scene.IsActive);
                vmScene.IsPrimaryApproved = Convert.ToBoolean(scene.IsPrimaryApproved);
                vmScene.SceneContent = scene.SceneContent.Trim();
                vmScene.SceneId = scene.SceneId;
                vmScene.SceneName = scene.SceneName;
                vmScene.SceneType = scene.SceneType;
                vmScene.SceneUrl = scene.SceneUrl;
                vmScene.Status = GetStatusDetails(scene);
                vmScene.UpdatedBy = Convert.ToInt32(scene.UpdatedBy);
                vmScene.UpdatedTime = DateTime.Now;
                vmScene.IconPosition = scene.IconPosition;
                vmScene.TemplateType = scene.TemplateType;
                //player.CreatedBy = vmPlayer.CreatedBy;
                //player.CreatedDate = DateTime.UtcNow;

                sceneViewList.Add(vmScene);

            }

            return sceneViewList;
        }
        private SceneViewModel ToViewModel(Scene scene)
        {
            SceneViewModel vmScene = new SceneViewModel();
            vmScene.Approver = Convert.ToInt32(scene.Approver);
            vmScene.Comments = scene.Comments;
            vmScene.IsActive = Convert.ToBoolean(scene.IsActive);
            vmScene.IsPrimaryApproved = Convert.ToBoolean(scene.IsPrimaryApproved);
            vmScene.SceneContent = scene.SceneContent.Trim();
            vmScene.SceneId = scene.SceneId;
            vmScene.SceneName = scene.SceneName;
            vmScene.SceneType = scene.SceneType;
            vmScene.SceneUrl = scene.SceneUrl;
            vmScene.Status = scene.Status;
            vmScene.UpdatedBy = Convert.ToInt32(scene.UpdatedBy);
            vmScene.UpdatedTime = DateTime.Now;

            return vmScene;
        }

        private BackgroundImageStorageModel ToImageViewModel(BackgroundImageStorage image)
        {
            BackgroundImageStorageModel vmImg = new BackgroundImageStorageModel();
            vmImg.ImageName = image.ImageName;
            vmImg.ImageStatus = image.ImageStatus.GetValueOrDefault();
            vmImg.ImageUrl = image.ImageUrl;
            vmImg.Opacity = image.Opacity;
            vmImg.ImageId = image.ImageId;

            return vmImg;
        }

        private BackgroundImageStorage ToImageModel(BackgroundImageStorageModel img)
        {
            BackgroundImageStorage image = new BackgroundImageStorage();

            //if (modified)
            //{
            //    image = dbContext.BackgroundImageStorages.Where(im => im.ImageId == img.ImageId).FirstOrDefault();
            //}
            //image.ImageId = img.ImageId;
            image.ImageName = img.ImageName;
            image.ImageStatus = true;
            image.ImageUrl = img.ImageUrl;
            image.Opacity = img.Opacity;
            //  image.DisplayContent = img.DisplayContent;
            return image;
        }

        private List<Approver> GetApprovers()
        {
            List<Approver> ApproverList = new List<Approver>();

            var userList = dbContext.Users.AsNoTracking().Where(u => u.IsActive == true && u.Role != "User").ToList();

            foreach (var user in userList)
            {
                ApproverList.Add(new Approver()
                {
                    UserId = user.UserId,
                    UserName = user.UserName
                });
            }

            return ApproverList;
        }

        private string GetStatusDetails(Scene scene)
        {
            string sceneStatus = "";
            if (scene.Status == "Submitted")
            {
                sceneStatus = "Pending for approval";
            }
            else if (scene.Status == "Rejected")
            {
                sceneStatus = "Rejected by the approver";
            }
            else
                sceneStatus = scene.Status;

            return sceneStatus;
        }

        public string UploadTemplateImage(HttpPostedFileBase imageToUpload, string imageName)
        {
            string imageBloburl = "";
            FileUploadService fileService = new FileUploadService();
            try
            {
                imageBloburl = fileService.UploadTemplateImage(imageToUpload, imageName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //Task<string> objsceneUrl = fileService.UploadImageAsync(uploadScene.Attachment);

            return imageBloburl;
        }

        public List<string> GetTemplateImages()
        {
            var urls = new List<string>();
            string imageBloburl = "";
            FileUploadService fileService = new FileUploadService();
            try
            {
                urls = fileService.GetTemplateImages();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            //Task<string> objsceneUrl = fileService.UploadImageAsync(uploadScene.Attachment);

            return urls;
        }






        //private async Task Upload(HttpPostedFileBase photo)
        //{
        //    FileUploadService fileService = new FileUploadService();

        //    sceneUrl = await fileService.UploadImageAsync(photo);
        //}


    }
    #endregion

}
