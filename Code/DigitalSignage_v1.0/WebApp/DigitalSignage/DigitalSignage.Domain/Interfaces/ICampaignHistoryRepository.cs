using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
    public interface ICampaignHistoryRepository :IDisposable
    {
        List<CampaignHistoryVM> GetAllCampaignHistory();
        void AddtoCampaignHistory(int campaignId, string status, int userId);
        List<CampaignHistoryVM> GetCampaignHistory(string criteria, int id, DateTime strtDate, DateTime endDate);
        List<CampaignViewModel> GetAllActiveCampaigns();
    }
}
