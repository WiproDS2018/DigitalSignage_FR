using DigitalSignage.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Digital_Signage.Controllers
{
    public class PlayerController : Controller
    {
        public static IPlayerRepository playerRepository;
        public static IDataValidator dataValidator;
        public PlayerController() { }
       
        // GET: /Player/
        #region Action Methods

        public ActionResult Index()
        {
            return View();
        }
        // Save Simple Player Form
        [HttpPost]
        public JsonResult SavePlayer(string vmplayer)
        {
                  
            PlayerViewModel viewPlayer = JsonConvert.DeserializeObject<PlayerViewModel>(vmplayer);

            string message= "";
            int result = 0;
            
           
            if (Session["LoggedUser"] != null)
            {

                UserViewModel loggedUser = new UserViewModel();

                loggedUser = (UserViewModel)Session["LoggedUser"];

                viewPlayer.AccountID = loggedUser.AccountID;
                viewPlayer.CreatedBy = loggedUser.UserId;             


            }
            else
            {
                message = SignageConstants.SESSIONERROR;
               
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
           

            try
            {
                
                message = dataValidator.ValidatePlayerSerialNo(viewPlayer);

                 //Here we will save data to the database.
                if (message == SignageConstants.SUCCESS)
                {
                    result = playerRepository.SavePlayer(viewPlayer);
                    if (result == 0) { message = SignageConstants.SAVEERROR; }
                }
            }
            catch(Exception ex)
            {
                message = SignageConstants.SAVEERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        // Save Simple Player Form
        [HttpGet]
        public JsonResult GetAllPlayers()
        {
            string message = "";
            List<PlayerViewModel> playerList = new List<PlayerViewModel>();
            //Here we will save data to the database.
            try
            {
                
                    playerList = playerRepository.GetPlayers();
            }
            catch (Exception ex)
            {
               // message = SignageConstants.SAVEERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }

          
            return new JsonResult { Data = playerList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult GetPlayer(int playerId)
        {
            string message = "";
            PlayerViewModel playerView = new PlayerViewModel();
            //Here we will save data to the database.
          try
            {
               
                    playerView = playerRepository.GetPlayer(playerId);
              
            }
            catch (Exception ex)
            {
                 message = SignageConstants.ERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }
            return new JsonResult { Data = playerView, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult EditPlayer(PlayerViewModel vmPlayer)
        {
           // PlayerViewModel player = JsonConvert.DeserializeObject<PlayerViewModel>(vmPlayer);


            string message = "";
            int result = 0;
            

            if (Session["LoggedUser"] != null)
            {

                UserViewModel loggedUser = new UserViewModel();

                loggedUser = (UserViewModel)Session["LoggedUser"];

                vmPlayer.AccountID = loggedUser.AccountID;
                vmPlayer.UpdatedBy = loggedUser.UserId;


            }
            else
            {
                message = SignageConstants.SESSIONERROR;
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            try
            {
                message = dataValidator.ValidatePlayerSerialNo(vmPlayer);
                if (message == SignageConstants.SUCCESS)
                {
                    result = playerRepository.EditPlayer(vmPlayer);
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
        public JsonResult DeletePlayer(string playerId)
        {
            string message = "";
            int result = 0;
            //string players = JsonConvert.DeserializeObject<string>(playerId);

            //int Id = 1002;
            //Here we will save data to the database
            List<int> playerList = new List<int>();
            try
            {
                playerList = GetPlayerList(playerId);
                message = dataValidator.ValidateDeviceDeletion(playerList);
                if (message == SignageConstants.SUCCESS)
                {
                    foreach (int Id in playerList)
                    {
                        result = playerRepository.Delete(Id);
                        if (result == 0) { message = SignageConstants.ERROR; }
                    }
                }

            }
            catch (Exception ex)
            {
                message = SignageConstants.SAVEERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }

            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        #region Player Group Methods
        [HttpPost]
        public ActionResult EditGroup(PlayerGroupViewModel playerGroup)
        {
            int success = 0;
            
           // PlayerGroupViewModel playerGroupViewModel = new PlayerGroupViewModel();
            string message = "";
            int result = 0;


            if (Session["LoggedUser"] != null)
            {

                UserViewModel loggedUser = new UserViewModel();

                loggedUser = (UserViewModel)Session["LoggedUser"];

                playerGroup.AccountID = loggedUser.AccountID;
                playerGroup.UpdatedBy = loggedUser.UserId;


            }
            else
            {
                message = SignageConstants.SESSIONERROR;
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            try
            {                
                  message = dataValidator.ValidatePlayerGroupName(playerGroup);

                    if (message == SignageConstants.SUCCESS)
                    {
                        success = playerRepository.EditGroup(playerGroup);
                        if (success == 0)
                        {
                            message = SignageConstants.SAVEERROR;
                        }
                    }
                
            }
            catch (Exception ex)
            {
                message = SignageConstants.SAVEERROR;
                LogHelper.WriteDebugLog(ex.ToString());
            }
           

            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpGet]
        public ActionResult getAllGroupPlayer()
        {
            List<PlayerGroupViewModel> playerviewmodelList = new List<PlayerGroupViewModel>();
            if (ModelState.IsValid)
            {
                playerviewmodelList = playerRepository.GetGroups();
            }

            return new JsonResult { Data = playerviewmodelList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult DeletePlayerGroup(string GroupIds)
        {
            int sucess = 0;
            string message = "";
            List<int> playerGroupList = new List<int>();
            try
                {

                    playerGroupList = GetPlayerList(GroupIds);
                    foreach (int Id in playerGroupList)
                    {
                        sucess = playerRepository.DeleteGroup(Id);
                        message = SignageConstants.SUCCESS;
                    }
                
                    if (sucess == 0)
                    {
                        message = SignageConstants.ERROR;
                    }
                    else
                    {
                        message = SignageConstants.SUCCESS;
                    }
                
                }
                catch(Exception ex)
                {
                  message = SignageConstants.ERROR;
                    LogHelper.WriteDebugLog(ex.ToString());
                }
            

            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetPlayerJoinGroup()
        {
            int sucess = 0;
            List<PlayerJoinGroupViewmodel> vmpjg = new List<PlayerJoinGroupViewmodel>();
            try
            {
                vmpjg = playerRepository.GetPlayerJoinGroup();
            }
            catch (Exception ex)
            {
                LogHelper.WriteDebugLog(ex.ToString());
            }
        
            return new JsonResult { Data = vmpjg, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult SavePlayerGroup(PlayerGroupViewModel vmplayerGroup)
        {
            string message = "";
            // PlayerGroupViewModel playergroup = JsonConvert.DeserializeObject<PlayerGroupViewModel>(vmplayerGroup);
            if (ModelState.IsValid)
            {
                int id = 0;

                if (Session["LoggedUser"] != null)
                {

                    UserViewModel loggedUser = new UserViewModel();

                    loggedUser = (UserViewModel)Session["LoggedUser"];

                    vmplayerGroup.AccountID = loggedUser.AccountID;
                    vmplayerGroup.CreatedBy = loggedUser.UserId;


                }
                else
                {
                    message = SignageConstants.SESSIONERROR;
                    return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }


                try
                {
                    message = dataValidator.ValidatePlayerGroupName(vmplayerGroup);

                    if (message == SignageConstants.SUCCESS)
                    {
                        id = playerRepository.SaveGroup(vmplayerGroup);
                        if (id == 0) { message = SignageConstants.SAVEERROR; }
                        
                    }
                }
                catch (Exception ex)
                {
                    message = SignageConstants.SAVEERROR;
                    LogHelper.WriteDebugLog(ex.ToString());
                }
               

            }
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        #endregion

        private List<int> GetPlayerList(string playerIds)
        {
            List<int> playerList = new List<int>();
            try
            {
                if (playerIds.Length > 0)
                {
                    string players = playerIds.Substring(1, playerIds.Length - 2);
                    string[] player = players.Split(',');

                    foreach (string id in player)
                    {
                        playerList.Add(Convert.ToInt32(id));
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteDebugLog(ex.ToString());
            }
             return playerList;
        }

        [HttpGet]
        public ActionResult GetUnGrouppedPlayers()
        {

            List<PlayerViewModel> UnGrouppedPlayerList = new List<PlayerViewModel>();

            UnGrouppedPlayerList = playerRepository.GetUngroupedPlayers();

            return new JsonResult { Data = UnGrouppedPlayerList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpGet]
        public ActionResult GetGrouppedPlayers(int groupid)
        {
            List<PlayerViewModel> GrouppedPlayerList = new List<PlayerViewModel>();
            GrouppedPlayerList = playerRepository.GetGroupedPlayers(groupid);

            return new JsonResult { Data = GrouppedPlayerList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public ActionResult SavePlayerToGroup(string playerIds, int stationId)
        {
            string message = "";
            int result = 0;
            string players = JsonConvert.DeserializeObject<string>(playerIds);
            List<int> playerList = new List<int>();

            playerList = GetFormatedPlayerList(players);

            try
            {
                int sucess = playerRepository.SavePlayersToGroup(playerList, stationId);

                message = "sucess";

            }
            catch (Exception ex)
            {

                LogHelper.WriteDebugLog(ex.ToString());
            }
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        private List<int> GetFormatedPlayerList(string players)
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

    }
}
