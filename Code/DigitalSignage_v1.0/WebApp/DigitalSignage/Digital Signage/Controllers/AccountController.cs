using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using DigitalSignage.Domain;

namespace Digital_Signage.Controllers
{
    public class AccountController : Controller
    {
        public void SignIn()
        {
            

            // Send an OpenID Connect sign-in request.
            if (!Request.IsAuthenticated)
            {

                Session["SignInType"] = "ADLogin";
                Session["UserRole"] = "User";
                Session["UserName"] = User.Identity.Name;
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/Home/HomePage" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
             
                //Session["UserName"] = User.Identity.Name;
            }
            else
            {
                Session["SignInType"] = "";
                Response.Redirect("/Home/HomePage");
            }
        }
        public void SignOut()
        {
            
            Session["UserName"] = "";
            Session["UserId"] = "";
            var type = Session["SignInType"];
            if (Session["SignInType"] !=null && Session["SignInType"].ToString() == "ADLogin")
            {
                    Session.Abandon();

                    // Send an OpenID Connect sign-out request.
                    HttpContext.GetOwinContext().Authentication.SignOut(
                        OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
                    //Response.Redirect("/");
            }
            else 
            {
                Session.Abandon();
                Response.Redirect("/");
            }

        }
        public void EndSession()
        {
            // If AAD sends a single sign-out message to the app, end the user's session, but don't redirect to AAD for sign out.
            HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
        }

    }
}
