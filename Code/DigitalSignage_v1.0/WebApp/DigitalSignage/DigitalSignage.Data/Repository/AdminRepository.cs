using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignage.Data.EF;
using DigitalSignage.Domain;

namespace DigitalSignage.Data
{
    public class AdminRepository : IAdminRepository
    {
        SignageDBContext dbContext;

        public AdminRepository()
        {
            dbContext = new SignageDBContext();
        }

        public List<SavedSceneViewModel> GetAllSavedScenes(int approverId)
        {
            var sceneList = new List<Scene>();
           if ( GetRole(approverId)=="Admin")
            { 


              sceneList = (from x in dbContext.Scenes
                             where x.IsPrimaryApproved == false 
                             && x.Status == SignageConstants.SCENESUBMIT
                             && x.Approver != 0
                             orderby x.SceneName ascending
                             select x).ToList();
            }
           else
            {
                sceneList = (from x in dbContext.Scenes
                             where x.IsPrimaryApproved == false
                             && x.Status == SignageConstants.SCENESUBMIT
                             && x.Approver == approverId
                             orderby x.SceneName ascending
                             select x).ToList();

            }

            return ToViewModelList(sceneList, approverId);
        }

        public int UpdateSceneStatus(SavedSceneViewModel savedScene)
        {
            int success = 0;
            int sceneId = savedScene.SceneId;
            string status = SignageConstants.SCENEREJECT;
            bool approvalStatus = false;
            if (savedScene.Status == "APR") { approvalStatus = true; status = SignageConstants.SCENEAPPROVE; }
            Scene scene = new Scene();
            try
            {
                scene = dbContext.Scenes.Where(c => c.SceneId == sceneId).FirstOrDefault();

                scene.IsPrimaryApproved = approvalStatus;
                scene.UpdatedTime = DateTime.UtcNow;
                scene.UpdatedBy = savedScene.Approver;
                scene.Status = status;

                dbContext.Entry(scene).State = System.Data.EntityState.Modified;
                dbContext.SaveChanges();
                success = 1;
            }
            catch (Exception ex)
            {               
                throw ex;
            }
            return success;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private List<SavedSceneViewModel> ToViewModelList(List<Scene> sceneList,int approver)
        {
           // approver = 1;

            List<SavedSceneViewModel> sceneViewList = new List<SavedSceneViewModel>();

            var user = dbContext.Users.Where(c => c.UserId == approver).FirstOrDefault();

            foreach (Scene scene in sceneList)
            {

                SavedSceneViewModel vmScene = new SavedSceneViewModel();
               
                vmScene.SceneId = scene.SceneId;
                vmScene.Approver = scene.Approver.Value;
                vmScene.ApproverName = user.UserName;
                vmScene.Comments = scene.Comments;
                vmScene.IsActive = scene.IsActive.Value;
                vmScene.IsPrimaryApproved = scene.IsPrimaryApproved.Value;
                vmScene.SceneContent = scene.SceneContent.Trim();
                vmScene.SceneName = scene.SceneName;
                vmScene.SceneType = scene.SceneType;
                vmScene.SceneUrl = scene.SceneUrl;
                vmScene.Status = scene.Status;
                vmScene.UpdatedBy = scene.UpdatedBy.Value;
                vmScene.IconPosition = scene.IconPosition;
                vmScene.TemplateType = scene.TemplateType;

                sceneViewList.Add(vmScene);

            }

            return sceneViewList;
        }

        private string GetRole(int approverId)
        {
            string role= "";
            var user = dbContext.Users.Where(c => c.UserId == approverId).FirstOrDefault();
            if (user != null)
            { 
                role = user.Role;
            }
            return  role ;

        }
    }
}
