using System.Web.Mvc;
using DigitalSignage.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;

namespace Digital_Signage.Controllers
{
    public class CampaignController : Controller
    {
        public static ICampaignRepository CampaignRepository;
        public static IDataValidator dataCampaignValidator;
        public static ICampaignHistoryRepository campaignHistoryRepository;
        public static IPlayerRepository playerRepository;

        public CampaignController() { }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveCampaign(string vmCampgn)
        {
            CampaignViewModel vmCampaign = JsonConvert.DeserializeObject<CampaignViewModel>(vmCampgn);

            string startTime = "";
            string endTime = "";
            string timeoffset = "0";
            timeoffset = vmCampaign.OffsetTime;
            if (timeoffset.Substring(0, 1) == "-")
            {
                timeoffset = timeoffset.Substring(1, timeoffset.Length - 1);
            }
            else
            {
                timeoffset = "-" + timeoffset;
            }
            double utcoffset = Convert.ToDouble(timeoffset);
            // Local .NET timeZone.
            DateTime strtDateTime = DateTime.Parse(vmCampaign.StartTimeVal);
            startTime = strtDateTime.ToUniversalTime().AddMinutes(utcoffset).ToString("HH:mm:ss tt");
            //startTime = strtDateTime.ToString("HH:mm:ss tt");
            startTime = startTime.Substring(0, 8);

            DateTime endDateTime = DateTime.Parse(vmCampaign.EndTimeVal);
            endTime = endDateTime.ToUniversalTime().AddMinutes(utcoffset).ToString("HH:mm:ss tt");
            //endTime = endDateTime.ToString("HH:mm:ss tt");
            endTime = endTime.Substring(0, 8);

            TimeSpan stime = TimeSpan.Parse(startTime);
            TimeSpan etime = TimeSpan.Parse(endTime);

            // Bug Fix for Daylight Saving
            if (vmCampaign.Copy == "N")
            {
                if (vmCampaign.Zone.Contains("Daylight"))
                {
                    if (stime.Hours == 0)
                    {
                        stime += TimeSpan.FromHours(24);
                    }
                    if (etime.Hours == 0)
                    {
                        etime += TimeSpan.FromHours(24);
                    }
                    stime += TimeSpan.FromHours(-1);
                    etime += TimeSpan.FromHours(-1);
                }
            }

            vmCampaign.StartTime = stime;
            vmCampaign.EndTime = etime;

            ////////
            //Session["UserName"] = vmCampaign.StartTimeVal;

            vmCampaign.StartDate = vmCampaign.StartDate.ToLocalTime();
            vmCampaign.EndDate = vmCampaign.EndDate.ToLocalTime();
            //vmCampaign.StartDate = vmCampaign.StartDate.ToUniversalTime().AddDays(1);
            //vmCampaign.EndDate = vmCampaign.EndDate.ToUniversalTime().AddDays(1);
            vmCampaign.StartDate = vmCampaign.StartDate.ToUniversalTime().AddMinutes(utcoffset);
            vmCampaign.EndDate = vmCampaign.EndDate.ToUniversalTime().AddMinutes(utcoffset);
            vmCampaign.Interval = vmCampaign.Interval * 1000;

            if (vmCampaign.Frequency != "Weekly")
            {
                vmCampaign.DaysOfWeek = "";
            }




            vmCampaign.SceneId = 0;// Scene Ids saving in SceneCampaign mapping table.

            string message = "";
            int result = 0;

            if (Session["LoggedUser"] != null)
            {

                UserViewModel loggedUser = new UserViewModel();

                loggedUser = (UserViewModel)Session["LoggedUser"];

                vmCampaign.AccountID = loggedUser.AccountID;
                vmCampaign.CreatedBy = loggedUser.UserId;


            }
            else
            {
                message = SignageConstants.SESSIONERROR;
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            //Here we will save data to the database.
            try
            {
                message = dataCampaignValidator.ValidateCampaignName(vmCampaign);
                if (message == SignageConstants.SUCCESS)
                {
                    result = CampaignRepository.AddCampaign(vmCampaign);

                    if (result == 0) { message = SignageConstants.SAVEERROR; }

                    campaignHistoryRepository.AddtoCampaignHistory(result, SignageConstants.CAMPAIGNSCHEDULE, 1);
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
        public JsonResult EditCampaign(string vmCampgn)
        {
            CampaignViewModel vmCampaign = JsonConvert.DeserializeObject<CampaignViewModel>(vmCampgn);

            string startTime = ""; //Pacific Daylight Time
            string endTime = "";
            string timeoffset = "0";
            bool isDaylightgSaving = false;

            timeoffset = vmCampaign.OffsetTime;
            if (timeoffset.Substring(0, 1) == "-")
            {
                timeoffset = timeoffset.Substring(1, timeoffset.Length - 1);
            }
            else
            {
                timeoffset = "-" + timeoffset;
            }

            double utcoffset = Convert.ToDouble(timeoffset);
            if (vmCampaign.Zone.Contains("Daylight"))
            {
                isDaylightgSaving = true;
            }
            // Local .NET timeZone.
            // DateTime strtDateTime = DateTime.Parse(vmCampaign.StartTimeVal);

            startTime = vmCampaign.StartDate.ToUniversalTime().AddMinutes(utcoffset).ToString("yyyy-MM-dd HH:mm:ss");
            //startTime = strtDateTime.ToString("HH:mm:ss tt");
            string[] startTimearray = startTime.Split(' ');
            startTime = startTimearray[1].Trim();

            //DateTime endDateTime = DateTime.Parse(vmCampaign.EndTimeVal);
            endTime = vmCampaign.EndDate.ToUniversalTime().AddMinutes(utcoffset).ToString("yyyy-MM-dd HH:mm:ss");
            string[] endTimearray = endTime.Split(' ');
            endTime = endTimearray[1].Trim();

            TimeSpan stime = TimeSpan.Parse(startTime);
            TimeSpan etime = TimeSpan.Parse(endTime);
            vmCampaign.StartTime = stime;
            vmCampaign.EndTime = etime;

            //if (vmCampaign.Zone.Contains("Daylight"))
            //{
            //    //stime += TimeSpan.FromHours(1);
            //    //etime += TimeSpan.FromHours(1);
            //}
            ////////
            //Session["UserName"] = vmCampaign.StartTimeVal;

            string[] startDatearray = startTimearray[0].Trim().Split('-');
            string[] endDatearray = endTimearray[0].Trim().Split('-');
            vmCampaign.StartDate = new DateTime(Convert.ToInt32(startDatearray[0]), Convert.ToInt32(startDatearray[1]), Convert.ToInt32(startDatearray[2]));
            vmCampaign.EndDate = new DateTime(Convert.ToInt32(endDatearray[0]), Convert.ToInt32(endDatearray[1]), Convert.ToInt32(endDatearray[2]));

            string message = "";
            int result = 0;

            if (Session["LoggedUser"] != null)
            {

                UserViewModel loggedUser = new UserViewModel();

                loggedUser = (UserViewModel)Session["LoggedUser"];

                vmCampaign.AccountID = loggedUser.AccountID;
                vmCampaign.UpdatedBy = loggedUser.UserId;


            }
            else
            {
                message = SignageConstants.SESSIONERROR;
                return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            //Here we will save data to the database.
            try
            {

                result = CampaignRepository.EditCampaign(vmCampaign);
                if (result == 0)
                {
                    message = SignageConstants.ERROR;
                }
                else
                {
                    message = SignageConstants.SUCCESS;
                    campaignHistoryRepository.AddtoCampaignHistory(vmCampaign.CampaignId, "Edited", 1);
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
        public JsonResult DeleteCampaign(int campaignId)
        {
            string message = "";
            int result = 0;
            try
            {
                result = CampaignRepository.DeleteCampaign(campaignId);
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


        // GET All Stations

        [HttpGet]
        public JsonResult GetCampaign(int campaignId)
        {
            CampaignViewModel vmCampaign = new CampaignViewModel();

            vmCampaign = CampaignRepository.GetCampaign(campaignId);

            return new JsonResult { Data = vmCampaign, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetAllCampaigns()
        {
            string message = "";

            List<CampaignViewModel> campaignList = new List<CampaignViewModel>();


            if (ModelState.IsValid)
            {
                campaignList = CampaignRepository.GetAllCampaigns();

                message = "success";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = campaignList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetAllExpiredCampaigns()
        {
            string message = "";

            List<CampaignViewModel> campaignList = new List<CampaignViewModel>();

            if (ModelState.IsValid)
            {
                campaignList = CampaignRepository.GetExpiredCampaigns();

                message = "success";

            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = campaignList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult ActivateCampaign(int campaignId)
        {
            string message = "";
            int result = 0;
            int trackerStatus = 0;
            //campaignId = 1002;

            if (ModelState.IsValid)
            {
                result = CampaignRepository.ActivateExpiredCampaign(campaignId);
                trackerStatus = CampaignRepository.UpdateTrackerTable(campaignId, SignageConstants.CAMPAIGNCANCEL);
                campaignHistoryRepository.AddtoCampaignHistory(campaignId, SignageConstants.CAMPAIGNACTIVATE, 1);
                message = SignageConstants.SUCCESS;
            }
            else
            {
                message = SignageConstants.ERROR;
            }



            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetCampaignsToPublish()
        {
            string message = "";
            List<CampaignViewModel> campaignList = new List<CampaignViewModel>();

            if (ModelState.IsValid)
            {
                campaignList = CampaignRepository.GetCampaignsToPublish();
                message = "success";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = campaignList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }




        [HttpPost]
        public JsonResult CancelCampaign(int campaignId)
        {
            string message = "";
            int result = 0;
            int trackerStatus = 0;
            //campaignId = 1002;

            if (ModelState.IsValid)
            {
                result = CampaignRepository.CancelPublish(campaignId);
                trackerStatus = CampaignRepository.UpdateTrackerTable(campaignId, SignageConstants.CAMPAIGNCANCEL);
                message = SignageConstants.SUCCESS;
                campaignHistoryRepository.AddtoCampaignHistory(campaignId, SignageConstants.CAMPAIGNCANCEL, 1);
            }
            else
            {
                message = SignageConstants.ERROR;
            }
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult PublishCampaign(int campaignId)
        {
            string message = "";
            int result = 0;
            int trackerStatus = 0;
            //campaignId = 1002;

            if (ModelState.IsValid)
            {
                result = CampaignRepository.PublishCampaign(campaignId);
                trackerStatus = CampaignRepository.UpdateTrackerTable(campaignId, SignageConstants.CAMPAIGNPUBLISH);
                campaignHistoryRepository.AddtoCampaignHistory(campaignId, SignageConstants.CAMPAIGNPUBLISH, 1);
                message = SignageConstants.SUCCESS;
            }
            else
            {
                message = SignageConstants.ERROR;
            }



            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetAllStations()
        {
            string message = "";
            List<StationViewModel> displayList = new List<StationViewModel>();

            if (ModelState.IsValid)
            {
                displayList = CampaignRepository.GetAllDiplayStations();
                message = "success";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = displayList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetAllSubGroupNames()
        {
            string message = "";
            List<PlayerGroupViewModel> displayList = new List<PlayerGroupViewModel>();

            if (ModelState.IsValid)
            {
                displayList = playerRepository.GetGroupDetails();
                message = "success";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = displayList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetUnassignedDeviceNames()
        {
            string message = "";
            List<PlayerViewModel> displayList = new List<PlayerViewModel>();

            if (ModelState.IsValid)
            {
                displayList = playerRepository.GetUnassignedDevices();
                message = "success";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = displayList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult GetApprovedScenes()
        {
            string message = "";
            List<SavedSceneViewModel> approvedSceneList = new List<SavedSceneViewModel>();

            if (ModelState.IsValid)
            {
                approvedSceneList = CampaignRepository.GetApprovedScenes();
                message = "success";
            }
            else
            {
                message = "Failed";
            }
            return new JsonResult { Data = approvedSceneList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        private string GetDaysofWeek(string days)
        {
            string scenes = "";

            if (days.Length > 0)
            {
                scenes = days.Substring(1, days.Length - 2);
            }

            return scenes;
        }

    }
}
