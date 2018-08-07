using System.Web.Mvc;
using DigitalSignage.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using DigitalSignage.Data;


namespace Digital_Signage.Controllers
{
    public class CampaignHistoryController : Controller
    {
        //
        // GET: /CampaignHistory/

        public static ICampaignHistoryRepository CampaignHistoryRepository;
        public static ICampaignRepository CampaignRepository;
        public static IPlayerRepository PlayerRepository;
        public static IDisplayStationRepository DisplayStationRepository;

        [HttpPost]
        public JsonResult GetCampaignHistory(string criteria,int id, String startDate, String endDate,string offsetTime)
        {
            //string message = "";           
            //int result = 0;
            //int trackerStatus = 0;
                   
            List<CampaignHistoryVM> vmHistList = new List<CampaignHistoryVM>();
             DateTime strtDateTime = DateTime.Parse(startDate);
            DateTime endDateTime = DateTime.Parse(endDate);
           //vmHistList= CampaignHistoryRepository.GetAllCampaignHistory();
            vmHistList= CampaignHistoryRepository.GetCampaignHistory(criteria,id,strtDateTime,endDateTime);

            return new JsonResult { Data = vmHistList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult GetDashBoardDetails()
        {
            List<DashBoardDisplayModel> dbList = new List<DashBoardDisplayModel>();
            DashBoardRepository dbr = new DashBoardRepository();
            dbList= dbr.GetDashBoardCampaignDetails();
            return new JsonResult { Data = dbList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult GetDashBoardDetailsBarChart()
        {       
            List<DashBoardDisplayModel> dbList2 = new List<DashBoardDisplayModel>();
            DashBoardRepository dbr = new DashBoardRepository();
            dbList2 = dbr.GetDashBoardSceneDetails();
            return new JsonResult { Data = dbList2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult GetDashBoardDetailsDevice()
        {
            List<DashBoardDisplayModel> dbList3 = new List<DashBoardDisplayModel>();
            DashBoardRepository dbr = new DashBoardRepository();
            dbList3 = dbr.GetDashBoardDeviceDetails();
            return new JsonResult { Data = dbList3, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult GetDeviceLocationDash()
        {
            List<DashBoardDisplayModel> dbList4 = new List<DashBoardDisplayModel>();
            DashBoardRepository dbr = new DashBoardRepository();
            dbList4 = dbr.GetDeviceLocationDashboard();  
            return new JsonResult { Data = dbList4, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetAllStations()
        {
         
            List<StationViewModel> displayList = new List<StationViewModel>();
            

                 displayList = CampaignRepository.GetAllDiplayStations();
                  
            return new JsonResult { Data = displayList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetAllCampaigns()
        {
     
 
            List<CampaignViewModel> campaignList = new List<CampaignViewModel>();

            campaignList = CampaignHistoryRepository.GetAllActiveCampaigns();
          
            return new JsonResult { Data = campaignList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetAllPlayers()
        {
            List<PlayerViewModel> playerList = new List<PlayerViewModel>();

            PlayerRepository playrepo = new PlayerRepository();
          
            playerList = playrepo.GetPlayers();

            return new JsonResult { Data = playerList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}
