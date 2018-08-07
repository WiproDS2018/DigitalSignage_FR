using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
    public interface IDataValidator:IDisposable
    {
        string ValidatePlayerSerialNo(PlayerViewModel vmPlayer);
        string ValidatePlayerGroupName(PlayerGroupViewModel vmPlayerGroup);
        string ValidateStationName(StationViewModel vmStation);
        string ValidateCampaignName(CampaignViewModel vmCampaign);
        string ValidateSceneName(SceneViewModel vmScene);
        string ValidateStationDeletion(List<int> stationids);
        string ValidateSceneDeletion(List<int> sceneIds);
        string ValidateDeviceDeletion(List<int> PlayerIdList);
        string ValidateImageName(BackgroundImageStorageModel vmImage);
        string Test(int tt);
    }
}
