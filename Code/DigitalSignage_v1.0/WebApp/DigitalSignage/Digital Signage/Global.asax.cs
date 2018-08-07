using Digital_Signage.Controllers;
using DigitalSignage.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace Digital_Signage
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            HomeController.dataImageValidator = new DataValidator();
            HomeController.sceneRepository = new SceneRepository();
            UserController.userRepository = new UserRepository();
            PlayerController.playerRepository = new PlayerRepository();
            DisplayStationController.DisplayStationRepository = new DisplayStationRepository();
            SceneController.sceneRepository = new SceneRepository();
            CampaignController.CampaignRepository = new CampaignRepository();
            CampaignHistoryController.CampaignRepository = new CampaignRepository();
            CampaignHistoryController.CampaignHistoryRepository = new CampaignHistoryRepository();
            CampaignController.playerRepository = new PlayerRepository();
            AdminController.AdminRepository = new AdminRepository();
            PlayerController.dataValidator = new DataValidator();
            DisplayStationController.dataStationValidator = new DataValidator();
            CampaignController.dataCampaignValidator = new DataValidator();
            CampaignController.campaignHistoryRepository = new CampaignHistoryRepository();
            SceneController.dataSceneValidator = new DataValidator();
            FaceRecCatalogController.faceRecRepository = new FaceRecRepository();
            
        }
    }
}