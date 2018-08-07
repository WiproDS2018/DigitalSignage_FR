using System.Web.Mvc;
using DigitalSignage.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace Digital_Signage.Controllers
{
    public class DisplayStationController : Controller
    {
        public static IDisplayStationRepository DisplayStationRepository;
        public static IDataValidator dataStationValidator;
        public DisplayStationController() { }

        // Save Station Data

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddStation(string displayStation)
        {
            StationViewModel vmDisplayStation = JsonConvert.DeserializeObject<StationViewModel>(displayStation);

            string message = "";
            int result = 0;

            int userid = 0;
            if (Session["LoggedUser"] != null)
            {
             
                UserViewModel loggedUser = new UserViewModel();

                loggedUser = (UserViewModel)Session["LoggedUser"];

                userid = loggedUser.UserId;

                vmDisplayStation.AccountID = loggedUser.AccountID;
                vmDisplayStation.CreatedBy = loggedUser.UserId;

            }
            else
            {
                message = SignageConstants.SESSIONERROR;
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            

            //Here we will save data to the database.
            try
            {
                message = dataStationValidator.ValidateStationName(vmDisplayStation);
                if (message == SignageConstants.SUCCESS)
                {
                    result = DisplayStationRepository.AddDisplayStation(vmDisplayStation);
                    if (result == 0) { message = SignageConstants.SAVEERROR; }
                }
                else
                {
                    message = SignageConstants.STATIONEXIST;
                }
            }
            catch (Exception ex)
            {
                message = SignageConstants.SAVEERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }

            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        // GET All Stations

        [HttpGet]
        public JsonResult GetAllStations()
        {
            //////Test By Group...................../////
            //List<PlayerByGroupTree> playerByGroupList = new List<PlayerByGroupTree>();
            //playerByGroupList = DisplayStationRepository.GetAvailablePlayersByGroup();
            /// ************************************
            string message = "";
            List<StationViewModel> displayList = new List<StationViewModel>();
           
            if (ModelState.IsValid)
            {
                displayList = DisplayStationRepository.GetAllDiplayStations();
                message = "success";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = displayList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        //Update Station
        [HttpPost]
        public JsonResult UpdateStation(StationViewModel vmStation)
        {
            //StationViewModel vmStation = JsonConvert.DeserializeObject<StationViewModel>(vmDisplayStation);

            string message = "";
            int result = 0;

            int userid = 0;
            if (Session["LoggedUser"] != null)
            {

                UserViewModel loggedUser = new UserViewModel();

                loggedUser = (UserViewModel)Session["LoggedUser"];

                userid = loggedUser.UserId;

                vmStation.AccountID = loggedUser.AccountID;
                vmStation.UpdatedBy = loggedUser.UserId;

            }
            else
            {
                message = SignageConstants.SESSIONERROR;
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            //Here we will save data to the database.
            try
            {
                message = dataStationValidator.ValidateStationName(vmStation);
                if (message == SignageConstants.SUCCESS)
                {
                    result = DisplayStationRepository.EditDisplayStation(vmStation);
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


        // Delete Station
        [HttpPost]
        public JsonResult DeleteStation(string stationID)
        {
            string message = "";
            int result = 0;

            List<int> staionList = new List<int>();
            staionList = GetStationList(stationID);
            try
            {
                message = dataStationValidator.ValidateStationDeletion(staionList);

                if (message == SignageConstants.SUCCESS)
                {
                    foreach (int item in staionList)
                    {
                        result = DisplayStationRepository.DeleteDisplayStation(item);
                        if (result == 0) { message = SignageConstants.ERROR; }
                    }
                }
            }
            catch (Exception ex)
            {
                message = SignageConstants.ERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }

            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult AssignPlayer(string playerIds, int stationId)
        {
            string message = "";
            string players = JsonConvert.DeserializeObject<string>(playerIds);
           

            int result = 0;
            List<int> playerList = new List<int>();

            playerList = GetPlayerList(players);

            if (ModelState.IsValid)
            {
                result = DisplayStationRepository.AssignPlayers(playerList, stationId);
                message = "sucess";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpGet]
        public JsonResult GetAvailablePlayers()
        {
            string message = "";
            
            List<PlayerViewModel> playerList = new List<PlayerViewModel>();

            if (ModelState.IsValid)
            {
                playerList = DisplayStationRepository.GetAvailablePlayers();                
                message = "success";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = playerList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetAssignedPlayers(int stationId)
        {
            string message = "";

            List<PlayerViewModel> playerList = new List<PlayerViewModel>();

            if (ModelState.IsValid)
            {                
                playerList = DisplayStationRepository.GetAssignnedPlayers(stationId);
                message = "success";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = playerList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetAssignedPlayersByGroup(int stationId)
        {
            string message = "";

            List<PlayerByGroupTree> playerByGroupList = new List<PlayerByGroupTree>();

            if (ModelState.IsValid)
            {
                playerByGroupList = DisplayStationRepository.GetAssignedPlayersByGroup(stationId);
                message = "success";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = playerByGroupList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpGet]
        public JsonResult GetAvailablePlayersByGroup()
        {
            string message = "";
            List<PlayerByGroupTree> playerByGroupList = new List<PlayerByGroupTree>();          

            if (ModelState.IsValid)
            {
                playerByGroupList = DisplayStationRepository.GetAvailablePlayersByGroup();
                message = "success";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = playerByGroupList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        private List<int> GetPlayerList(string players)
        {
            List<int> playerList = new List<int>();

            players = players.Trim().TrimEnd(',');

            if (players.Length > 0)
            {
                string[] player = players.Split(',');               
               
                foreach (string id in player)
                {                   
                    playerList.Add(Convert.ToInt32(id));
                }
            }
            return playerList;
        }

        private List<int> GetStationList(string stationIds)
        {
            List<int> stationList = new List<int>();

            if (stationIds.Length > 0)
            {
                string stations = stationIds.Substring(1, stationIds.Length - 2);
                string[] stationArray = stations.Split(',');

                foreach (string id in stationArray)
                {
                    stationList.Add(Convert.ToInt32(id));
                }
            }
            return stationList;
        }

    }
}
