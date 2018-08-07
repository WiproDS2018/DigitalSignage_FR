using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DigitalSignage.Domain
{
    public interface ISceneRepository: IDisposable
    {
        int SaveScene(SceneViewModel scene);
        
        List<SceneViewModel> GetAllScenes();
        SceneViewModel GetScene(int sceneId);
        int EditScene(SceneViewModel scene);
        int Delete(int sceneId);
        int SubmitScene(int sceneId, int approverId);
        string UploadFile(FileUploadViewModel uploadScene,string sceneName);
        int DeleteHomeImageRecord();
        string UploadHTMLFile(SceneViewModel scene);
        string DeleteFromBlob(SceneViewModel scene);
        BackgroundImageStorageModel GetBackImage();
        List<SceneViewModel> TrackScenes(int userId);
        string UploadTemplateImage(HttpPostedFileBase imageToUpload, string sceneName);
        List<string> GetTemplateImages();
        int SaveImage(BackgroundImageStorageModel image);
        string UploadImageFile(BackgroundImageStorageModel uploadImage, string imageName);
       
    }
}
