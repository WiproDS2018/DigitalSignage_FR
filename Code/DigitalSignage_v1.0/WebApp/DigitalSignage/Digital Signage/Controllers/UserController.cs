using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DigitalSignage.Domain;
using Newtonsoft.Json;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Web.SessionState;

namespace Digital_Signage.Controllers
{
    public class UserController : Controller
    {

        public static IUserRepository userRepository;

        public UserController() { }

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllUsers()
        {
            string message = "";
            List<UserViewModel> userList = new List<UserViewModel>();
            //Here we will save data to the database.
            if (ModelState.IsValid)
            {
                userList = userRepository.GetUsers();
                message = "sucess";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = userList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult ValidateUser(FormCollection userData)
        {
            var loggedUser = userData["lusername"].ToString();
            var loggedPassword = userData["linputPassword"].ToString();

            try
            {

                var loginuser = userRepository.GetUsers().Where(u => u.UserName.ToUpper().Equals(loggedUser.ToUpper())).FirstOrDefault();

                if (loginuser != null && loginuser.Password == loggedPassword)
                {
                    Session["UserName"] = loginuser.UserName;
                    Session["UserId"] = loginuser.UserId;
                    Session["UserRole"] = loginuser.Role;
                    Session["LoggedUser"] = loginuser;
                    Session["SignInType"] = "NoMicrosoft";


                    return RedirectToAction("HomePage", "Home");
                }
                TempData["ErrorMessage"] = SignageConstants.INVALIDLOGIN;
            }
            catch (Exception ex)
            {
                LogHelper.WriteDebugLog(ex.ToString());
                TempData["ErrorMessage"] = SignageConstants.INVALIDLOGIN;
            }
            return RedirectToAction("Login", "Home");

            //return RedirectToAction("Campaign", "Home");
        }

        [HttpPost]
        public ActionResult SaveUserOld(FormCollection newUser)//string userData)
        {
            var password = newUser["newPassword"].ToString();
            var confrimpassword = newUser["confirmPassword"].ToString();
            string userName = newUser["username"].ToString().Trim();
            string message = string.Empty;
            int result = 0;
            try {
                if (password != confrimpassword)
                {
                    message = SignageConstants.PASSWORDMISSMATCH;
                }
                else if (userName.Length == 0 || string.IsNullOrEmpty(userName))
                {
                    message = "User name cannot be empty";
                }
                else
                {
                    UserViewModel userInfo = new UserViewModel();
                    userInfo.UserName = newUser["username"].ToString();
                    userInfo.Password = password;
                    userInfo.Email = newUser["email"].ToString();
                    userInfo.Role = "User"; // Default role
                    userInfo.IsActive = true;
                    userInfo.AccountID = 108;
                    var loginuser = userRepository.GetUsers().Where(u => u.UserName.Equals(userInfo.UserName)).FirstOrDefault();
                    if (loginuser == null)
                    {
                        result = userRepository.SaveUser(userInfo);
                        message = SignageConstants.SUCCESSUSER;
                        TempData["ShowLoginPage"] = true;
                    }
                    else
                    {
                        message = SignageConstants.USEREXISTS;
                        TempData["ShowLoginPage"] = false;
                    }
                }

                TempData["UserMessage"] = message;

            }
            catch (Exception ex)
            {
                TempData["UserMessage"] = SignageConstants.ERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }

            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult SaveUser(string newUser)//string userData)
        {
            UserViewModel viewUser = JsonConvert.DeserializeObject<UserViewModel>(newUser);
            var password = viewUser.Password;
            var confrimpassword = "";
            string userName = viewUser.UserName;
            string message = string.Empty;
            int result = 0;
            try
            {
                //if (password != confrimpassword)
                //{
                //    message = SignageConstants.PASSWORDMISSMATCH;
                //}
                if (userName.Length == 0 || string.IsNullOrEmpty(userName))
                {
                    message = "User name cannot be empty";
                }
                else
                {
                    UserViewModel userInfo = new UserViewModel();
                    userInfo.UserName = viewUser.UserName;
                    userInfo.Password = password;
                    userInfo.Email = viewUser.Email;
                    userInfo.Role = "User"; // Default role
                    userInfo.IsActive = true;
                    userInfo.AccountID = 108;
                    var loginuser = userRepository.GetUsers().Where(u => u.UserName.Equals(userInfo.UserName)).FirstOrDefault();
                    if (loginuser == null)
                    {
                        result = userRepository.SaveUser(userInfo);
                        message = SignageConstants.SUCCESSUSER;
                        //TempData["ShowLoginPage"] = true;
                    }
                    else
                    {
                        message = SignageConstants.USEREXISTS;
                        //TempData["ShowLoginPage"] = false;
                    }
                }

                // TempData["UserMessage"] = message;

            }
            catch (Exception ex)
            {
                //TempData["UserMessage"] = SignageConstants.ERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }

            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult EditUser(string userData)
        {
            UserViewModel user_list = JsonConvert.DeserializeObject<UserViewModel>(userData);

            string message = "";
            int result = 0;

            //Here we will save data to the database.
            if (ModelState.IsValid)
            {
                result = userRepository.EditUser(user_list);
                message = "sucess";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult ResetUserPassword(string resetData)
        {
            UserViewModel user_list = JsonConvert.DeserializeObject<UserViewModel>(resetData);

            string message = "";
            int result = 0;

            //Here we will save data to the database.
            if (ModelState.IsValid)
            {
                result = userRepository.ResetUserPassword(user_list);
                if (result == -1) message = "Username and Email does not exists";
                else if (result == 0) message = "Error";
                else
                    message = "Success";

            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]
        public JsonResult changePassword(string oldPass, string newPass)
        {
            //UserViewModel users = JsonConvert.DeserializeObject<UserViewModel>(password);
            int user = 0;
            if (Session["UserId"] == null)
            {
                string msg = "Please Login.";
                return new JsonResult { Data=msg};
            }
            else
            {
                user = int.Parse(Session["UserId"].ToString()); 
            }

            string message = "";
            int result = 0;
          
             result = userRepository.EditUserPassword(oldPass,newPass,user);
            if (result == -1) message = "Old Password not matching";
            else if (result == -2) message = "Old and New Password should not be same";
            else if (result == 0) message = "Error";
            else
                message = "Success";
           
            
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]
        public JsonResult UpdateUserRole(int userId,int roleId)
        {
           

            string message = "";
            int result = 0;

            //Here we will save data to the database.
            if (ModelState.IsValid)
            {
                result = userRepository.UpdateUserRoles(userId, roleId);
                message = "Success";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetAllUserWithRoles()
        {

            string message = "";
            List<UserRoleViewModel> userRoleList = new List<UserRoleViewModel>();
           
           
            if (ModelState.IsValid)
            {
                userRoleList = userRepository.GetUserRoles();
                message = "sucess";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = userRoleList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public void MSSignin()//string userData)
        {
            try
            { 
           
                // Send an OpenID Connect sign-in request.
                if (!Request.IsAuthenticated)
                {
                    HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
                }
            

          }
            catch (Exception ex)
            {
                TempData["UserMessage"] = SignageConstants.ERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }

        
        }
    }
}
