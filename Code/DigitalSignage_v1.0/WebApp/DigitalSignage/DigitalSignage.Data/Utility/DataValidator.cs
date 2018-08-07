using System;
using System.Collections.Generic;
using System.Linq;
using DigitalSignage.Data.EF;
using DigitalSignage.Domain;
using System.Data;

namespace DigitalSignage.Data
{
    public class DataValidator : IDataValidator
    {
        SignageDBContext dbContext;
        string sceneUrl = "";

        public DataValidator()
        {
            dbContext = new SignageDBContext();
        }




        public string ValidatePlayerSerialNo(PlayerViewModel vmPlayer)
        {
            string statusMessage = "";

            var player = (dynamic)null;

            if (vmPlayer.PlayerId > 0)
            {
                player = dbContext.Players.Where(c => c.PlayerSerialNo == vmPlayer.PlayerSerialNo && c.PlayerId != vmPlayer.PlayerId).FirstOrDefault();
            }
            else
            {
                player = dbContext.Players.Where(c => c.PlayerSerialNo == vmPlayer.PlayerSerialNo).FirstOrDefault();
            }

            if (player != null)
            {
                statusMessage = SignageConstants.PLAYEREXISTS;
            }
            else
            {
                statusMessage = SignageConstants.SUCCESS;
            }
            return statusMessage;
        }

        public string ValidatePlayerGroupName(PlayerGroupViewModel vmPlayerGroup)
        {
            string statusMessage = "";

            var playergroup = (dynamic)null;

            if (vmPlayerGroup.GroupId > 0)
            {
                //playergroup = dbContext.PlayerGroups.Where(c => c.GroupName == vmPlayerGroup.GroupName).FirstOrDefault();
                playergroup = dbContext.PlayerGroups.Where(c => c.GroupName == vmPlayerGroup.GroupName && c.GroupId != vmPlayerGroup.GroupId).FirstOrDefault();
            }
            else
            {
                playergroup = dbContext.PlayerGroups.Where(c => c.GroupName == vmPlayerGroup.GroupName).FirstOrDefault();
            }

            if (playergroup != null)
            {
                statusMessage = SignageConstants.PLAYERGROUPEXIST;
            }
            else
            {
                statusMessage = SignageConstants.SUCCESS;
            }
            return statusMessage;
        }
        public string ValidateStationName(StationViewModel vmStation)
        {
            string statusMessage = "";
            var station = (dynamic)null;
            if (vmStation.DisplayStationid > 0)
            {
                station = dbContext.Displays.Where(c => c.DisplayName == vmStation.DisplayStationName && c.DisplayId != vmStation.DisplayStationid).FirstOrDefault();

            }
            else
            {
                station = dbContext.Displays.Where(c => c.DisplayName == vmStation.DisplayStationName).FirstOrDefault();
            }

            if (station != null)
            {
                statusMessage = SignageConstants.STATIONEXIST;
            }
            else
            {
                statusMessage = SignageConstants.SUCCESS;
            }
            return statusMessage;

        }

        public string ValidateCampaignName(CampaignViewModel vmCampaign)
        {
            string statusMessage = "";
            var campaign = (dynamic)null;
            if (vmCampaign.CampaignId > 0)
            {
                campaign = dbContext.Campaigns.Where(c => c.CampaignName == vmCampaign.CampaignName && c.CampaignId != vmCampaign.CampaignId).FirstOrDefault();
            }
            else
            {
                campaign = dbContext.Campaigns.Where(c => c.CampaignName == vmCampaign.CampaignName).FirstOrDefault();
            }
            if (campaign != null)
            {
                statusMessage = SignageConstants.CAMPAIGNEXIST;
            }
            else
            {
                statusMessage = SignageConstants.SUCCESS;
            }
            return statusMessage;
        }
        public string ValidateSceneName(SceneViewModel vmScene)
        {
            string statusMessage = "";
            var scene = (dynamic)null;
            if (vmScene.SceneId > 0)
            {
                scene = dbContext.Scenes.Where(c => c.SceneName == vmScene.SceneName && c.SceneId != vmScene.SceneId).FirstOrDefault();
            }
            else
            {
                scene = dbContext.Scenes.Where(c => c.SceneName == vmScene.SceneName).FirstOrDefault();
            }
            if (scene != null)
            {
                statusMessage = SignageConstants.SCENEEXISTS;
            }
            else
            {
                statusMessage = SignageConstants.SUCCESS;
            }
            return statusMessage;
        }

        public string ValidateImageName(BackgroundImageStorageModel vmImage)
        {
            string statusMessage = "";
            var image = (dynamic)null;
            if (vmImage.ImageId > 0)
            {
                image = dbContext.BackgroundImageStorages.Where(m => m.ImageName == vmImage.ImageName && m.ImageId != vmImage.ImageId).FirstOrDefault();
            }
            else
            {
                image = dbContext.BackgroundImageStorages.Where(m => m.ImageName == vmImage.ImageName).FirstOrDefault();
            }
            if (image != null)
            {
                statusMessage = SignageConstants.IMAGEEXISTS;
            }
            else
            {
                statusMessage = SignageConstants.SUCCESS;
            }
            return statusMessage;
        }

        public string ValidateStationDeletion(List<int> StationIdList)
        {
            string statusMessage = "";
            foreach (int stationid in StationIdList)
            {
                var station = dbContext.Campaigns.AsNoTracking().Where(c => c.DisplayId == stationid).FirstOrDefault();
                var stationname = dbContext.Displays.AsNoTracking().Where(c => c.DisplayId == stationid).FirstOrDefault();
                if (station != null)
                {
                    string campaignName = station.CampaignName.ToString();
                    string stationName = stationname.DisplayName.ToString();
                    statusMessage = "Device Group \"" + stationName + "\" is Already Assigned to \" " + campaignName + "\" PlayList. Can't delete the Device Group.";
                    return statusMessage;
                }
                else
                {
                    statusMessage = SignageConstants.SUCCESS;
                }
            }


            return statusMessage;
        }

        public string ValidateDeviceDeletion(List<int> PlayerIdList)
        {
            string statusMessage = "";

            var assignedDevice = (from item in dbContext.AssignPlayerDisplays select item.PlayerId).ToList();
            var assignedSubGroupDevice = (from dev in dbContext.PlayerGroupMappings.Where(dev => !assignedDevice.Contains(dev.PlayerId)) select dev.PlayerId).ToList();
            assignedDevice.AddRange(assignedSubGroupDevice);
            var devicelist_not_in_parent = dbContext.Players.Where(item => !assignedDevice.Contains(item.PlayerId)).ToList();
            foreach (int deviceid in PlayerIdList)
            {
                // var device = dbContext.DeviceContentTrackers.AsNoTracking().Where(d => d.DeviceId == deviceid).FirstOrDefault();
                //var campaign = dbContext.Campaigns.AsNoTracking().Select
                bool status = IsDeviceStandAlone(devicelist_not_in_parent, deviceid);
                if (status)
                {

                    var delDevice = dbContext.Campaigns.AsNoTracking().Where(del => del.DisplayId == deviceid).FirstOrDefault();
                    if (delDevice != null)
                    {

                        statusMessage = "Device attached to Playlist cannot be Deleted!";
                        return statusMessage;
                    }
                    else
                    {
                        statusMessage = SignageConstants.SUCCESS;
                    }
                }
                else
                {
                    statusMessage = "Device attached to Device Group cannot be Deleted!";
                }
            }
            return statusMessage;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string Test(int tt)
        {
            throw new NotImplementedException();
        }


        public string ValidateSceneDeletion(List<int> sceneIds)
        {
            string statusMessage = "";
            foreach (int id in sceneIds)
            {
                var campScenes = dbContext.CampaignSceneMappings.AsNoTracking().Where(c => c.SceneId == id).FirstOrDefault();
                var scenes = dbContext.Scenes.AsNoTracking().Where(c => c.SceneId == id).FirstOrDefault();
                if (campScenes != null)
                {

                    string sceneName = scenes.SceneName.ToString();
                    statusMessage = "Scene \"" + sceneName + "\" is Already Assigned to Playlist. Can't delete the scene.";
                    return statusMessage;
                }
                else
                {
                    statusMessage = SignageConstants.SUCCESS;
                }
            }


            return statusMessage;
        }

        public bool IsDeviceStandAlone(List<Player> playerlist, int deviceid)
        {
            bool result = false;
            foreach (var player in playerlist)
            {
                if (player.PlayerId == deviceid)
                {
                    return result = true;
                }
                else
                {
                    result = false;
                }

            }
            return result;
        }
    }
}
