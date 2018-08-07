using DigitalSignage.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Digital_Signage.Controllers
{
    public class FaceRecCatalogController : Controller
    {
        public static IFaceRecRepository faceRecRepository;       

        [HttpPost]
        public JsonResult UploadCatalog(FaceRecCatalog catalogTemplate)
        {
            //if (Request.Files.Count > 0)
            //{
            //    var image = Request.Files[0];
            //    //do something...
            //}

       
            string message = "";
            string imageUrl = "";
            int result = 0,ageUpper=0,ageLower=0;
            int userid = 0;
          
            if (Session["UserId"] != null)
            {
                userid = Convert.ToInt32(Session["UserId"]);
            }
            FaceRecCatalog catalogVm = new FaceRecCatalog();
            catalogVm.Title = catalogTemplate.Title;
            catalogVm.AgeRange = catalogTemplate.AgeRange;
            catalogVm.Attachment = catalogTemplate.Attachment;
            catalogVm.SceneType = SignageConstants.IMAGEUPLOAD;
            catalogVm.IsActive = true;
            string[] ageArray = catalogVm.AgeRange.Split(' ');
            ageLower = Convert.ToInt32(ageArray[0]);
            ageUpper = Convert.ToInt32(ageArray[3]);
            catalogVm.AgeUpper=ageUpper;
            catalogVm.AgeLower = ageLower;
            catalogVm.Gender = catalogTemplate.Gender;

            //Here we will save data to the database.
            try
            {
                    //Saving to Blob and getting URL
                    if (catalogVm.SceneType == SignageConstants.IMAGEUPLOAD || catalogVm.SceneType == SignageConstants.VIDEO)
                    {
                        imageUrl = faceRecRepository.UploadCatalog(catalogVm.Attachment, catalogVm.Title);
                        catalogVm.UploadUrl = imageUrl;
                    }
                    result = faceRecRepository.SaveCatalogScene(catalogVm);
                    message = SignageConstants.SUCCESS;
                    if (result == 0) { message = SignageConstants.SAVEERROR; }            


            }
            catch (Exception ex)
            {
                message = SignageConstants.SAVEERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }

            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult SaveCatalog(string catalogView)
        {

            FaceRecCatalog catalogVm = JsonConvert.DeserializeObject<FaceRecCatalog>(catalogView);

            string message = "";
            string htmlFileUrl = "";
            int result, userid = 0;
           
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllCatalog()
        {
            string message = "";
            List<FaceRecCatalog> catalogList = new List<FaceRecCatalog>();
           
            try
            {               
                    catalogList = faceRecRepository.GetAllCatalog();
                    message = SignageConstants.SUCCESS;
               
            }
            catch (Exception ex)
            {
                message = SignageConstants.ERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }

            return new JsonResult { Data = catalogList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

       


    }
}
