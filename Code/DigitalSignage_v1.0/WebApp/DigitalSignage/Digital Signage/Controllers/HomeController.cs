using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DigitalSignage.Domain;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;


namespace Digital_Signage.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/      
        public static ISceneRepository sceneRepository;
        public static IDataValidator dataImageValidator;

        public ActionResult Login()
        {

            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.LoginMessage = TempData["ErrorMessage"].ToString();
                TempData["ErrorMessage"] = null;
            }
            if (TempData["UserMessage"] != null)
            {
                ViewBag.LoginMessage = TempData["UserMessage"];
                TempData["UserMessage"] = null;
            }

            if (TempData["ShowLoginPage"] != null)
            {
                ViewBag.ShowLoginPage = Convert.ToBoolean(TempData["ShowLoginPage"]);
                TempData["ShowLoginPage"] = null;
            }
            else
            {
                ViewBag.ShowLoginPage = false;
            }

            return View();
        }


        public ActionResult Logout(PlayerViewModel vmplayer)
        {
            Session["UserName"] = "";
            Session["UserId"] = "";
            Session.Abandon();

            //return RedirectToAction("Login", "Home");

            // Send an OpenID Connect sign-out request.
            HttpContext.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
            return RedirectToAction("Login", "Home");

        }

        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult Player()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult Station()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult Scenes()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult Campaign()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult templateUpload()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult templatesView()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult Admin()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult User()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult Settings()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        // Save Simple Registration Form


        public ActionResult PlayerGroup()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult Catalogue()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult Report()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public ActionResult HomePage()
        {
            if (Session["SignInType"] != null && Session["SignInType"].ToString() == "ADLogin")
            {
                UserViewModel loginuser = new UserViewModel();

                loginuser.UserName ="AdUser"; 
                loginuser.UserId = 1;
                loginuser.Role = "User";
                Session["UserName"] = loginuser.UserName;
                Session["UserId"] = loginuser.UserId;
                Session["UserRole"] = loginuser.Role;
                Session["LoggedUser"] = loginuser;
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        public ActionResult createDeviceGroup()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public JsonResult DeleteHomeImage()
        {
            int result = 0;
            string message = "";
            try
            {
                result = sceneRepository.DeleteHomeImageRecord();
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
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult getBannerImageSer()
        {
            string message = "";
            BackgroundImageStorageModel bachkImage = new BackgroundImageStorageModel();
            //Here we will save data to the database.
            try
            {
                if (ModelState.IsValid)
                {
                    bachkImage = sceneRepository.GetBackImage();
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
            return new JsonResult { Data = bachkImage, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult SaveImage(BackgroundImageStorageModel image)
        {

            string message = "";
            string imageUrl = "";
            int result = 0;
            int userid = 0;
            BackgroundImageStorageModel backImageVm = new BackgroundImageStorageModel();

            if (Session["LoggedUser"] != null)
            {

                UserViewModel loggedUser = new UserViewModel();

                loggedUser = (UserViewModel)Session["LoggedUser"];

                userid = loggedUser.UserId;

            }
            else
            {
                message = SignageConstants.SESSIONERROR;
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            //backImageVm.ImageId = image.ImageId;
            backImageVm.ImageName = image.ImageName;
            backImageVm.ImageStatus = image.ImageStatus;
            backImageVm.Opacity = image.Opacity;
            // backImageVm.ImageUrl = image.ImageUrl;
            //backImageVm.DisplayContent = image.DisplayContent;


            //    //Here we will save data to the database.
            try
            {

                sceneRepository.DeleteHomeImageRecord();
                message = dataImageValidator.ValidateImageName(backImageVm);
                if (message == SignageConstants.SUCCESS)
                {
                    //Saving to Blob and getting URL

                    imageUrl = sceneRepository.UploadImageFile(image, backImageVm.ImageName);
                    backImageVm.ImageUrl = imageUrl;


                    result = sceneRepository.SaveImage(backImageVm);
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
        public ActionResult Dashboard()
        {

            if (Session["UserName"] == null)
            {

                return RedirectToAction("Login", "Home");
            }
            return View();
        }

    }
}
