using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignage.Data.EF;
using DigitalSignage.Domain;
using System.Data.SqlClient;

namespace DigitalSignage.Data
{
    public class CampaignRepository : ICampaignRepository
    {
        SignageDBContext dbContext;

        public CampaignRepository()
        {
            dbContext = SinageDBManager.Context;
        }

        #region Public Methods
        public int AddCampaign(CampaignViewModel vmCampaign)
        {
            int id = 0;
            try
            {
                var campaign = ToModel(vmCampaign, false);
                campaign.Status = SignageConstants.CAMPAIGNSCHEDULE;
                dbContext.Campaigns.Add(campaign);
                dbContext.SaveChanges();

                //Newly created Campaign
                id = dbContext.Campaigns.FirstOrDefault(p => p.CampaignName.Equals(vmCampaign.CampaignName)).CampaignId;

                //Adding Scenes to Campaign Mapping Table
                List<int> sceneList = new List<int>();
                sceneList = GetSceneList(vmCampaign.MultiSceneIds);
                AddScenesToCampaign(sceneList, id);

                //Updating Scene Status.

                foreach (int scenId in sceneList)
                {
                    var scene = dbContext.Scenes.Where(c => c.SceneId == scenId).FirstOrDefault();
                    scene.Status = SignageConstants.SCENESCHEDULE;
                    dbContext.Entry(scene).State = System.Data.EntityState.Modified;
                    dbContext.SaveChanges();
                }


            }
            catch (Exception ex)
            { throw ex; }
            return id;
        }
        public List<CampaignViewModel> GetExpiredCampaigns()
        {
            //var campaignList = (from ex in dbContext.Campaigns.AsNoTracking()
            //                    where ex.IsActive == true && ex.Published == true && ex.Status == "Expired"
            //                    orderby ex.CampaignName ascending
            //                    select ex).ToList();

            var campaignList = dbContext.Database
                                 .SqlQuery<CampaignViewModel>("SP_GetCampaignList")
                                 .ToList();

            var expiredCampaignList = (from x in campaignList
                                       where x.IsActive == true && x.IsPublished == true && x.Status == "Expired"
                                       orderby x.CampaignName ascending
                                       select x).ToList();

            return expiredCampaignList;

            //return ToViewModelList(campaignList);
        }

        public int CancelPublish(int campaignId)
        {
            int success = 0;

            try
            {
                var campaign = dbContext.Campaigns.Find(campaignId);
                campaign.Published = false;
                campaign.Status = SignageConstants.CAMPAIGNSCHEDULE;
                dbContext.Entry(campaign).State = System.Data.EntityState.Modified;
                dbContext.SaveChanges();

                //Updating Scene Status.
                string scenes = GetCampaignSceneIds(campaignId);
                string[] sceneList = scenes.Split(',');
                foreach (string sceneId in sceneList)
                {
                    int id = Convert.ToInt32(sceneId);
                    var scene = dbContext.Scenes.Where(c => c.SceneId == id).FirstOrDefault();
                    scene.Status = SignageConstants.SCENESCHEDULE;
                    dbContext.Entry(scene).State = System.Data.EntityState.Modified;
                    dbContext.SaveChanges();
                    success = 1;
                }
            }
            catch (Exception ex)
            {
                success = 0;
                throw ex;
            }
            return success;
        }

        public int DeleteCampaign(int campaignId)
        {
            int success = 0;
            try
            {

                RemoveScenesFromCampaign(campaignId);
                var campaign = dbContext.Campaigns.Find(campaignId);
                dbContext.Campaigns.Remove(campaign);
                dbContext.SaveChanges();
                success = 1;


            }
            catch (Exception ex)
            {
                success = 0;
                throw ex;
            }
            return success;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int EditCampaign(CampaignViewModel vmCampaign)
        {
            int success = 0;
            try
            {
                var campaign = dbContext.Campaigns.Where(c => c.CampaignId == vmCampaign.CampaignId).FirstOrDefault();
                campaign.DisplayId = vmCampaign.DisplayId;
                campaign.StartDate = vmCampaign.StartDate;
                campaign.EndDate = vmCampaign.EndDate;
                campaign.StartTime = vmCampaign.StartTime;
                campaign.EndTime = vmCampaign.EndTime;
                campaign.AccountID = vmCampaign.AccountID;
                campaign.UpdatedBy = vmCampaign.UpdatedBy;
                dbContext.Entry(campaign).State = System.Data.EntityState.Modified;
                dbContext.SaveChanges();
                success = 1;
            }
            catch (Exception ex)
            {
                success = 0;
                throw ex;
            }
            return success;
        }
        //Geting All Campaigns/Playlist which are published and not Expired (View Playlist)
        public List<CampaignViewModel> GetAllCampaigns()
        {
            //var campaignList = (from x in dbContext.Campaigns.AsNoTracking()
            //                    where x.IsActive == true && x.Published == true && x.Status !="Expired"
            //                    orderby x.CampaignName ascending
            //                    select x).ToList();


            //var clientIdParameter = new SqlParameter("@ClientId", 4);

            //var result = dbContext.Database
            //    .SqlQuery<CampaignViewModel>("GetResultsForCampaign @ClientId", clientIdParameter)
            //    .ToList();

            var campaignList = dbContext.Database
               .SqlQuery<CampaignViewModel>("SP_GetCampaignList")
               .ToList();

            var publishedCampaignList = (from x in campaignList
                                         where x.IsActive == true && x.IsPublished == true && x.Status != "Expired"
                                         orderby x.CampaignName ascending
                                         select x).ToList();

            return publishedCampaignList;

            //return ToViewModelList(result2);
        }

        public List<StationViewModel> GetAllDiplayStations()
        {
            var assignedDisplay = (from c in dbContext.AssignPlayerDisplays select c.DisplayId).ToList();
            var displayList = dbContext.Displays.Where(c => assignedDisplay.Contains(c.DisplayId)).ToList();

            return ToStationViewModelList(displayList);
        }

        public List<SavedSceneViewModel> GetApprovedScenes()
        {
            var sceneList = (from x in dbContext.Scenes
                             where x.IsPrimaryApproved == true
                             && x.IsActive == true
                             && (x.Status == SignageConstants.SCENEAPPROVE || x.Status == SignageConstants.SCENESCHEDULE || x.Status == SignageConstants.SCENEPUBLISH)
                             orderby x.SceneName ascending
                             select x).ToList();

            return ToApprovedSceneViewModelList(sceneList);
        }

        public CampaignViewModel GetCampaign(int campaignId)
        {
            var campaign = dbContext.Campaigns.Where(c => c.CampaignId == campaignId).FirstOrDefault();
            CampaignViewModel vmCampaign = new CampaignViewModel();
            vmCampaign = ToViewModel(campaign);
            return vmCampaign;
        }

        public int ActivateExpiredCampaign(int campaignId)
        {
            int success = 0;
            try
            {
                var campaign = dbContext.Campaigns.Where(c => c.CampaignId == campaignId).FirstOrDefault();
                campaign.Published = false;
                campaign.Status = SignageConstants.CAMPAIGNSCHEDULE;
                dbContext.Entry(campaign).State = System.Data.EntityState.Modified;
                dbContext.SaveChanges();

                //Updating Scene Status.               
                string scenes = GetCampaignSceneIds(campaignId);
                string[] sceneList = scenes.Split(',');
                foreach (string sceneId in sceneList)
                {
                    int id = Convert.ToInt32(sceneId);
                    var scene = dbContext.Scenes.Where(c => c.SceneId == id).FirstOrDefault();
                    scene.Status = SignageConstants.SCENESCHEDULE;
                    dbContext.Entry(scene).State = System.Data.EntityState.Modified;
                    dbContext.SaveChanges();
                }

                success = 1;
            }
            catch (Exception ex)
            {
                success = 0;
                throw ex;
            }
            return success;

        }

        public List<CampaignViewModel> GetCampaignsToPublish()
        {
            //var campaignList = (from x in dbContext.Campaigns.AsNoTracking()
            //                    where x.IsActive == true && x.Published == false
            //                    orderby x.CampaignName ascending
            //                    select x).ToList();

            var campaignList = dbContext.Database
               .SqlQuery<CampaignViewModel>("SP_GetCampaignList")
               .ToList();
            var savedCampaignList = (from x in campaignList
                                     where x.IsActive == true && x.IsPublished == false
                                     orderby x.CampaignName ascending
                                     select x).ToList();

            return savedCampaignList;

            //return ToViewModelList(campaignList);
        }

        public int PublishCampaign(int campaignId)
        {
            int success = 0;
            try
            {
                var campaign = dbContext.Campaigns.Where(c => c.CampaignId == campaignId).FirstOrDefault();
                campaign.Published = true;
                campaign.Status = SignageConstants.CAMPAIGNPUBLISH;
                dbContext.Entry(campaign).State = System.Data.EntityState.Modified;
                dbContext.SaveChanges();

                //Updating Scene Status.               
                string scenes = GetCampaignSceneIds(campaignId);
                string[] sceneList = scenes.Split(',');
                foreach (string sceneId in sceneList)
                {
                    int id = Convert.ToInt32(sceneId);
                    var scene = dbContext.Scenes.Where(c => c.SceneId == id).FirstOrDefault();
                    scene.Status = SignageConstants.SCENEPUBLISH;
                    dbContext.Entry(scene).State = System.Data.EntityState.Modified;
                    dbContext.SaveChanges();
                }

                success = 1;
            }
            catch (Exception ex)
            {
                success = 0;
                throw ex;
            }
            return success;
        }

        public int UpdateTrackerTable(int campaignId, string status)
        {
            int success = 0;
            try
            {
                SqlParameter[] sqlParams = {
                                            new SqlParameter("@CampaignId", campaignId),
                                            new SqlParameter("@Status", status)
                                                };
                dbContext.Database.ExecuteNonQuery("[dbo].[SP_UpdateDeviceTracker]", sqlParams);

                success = 1;
            }
            catch (Exception ex)
            {
                success = 0;
                throw ex;
            }
            return success;
        }



        #endregion

        #region Private Methods
        private Player ToModel(PlayerViewModel vmPlayer, bool modified)
        {
            Player player = new Player();

            if (modified)
            {
                player = dbContext.Players.Where(c => c.PlayerId == vmPlayer.PlayerId).FirstOrDefault();
            }

            player.PlayerSerialNo = vmPlayer.PlayerSerialNo;
            player.PlayerName = vmPlayer.PlayerName;
            //player.IsActive = vmPlayer.IsActive;
            //player.CreatedBy = vmPlayer.CreatedBy;
            //player.CreatedDate = DateTime.UtcNow;

            //if (vmPlayer.CreatedBy > 0)
            //{
            //    player.User = dbContext.Users.Find(vmPlayer.CreatedBy);
            //}
            return player;
        }

        private Campaign ToModel(CampaignViewModel vmCampaign, bool modified)
        {
            Campaign Campaign = new Campaign();
            if (modified)
            {
                Campaign = dbContext.Campaigns.Where(c => c.CampaignId == vmCampaign.CampaignId).FirstOrDefault();
            }
            Campaign.CampaignName = vmCampaign.CampaignName;
            Campaign.DisplayId = vmCampaign.DisplayId;
            Campaign.StartDate = vmCampaign.StartDate;
            Campaign.StartTime = vmCampaign.StartTime;
            Campaign.EndDate = vmCampaign.EndDate;
            Campaign.EndTime = vmCampaign.EndTime;
            Campaign.SceneId = vmCampaign.SceneId;
            Campaign.Frequency = vmCampaign.Frequency;
            Campaign.Interval = vmCampaign.Interval;
            Campaign.IsActive = true;
            Campaign.Status = vmCampaign.Status;
            Campaign.Published = false;
            Campaign.UpdatedDate = DateTime.UtcNow;
            Campaign.AccountID = vmCampaign.AccountID;
            Campaign.CreatedBy = vmCampaign.CreatedBy;
            Campaign.DeviceGroup = vmCampaign.Type;
            Campaign.Zone = vmCampaign.Zone;
            Campaign.DaysOfWeek = vmCampaign.DaysOfWeek;

            //if (vmPlayer.CreatedBy > 0)
            //{
            //Campaign.UpdatedBy = dbContext.Users.Find(vmPlayer.CreatedBy);
            //}


            return Campaign;
        }

        private CampaignViewModel ToViewModel(Campaign campaign)
        {
            CampaignViewModel vmCampaign = new CampaignViewModel();
            vmCampaign.CampaignId = campaign.CampaignId;
            vmCampaign.CampaignName = campaign.CampaignName;
            vmCampaign.DisplayId = campaign.DisplayId.Value;
            vmCampaign.StartDate = campaign.StartDate.Value;
            vmCampaign.StartTime = campaign.StartTime.Value;
            vmCampaign.EndDate = campaign.EndDate.Value;
            vmCampaign.EndTime = campaign.EndTime.Value;
            vmCampaign.SceneId = campaign.SceneId.Value;
            vmCampaign.Frequency = campaign.Frequency;
            vmCampaign.IsActive = campaign.IsActive.Value;
            vmCampaign.Status = campaign.Status;
            vmCampaign.IsPublished = campaign.Published.Value;
            vmCampaign.UpdatedTime = DateTime.UtcNow;

            //if (vmPlayer.CreatedBy > 0)
            //{
            //    player.User = dbContext.Users.Find(vmPlayer.CreatedBy);
            //}
            return vmCampaign;
        }

        // Not Used For GetAll,Publish and Expire Campaigns - Depricated.
        private List<CampaignViewModel> ToViewModelList(List<Campaign> campaignList)
        {

            List<CampaignViewModel> campaignViewList = new List<CampaignViewModel>();

            foreach (Campaign campaign in campaignList)
            {

                CampaignViewModel vmCampaign = new CampaignViewModel();
                vmCampaign.CampaignId = campaign.CampaignId;
                vmCampaign.CampaignName = campaign.CampaignName;

                vmCampaign.DisplayId = campaign.DisplayId.Value;
                vmCampaign.Type = campaign.DeviceGroup;
                if (vmCampaign.Type == "Parent Group")
                {
                    vmCampaign.DisplayName = (from pl in dbContext.Displays.AsNoTracking() where pl.DisplayId == vmCampaign.DisplayId select pl.DisplayName).SingleOrDefault();
                }
                else if (vmCampaign.Type == "Sub Group")
                {
                    vmCampaign.DisplayName = (from gl in dbContext.PlayerGroups.AsNoTracking() where gl.GroupId == vmCampaign.DisplayId select gl.GroupName).SingleOrDefault();
                }
                else
                {
                    vmCampaign.DisplayName = (from dl in dbContext.Players.AsNoTracking() where dl.PlayerId == vmCampaign.DisplayId select dl.PlayerName).SingleOrDefault();
                }
                vmCampaign.StartDate = campaign.StartDate.Value;
                vmCampaign.StartTime = campaign.StartTime.Value;
                vmCampaign.EndDate = campaign.EndDate.Value;
                vmCampaign.EndTime = campaign.EndTime.Value;
                vmCampaign.SceneId = campaign.SceneId.Value;
                vmCampaign.SceneName = GetCampaignSceneNames(campaign.CampaignId);
                vmCampaign.MultiSceneIds = GetCampaignSceneIds(campaign.CampaignId);
                vmCampaign.Frequency = campaign.Frequency;
                vmCampaign.IsActive = campaign.IsActive.Value;
                vmCampaign.Status = campaign.Status;
                vmCampaign.IsPublished = campaign.Published.Value;
                vmCampaign.UpdatedTime = DateTime.UtcNow;
                vmCampaign.DaysOfWeek = campaign.DaysOfWeek;

                string strDAte = campaign.StartTime.Value.ToString();

                //test  
                //string startTime = "";
                //string endTime = "";
                //DateTime strtDateTime = DateTime.Parse(vmCampaign.StartTime.ToString()).ToLocalTime();
                //startTime = strtDateTime.ToString("HH:mm:ss tt");
                //startTime = startTime.Substring(0, 8);

                //DateTime endDateTime = DateTime.Parse(vmCampaign.EndTime.ToString()).ToLocalTime();
                //endTime = endDateTime.ToString("HH:mm:ss tt");
                //endTime = endTime.Substring(0, 8);


                //vmCampaign.StartDateAndTime = StartDateAndTime.ToShortDateString() + " " + strtDateTime.ToString();
                //vmCampaign.StartDateAndTime= vmCampaign.StartDate.ToShortDateString() + " " + strtDateTime.ToString();
                //vmCampaign.StartDateAndTime = vmCampaign.StartDate.ToShortDateString() + "--" + strDAte + "-" + vmCampaign.StartTime.ToString();

                vmCampaign.StartDateAndTime = vmCampaign.StartDate.ToShortDateString() + " " + vmCampaign.StartTime.ToString();
                vmCampaign.EndDateAndTime = vmCampaign.EndDate.ToShortDateString() + " " + vmCampaign.EndTime.ToString();

                campaignViewList.Add(vmCampaign);

            }

            return campaignViewList;
        }

        private List<StationViewModel> ToStationViewModelList(List<Display> displayList)
        {

            List<StationViewModel> displayViewList = new List<StationViewModel>();

            foreach (Display display in displayList)
            {
                StationViewModel vmDisplay = new StationViewModel();
                vmDisplay.DisplayStationName = display.DisplayName;
                vmDisplay.DisplayStationLocation = display.Location;
                vmDisplay.DisplayStationid = display.DisplayId;

                displayViewList.Add(vmDisplay);

            }

            return displayViewList;
        }

        private List<SavedSceneViewModel> ToApprovedSceneViewModelList(List<Scene> sceneList)
        {

            List<SavedSceneViewModel> sceneViewList = new List<SavedSceneViewModel>();

            foreach (Scene scene in sceneList)
            {

                SavedSceneViewModel vmScene = new SavedSceneViewModel();
                vmScene.SceneId = scene.SceneId;
                vmScene.Approver = scene.Approver.Value;
                vmScene.Comments = scene.Comments;
                vmScene.IsActive = scene.IsActive.Value;
                vmScene.IsPrimaryApproved = scene.IsPrimaryApproved.Value;
                vmScene.SceneContent = scene.SceneContent.Trim();
                vmScene.SceneName = scene.SceneName;
                vmScene.SceneType = scene.SceneType;
                vmScene.SceneUrl = scene.SceneUrl;
                vmScene.Status = scene.Status;
                vmScene.UpdatedBy = scene.UpdatedBy.Value;
                vmScene.TemplateType = scene.TemplateType;
                vmScene.IconPosition = scene.IconPosition;
                sceneViewList.Add(vmScene);

            }

            return sceneViewList;
        }

        public int UpdateStartEndTime(int campaignId, string startTime, string endTime)
        {
            int success = 0;
            try
            {
                SqlParameter[] sqlParams = {
                                            new SqlParameter("@CampaignId", campaignId),
                                            new SqlParameter("@StartTime", startTime),
                                             new SqlParameter("@EndTime", endTime)
                                                };
                dbContext.Database.ExecuteNonQuery("[dbo].[SP_UpdateCampaignTime]", sqlParams);

                success = 1;
            }
            catch (Exception ex)
            {
                success = 0;
            }
            return success;
        }


        public int AddScenesToCampaign(List<int> sceneList, int campaignId)
        {
            //dbContext.CampaignSceneMappings

            int sucess = 0;
            try
            {
                var sceneCampaignMapping = (from ca in dbContext.CampaignSceneMappings where ca.CampaignId == campaignId select ca).ToList();
                foreach (var item in sceneCampaignMapping)
                {
                    dbContext.CampaignSceneMappings.Remove(item);
                    dbContext.SaveChanges();
                }


                foreach (int scene in sceneList)
                {
                    CampaignSceneMapping campaignSceneMapping = new CampaignSceneMapping();
                    campaignSceneMapping.CampaignId = campaignId;
                    campaignSceneMapping.SceneId = scene;
                    dbContext.CampaignSceneMappings.Add(campaignSceneMapping);
                    dbContext.SaveChanges();
                    sucess = 1;
                }
            }
            catch (Exception ex)
            {
                sucess = 0;
                throw ex;
            }

            return sucess;
        }

        public int RemoveScenesFromCampaign(int campaignId)
        {
            int sucess = 0;
            try
            {
                var sceneCampaignMapping = (from ca in dbContext.CampaignSceneMappings where ca.CampaignId == campaignId select ca).ToList();
                foreach (var item in sceneCampaignMapping)
                {
                    dbContext.CampaignSceneMappings.Remove(item);
                    dbContext.SaveChanges();
                }
                sucess = 1;
            }
            catch (Exception ex)
            {
                sucess = 0;
                throw ex;
            }

            return sucess;
        }


        #endregion

        private string GetCampaignSceneNames(int campaignId)
        {
            string sceneNames = "";

            var sceneCampaignMapping = (from ca in dbContext.CampaignSceneMappings where ca.CampaignId == campaignId select ca).ToList();
            foreach (var item in sceneCampaignMapping)
            {
                string scene = (from sc in dbContext.Scenes where sc.SceneId == item.SceneId select sc.SceneName).SingleOrDefault();
                sceneNames += scene + ",";
            }
            sceneNames = sceneNames.TrimEnd(',');

            return sceneNames;
        }

        private string GetCampaignSceneIds(int campaignId)
        {
            string sceneIds = "";

            var sceneCampaignMapping = (from ca in dbContext.CampaignSceneMappings where ca.CampaignId == campaignId select ca).ToList();
            foreach (var item in sceneCampaignMapping)
            {
                sceneIds += item.SceneId.ToString() + ",";
            }
            sceneIds = sceneIds.TrimEnd(',');

            return sceneIds;
        }

        private List<int> GetSceneList(string sceneIds)
        {
            List<int> sceneList = new List<int>();

            if (sceneIds.Length > 0)
            {
                string scenes = sceneIds.Substring(1, sceneIds.Length - 2);
                string[] scene = scenes.Split(',');

                foreach (string id in scene)
                {
                    sceneList.Add(Convert.ToInt32(id));
                }
            }


            return sceneList;
        }


    }
}
