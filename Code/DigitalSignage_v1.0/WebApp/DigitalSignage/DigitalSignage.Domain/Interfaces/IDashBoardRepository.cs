using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
   public interface IDashBoardRepository : IDisposable
    {
        List<DashBoardDisplayModel> GetDashBoardCampaignDetails();
        List<DashBoardDisplayModel> GetDashBoardSceneDetails();
    }
}
