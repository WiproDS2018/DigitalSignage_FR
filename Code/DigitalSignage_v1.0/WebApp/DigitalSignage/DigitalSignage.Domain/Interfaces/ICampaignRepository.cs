using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
    public interface ICampaignRepository : IDisposable
    {
        int AddCampaign(CampaignViewModel campaign);
        int EditCampaign(CampaignViewModel campaign);
        int DeleteCampaign(int campaignId);
        int PublishCampaign(int campaignId);
        int ActivateExpiredCampaign(int campaignId);
        int CancelPublish(int campaignId);
        List<CampaignViewModel> GetAllCampaigns();
        List<CampaignViewModel> GetCampaignsToPublish();
        List<CampaignViewModel> GetExpiredCampaigns();
        CampaignViewModel GetCampaign(int campaignId);
        List<StationViewModel> GetAllDiplayStations();
        List<SavedSceneViewModel> GetApprovedScenes();
        int UpdateTrackerTable(int campaignId,string status);
        int UpdateStartEndTime(int campaignId, string startTime, string endTime);
        int AddScenesToCampaign(List<int> SceneList, int CampaignId);
        int RemoveScenesFromCampaign(int CampaignId);

    }
}
