using DigitalSignage.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Digital_Signage.Controllers
{
    public class AdminController : Controller
    {
        public static IAdminRepository AdminRepository;

        public AdminController() { }

        [HttpGet]
        public JsonResult GetAllPendingApprovals()
        {
            string message = "";
            List<SavedSceneViewModel> pendingSceneList = new List<SavedSceneViewModel>();

            int userid = 0;           
            if ( Session["UserId"] != null)
            {
                userid = Convert.ToInt32(Session["UserId"]);                
            }
           
            pendingSceneList = AdminRepository.GetAllSavedScenes(userid);
                message = "sucess";
           
            return new JsonResult { Data = pendingSceneList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult Update(string vmApprove)
        {
            SavedSceneViewModel vmAppScene = JsonConvert.DeserializeObject<SavedSceneViewModel>(vmApprove);

            string message = "";
            int result = 0;

            //Here we will save data to the database.
            if (ModelState.IsValid)
            {
                result = AdminRepository.UpdateSceneStatus(vmAppScene);
                message = "Success";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        

         }

    }
}
