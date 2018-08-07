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
    public class CampaignHistoryRepository : ICampaignHistoryRepository
    {
        SignageDBContext dbContext;

        public CampaignHistoryRepository()
        {
            dbContext = SinageDBManager.Context;
        }
        public List<CampaignHistoryVM> GetAllCampaignHistory()
        {
            List<CampaignHistoryVM> vmcampHistList = new List<CampaignHistoryVM>();
            //var sceneCampaignMapping = (from ca in dbContext.VWCampaignHistories where ca.CampaignId == campaignId select ca).ToList();
            var sceneHistory = (from ca in dbContext.VWCampaignHistories select ca).ToList();
            vmcampHistList = ToHistViewModelList(sceneHistory);
            return vmcampHistList;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private List<CampaignHistoryVM> ToHistViewModelList(List<VWCampaignHistory> campHistList)
        {

            List<CampaignHistoryVM> vmcampHistList = new List<CampaignHistoryVM>();

            foreach (VWCampaignHistory campaign in campHistList)
            {

                CampaignHistoryVM vmHstCampaign = new CampaignHistoryVM();
                vmHstCampaign.CampaignId = campaign.CampaignId;
                vmHstCampaign.CampaignName = campaign.CampaignName;
                vmHstCampaign.DisplayId = campaign.DisplayId;
                vmHstCampaign.DisplayName = campaign.DisplayName;
                vmHstCampaign.StartDate = campaign.StartDate.Value;
                vmHstCampaign.StartTime = campaign.StartTime.Value;
                vmHstCampaign.EndDate = campaign.EndDate.Value;
                vmHstCampaign.EndTime = campaign.EndTime.Value;
                vmHstCampaign.SceneId = campaign.SceneId;
                vmHstCampaign.SceneName = campaign.SceneName;
                vmHstCampaign.PlayerName = campaign.Playername;
                //vmHstCampaign.MultiSceneIds = GetCampaignSceneIds(campaign.CampaignId);
                vmHstCampaign.Status = campaign.Status;               
                string strDAte = campaign.StartTime.Value.ToString();               
                //vmHstCampaign.StartDateAndTime = vmHstCampaign.StartDate.ToShortDateString() + " " + vmHstCampaign.StartTime.ToString();
                //vmHstCampaign.EndDateAndTime = vmHstCampaign.EndDate.ToShortDateString() + " " + vmHstCampaign.EndTime.ToString();
                vmHstCampaign.StartTimeVal = vmHstCampaign.StartDate.ToShortDateString() + " " + vmHstCampaign.StartTime.ToString();
                vmHstCampaign.EndTimeVal=  vmHstCampaign.EndDate.ToShortDateString() + " " + vmHstCampaign.EndTime.ToString();

                vmcampHistList.Add(vmHstCampaign);

            }

            return vmcampHistList;
        }


        public void AddtoCampaignHistory(int campaignId, string status, int userId)
        {
            try
            {
                SqlParameter[] sqlParams = {
                                            new SqlParameter("@CampaignId", campaignId),
                                            new SqlParameter("@Status", status),
                                             new SqlParameter("@UserId", userId)
                                                };
                dbContext.Database.ExecuteNonQuery("[dbo].[SP_AddCampHistory]", sqlParams);

              
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }


        public List<CampaignHistoryVM> GetCampaignHistory(string criteria, int id, DateTime strtDate, DateTime endDate)
        {
            List<CampaignHistoryVM> vmcampHistList = new List<CampaignHistoryVM>();
            //var sceneCampaignMapping = (from ca in dbContext.VWCampaignHistories where ca.CampaignId == campaignId select ca).ToList();
            if (criteria == "Station")
            {              
                var sceneHistory = (from ca in dbContext.VWCampaignHistories.AsNoTracking() where ca.DisplayId == id && ca.StartDate>=strtDate && ca.EndDate<= endDate select ca).ToList();           
                vmcampHistList = ToHistViewModelList(sceneHistory);
            }
            if (criteria == "Campaign")
            {
                var sceneHistory = (from ca in dbContext.VWCampaignHistories.AsNoTracking() where ca.CampaignId == id && ca.StartDate >= strtDate && ca.EndDate <= endDate select ca).ToList();
                vmcampHistList = ToHistViewModelList(sceneHistory);
            }
            if(criteria == "Player")
            {
                var sceneHistory = (from ca in dbContext.VWCampaignHistories.AsNoTracking() where ca.DeviceId == id && ca.StartDate >=strtDate && ca.EndDate <= endDate select ca).ToList();
                vmcampHistList = ToHistViewModelList(sceneHistory);
            }
            return vmcampHistList;
        }





        public List<CampaignViewModel> GetAllActiveCampaigns()
        {
            var campaignList = (from x in dbContext.Campaigns
                                where x.IsActive == true
                                orderby x.CampaignName ascending
                                select x).ToList();

            return ToViewModelList(campaignList);
        }

        private List<CampaignViewModel> ToViewModelList(List<Campaign> campaignList)
        {

            List<CampaignViewModel> campaignViewList = new List<CampaignViewModel>();

            foreach (Campaign campaign in campaignList)
            {
                CampaignViewModel vmCampaign = new CampaignViewModel();
                vmCampaign.CampaignId = campaign.CampaignId;
                vmCampaign.CampaignName = campaign.CampaignName;        
                campaignViewList.Add(vmCampaign);
            }

            return campaignViewList;
        }
    }


}
