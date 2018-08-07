using DigitalSignage.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Digital_Signage.Controllers
{
    public class SceneController : Controller
    {
        //
        // GET: /Scene/
        public static ISceneRepository sceneRepository;
        public static IDataValidator dataSceneValidator;

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult UploadScene(FileUploadViewModel sceneTemplate)
        {
            //if (Request.Files.Count > 0)
            //{
            //    var image = Request.Files[0];
            //    //do something...
            //}

            string message = "";
            string imageUrl = "";
            int result = 0;
            int userid = 0;
            SceneViewModel sceneVm = new SceneViewModel();
           
            if (Session["LoggedUser"] != null)
            {
                
                UserViewModel loggedUser = new UserViewModel();

                loggedUser = (UserViewModel)Session["LoggedUser"];

                userid = loggedUser.UserId;

                sceneVm.AccountID = loggedUser.AccountID;
                sceneVm.CreatedBy = loggedUser.UserId;

            }
            else
            {
                message = SignageConstants.SESSIONERROR;
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            sceneVm.SceneName = sceneTemplate.Title;
            sceneVm.Comments = sceneTemplate.Description;
            sceneVm.IconPosition = sceneTemplate.IconPosition;
            if (!(String.IsNullOrEmpty(sceneVm.IconPosition)))
            {
                sceneVm.TemplateType = "Time and Weather Template";
            }

            sceneVm.SceneContent = "";
            sceneVm.IsActive = true;
            sceneVm.IsPrimaryApproved = false;
            sceneVm.Status = "Saved";
            sceneVm.SceneContent = "";
            sceneVm.UpdatedBy = userid;
            sceneTemplate.SceneType = GetSceneType(sceneTemplate.SceneType.Trim());
            sceneVm.SceneType = sceneTemplate.SceneType;
            
            
        
            //Here we will save data to the database.
            try
            {

                message = dataSceneValidator.ValidateSceneName(sceneVm);
                if (message == SignageConstants.SUCCESS)
                {
                    //Saving to Blob and getting URL
                    if (sceneVm.SceneType == SignageConstants.IMAGEUPLOAD || sceneVm.SceneType == SignageConstants.VIDEO  ||sceneVm.SceneType == SignageConstants.PPT )
                    { 
                        imageUrl = sceneRepository.UploadFile(sceneTemplate, sceneVm.SceneName);
                        sceneVm.SceneUrl = imageUrl;
                    }
                    else
                    {
                        sceneVm.SceneUrl = sceneTemplate.UploadUrl;
                    }
                    
                    //message = await sceneRepository.UploadScene(sceneVm);
                    result = sceneRepository.SaveScene(sceneVm);
                    if (result == 0) { message = SignageConstants.SAVEERROR; }
                }


            }
            catch (Exception ex)
            {
                message = SignageConstants.SAVEERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }

            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult SaveScene(string sceneTemplate)
        {
            SceneViewModel sceneVm = JsonConvert.DeserializeObject<SceneViewModel>(sceneTemplate);


            string message = "";
            string htmlFileUrl = "";
            int result, userid = 0;
            if (Session["LoggedUser"] != null)
            {
        
                UserViewModel loggedUser = new UserViewModel();

                loggedUser = (UserViewModel)Session["LoggedUser"];

                userid = loggedUser.UserId;
                sceneVm.AccountID = loggedUser.AccountID;
                sceneVm.CreatedBy = loggedUser.UserId;
            
            }
            else
            {
                message = SignageConstants.SESSIONERROR;
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            sceneVm.LocalFilePath = Server.MapPath("~");
            sceneVm.UpdatedBy = userid;
            //Added for Accomodating Time Eather from Upload
            sceneVm.IsActive = true;
            if (String.IsNullOrEmpty(sceneVm.IconPosition))
            {
                sceneVm.TemplateType = "";
            }
            
            
            sceneVm.SceneType = SignageConstants.IMAGETEMPLATE;
            //sceneVm.TemplateType = 

            //Here we will save data to the database.
            try
            {                
                    message = dataSceneValidator.ValidateSceneName(sceneVm);
                    if (message == SignageConstants.SUCCESS)
                    {
                        //Saving to Blob and getting URL 
                        htmlFileUrl = sceneRepository.UploadHTMLFile(sceneVm);
                        sceneVm.SceneUrl = htmlFileUrl;
                        sceneVm.SceneContent = "";
                        result = sceneRepository.SaveScene(sceneVm);
                        if (result == 0) { message = SignageConstants.SAVEERROR; }
                    }               
                
            }
            catch (Exception ex)
            {
                message = SignageConstants.SAVEERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult DeleteScene(string sceneids)
        {
            string message = "", status="";
            int result = 0;
            List<int> sceneList = new List<int>();
            //int playerId = 1002;

            sceneList = GetSceneList(sceneids);
            message = dataSceneValidator.ValidateSceneDeletion(sceneList);
            if (message == SignageConstants.SUCCESS)
            {
                foreach (int Id in sceneList)
                {
                    SceneViewModel vmScene = sceneRepository.GetScene(Id);
                    try
                    {
                        result = sceneRepository.Delete(Id);
                        if (result == 0)
                        {
                            message = SignageConstants.ERROR;
                        }
                        else
                        {
                            message = SignageConstants.SUCCESS;
                        }
                    }
                    catch (Exception ex)
                    {
                        message = SignageConstants.ERROR;
                        LogHelper.WriteDebugLog(ex.ToString());
                    }
                    try
                    {

                        status = sceneRepository.DeleteFromBlob(vmScene);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteDebugLog(ex.ToString());
                    }
                }
            }

            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult EditScene(string vmscene)
        {
            SceneViewModel scene = JsonConvert.DeserializeObject<SceneViewModel>(vmscene);

            string message = "";
            int result = 0;

            if (Session["LoggedUser"] != null)
            {

                UserViewModel loggedUser = new UserViewModel();

                loggedUser = (UserViewModel)Session["LoggedUser"];

                scene.AccountID = loggedUser.AccountID;
                scene.UpdatedBy = loggedUser.UserId;


            }
            else
            {
                message = SignageConstants.SESSIONERROR;
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }


            try
            {
                if (ModelState.IsValid)
                {
                    result = sceneRepository.EditScene(scene);
                    message = "sucess";
                }
                else
                {
                    message = "Failed";
                }
            }
            catch (Exception ex)
            {
                message = "Failed";
                LogHelper.WriteDebugLog(ex.ToString());
            }
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetScene(int sceneId)
        {
            string message = "";
            SceneViewModel sceneView = new SceneViewModel();
            //Here we will save data to the database.
            try
            {
                if (ModelState.IsValid)
                {
                    sceneView = sceneRepository.GetScene(sceneId);
                    message = "sucess";
                }
                else
                {
                    message = "Failed";
                }
            }
            catch (Exception ex)
            {
                message = SignageConstants.SAVEERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }
            return new JsonResult { Data = sceneView, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetAllScenes()
        {
            string message = "";
            List<SceneViewModel> sceneList = new List<SceneViewModel>();

            ////Test Strack
            //List<SceneViewModel> sceneList2 = new List<SceneViewModel>();
            //int userId = 1;
            //Here we will save data to the database.
            try
            {
                if (ModelState.IsValid)
                {                  
                    sceneList = sceneRepository.GetAllScenes();
                    message = "sucess";
                }
                else
                {
                    message = "Failed";
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteDebugLog(ex.ToString());
            }

            return new JsonResult { Data = sceneList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult TrackScenes()
        {
            string message = "";
            List<SceneViewModel> sceneList = new List<SceneViewModel>();
            int userId = 1;
            Console.Write("*******************************************************************");
            Console.Write(sceneList);
          
            //Here we will save data to the database.
            try
            {
                if (ModelState.IsValid)
                {                    
                    sceneList = sceneRepository.TrackScenes(userId);
                    message = "sucess";
                }
                else
                {
                    message = "Failed";
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteDebugLog(ex.ToString());
            }

            return new JsonResult { Data = sceneList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult SubmitScene(int sceneId, int userId)
        {
            string message = "";
            int result = 0;
            //int playerId = 1002;
            //Here we will save data to the database.
            try
            {
                if (ModelState.IsValid)
                {
                    result = sceneRepository.SubmitScene(sceneId, userId);
                    message = "Success";
                }
                else
                {
                    message = "Failed";
                }
            }
            catch (Exception ex)
            {
                message = SignageConstants.SAVEERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult UploadScene2(HttpPostedFileBase aFile)
        {
            string filename = "";
            if (aFile != null && aFile.ContentLength > 0)
            {
                filename = Path.GetFileName(aFile.FileName);
                string path = Path.Combine(Server.MapPath("~/Images/templateImages/"), filename);
                aFile.SaveAs(path);
            }
            if (Request.Files.Count > 0)
            {
            }
            string message = "/Images/templateImages/" + filename;
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult GetImages()
        {
            string directoryPath = @"\Images\templateImages";
            string[] fileEntries = Directory.GetFiles(Server.MapPath(directoryPath));
            List<string> imageFiles = new List<string>();
            try
            {
                foreach (string fileName in fileEntries)
                {
                    string path = System.IO.Path.GetFileName(fileName);
                    imageFiles.Add(path);
                    //imageContent = imageContent + "<div  style=\"float: left;\"><img src='Images\\" + System.IO.Path.GetFileName(fileName) + "'/></div>";
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteDebugLog(ex.ToString());
            }
            return new JsonResult { Data = imageFiles, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]
        public JsonResult UploadTemplate(HttpPostedFileBase aFile)
        {
            string message = "";
            if (aFile != null && aFile.ContentLength > 0)
            {
                var filename = Path.GetFileName(aFile.FileName).ToString();
                //var path = Path.Combine(Server.MapPath("~/Images/templateImages/"), filename);
                //aFile.SaveAs(path);
               // message = sceneRepository.UploadTemplateImage(aFile, filename);
            }
            if (Request.Files.Count > 0)
            {
            }
           
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        private List<int> GetSceneList(string sceneIds)
        {
            List<int> sceneList = new List<int>();
            try
            {
                if (sceneIds.Length > 0)
                {
                    string scenes = sceneIds.Substring(1, sceneIds.Length - 2);
                    string[] scene = scenes.Split(',');

                    foreach (string id in scene)
                    {
                        sceneList.Add(Convert.ToInt32(id));
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteDebugLog(ex.ToString());
            }
            return sceneList;
        }

        private string GetSceneType(string type)
        {
            string sceneType ="";

            switch (type)
            {
                case "1":
                    sceneType = SignageConstants.IMAGEUPLOAD;
                    break;
                case "2":
                    sceneType = SignageConstants.IMAGEUPLOAD;
                    break;
                case "3":
                    sceneType = SignageConstants.VIDEO;
                    break;
                case "4":
                    sceneType = SignageConstants.WEBURL;
                    break;
                case "5":
                    sceneType = SignageConstants.VIDEOURL;
                    break;
                default:
                    sceneType = "";
                    break;
            }

            return sceneType;

        }

    }
}
