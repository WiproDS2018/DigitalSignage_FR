
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
    public class DashBoardRepository : IDashBoardRepository
    {
        SignageDBContext dBContext;

        public DashBoardRepository()
        {
            dBContext = SinageDBManager.Context;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public List<DashBoardDisplayModel> GetDashBoardCampaignDetails()
        {
            List<DashBoardDisplayModel> dashboardList = new List<DashBoardDisplayModel>();

            var statusmaping = (from sa in dBContext.Campaigns select sa.Status).Distinct().ToList();

            foreach (string status in statusmaping)
            {
                int count = (from sl in dBContext.Campaigns where sl.Status == status select sl).Count();

                DashBoardDisplayModel ddm = new DashBoardDisplayModel();

                ddm.Yvalue = count;
                ddm.Label = status;
                if (ddm.Label == "Scheduled")
                {
                    ddm.Label = "Unpublished";
                }
                dashboardList.Add(ddm);

            }
            return dashboardList;

        }

        public List<DashBoardDisplayModel> GetDashBoardSceneDetails()
        {
            List<DashBoardDisplayModel> dashboardList = new List<DashBoardDisplayModel>();

            var scenemaping = (from sa in dBContext.Scenes select sa.Status).Distinct().ToList();

            foreach (string status in scenemaping)
            {
                int count = (from sl in dBContext.Scenes where sl.Status == status select sl).Count();

                DashBoardDisplayModel ddm = new DashBoardDisplayModel();

                ddm.Yvalue = count;
                ddm.Label = status;
                if (ddm.Label == "Submitted")
                {
                    ddm.Label = "Pending for approval";
                }
                if (ddm.Label == "Scheduled")
                {
                    ddm.Label = "Ready for Publish";
                }

                dashboardList.Add(ddm);

            }
            return dashboardList;
        }

        public List<DashBoardDisplayModel> GetDashBoardDeviceDetails()
        {
            DashBoardDisplayModel ddm = new DashBoardDisplayModel();
            DashBoardDisplayModel ddno = new DashBoardDisplayModel();
            List<DashBoardDisplayModel> dashboardList = new List<DashBoardDisplayModel>();
            var devicemaping = (from dm in dBContext.DeviceContentTrackers select dm.Status).Distinct().ToList();
            //List<int> showPlayers = new List<int>();
            // Getting all players which are assigned
            var showPlayers = (from pl in dBContext.DeviceContentTrackers.AsNoTracking() where pl.Published == true select pl.DeviceId).Distinct().ToList();
            // Getting all players which are not Show
            var playerList = (dBContext.Players.AsNoTracking().Where(x => !showPlayers.Contains(x.PlayerId)).ToList());


            ddm.Yvalue = showPlayers.Count(); ;
            ddm.Label = "Show";
            dashboardList.Add(ddm);
            ddno.Yvalue = playerList.Count(); ;
            ddno.Label = "No Show";
            dashboardList.Add(ddno);


            return dashboardList;

        }

        public List<DashBoardDisplayModel> GetDeviceLocationDashboard()
        {
            List<DashBoardDisplayModel> dashboardList = new List<DashBoardDisplayModel>();

            var locationlist = (from nm in dBContext.Displays.AsNoTracking() select nm).ToList();

            foreach (var location in locationlist)
            {
                DashBoardDisplayModel ddm = new DashBoardDisplayModel();
                var devicelist = (from dev in dBContext.AssignPlayerDisplays.AsNoTracking() where dev.DisplayId == location.DisplayId select dev).ToList();
                int count = devicelist.Count;
                ddm.Label = location.DisplayName;
                ddm.Yvalue = count;

                List<Devices> dList = new List<Devices>();

                foreach (var dev in devicelist)
                {

                    Devices device = new Devices();
                    device.DeviceId = dev.PlayerId;
                    string dname = (from dnm in dBContext.Players where dnm.PlayerId == dev.PlayerId select dnm.PlayerName).SingleOrDefault();
                    var plGroupMap = (from plg in dBContext.PlayerGroupMappings where plg.PlayerId == dev.PlayerId select plg).SingleOrDefault();
                    if (plGroupMap != null)
                    {
                        var subgroup = dBContext.PlayerGroups.Find(plGroupMap.GroupId);
                        dname = subgroup.GroupName + "->" + dname;
                    }
                    device.DeviceName = dname;
                    dList.Add(device);

                }
                //if (dList.Count == 0)
                //{
                //    Devices device = new Devices();
                //    device.DeviceId = 0;
                //    device.DeviceName = "NoDevce";
                //    dList.Add(device);
                //}
                ddm.DeviceList = dList;
                dashboardList.Add(ddm);

            }

            var subgrouplist = (from nm in dBContext.PlayerGroups.AsNoTracking() select nm).ToList();

            foreach (var subgroup in subgrouplist)
            {
                DashBoardDisplayModel ddm = new DashBoardDisplayModel();
                var devicelist = (from dev in dBContext.PlayerGroupMappings.AsNoTracking() where dev.GroupId == subgroup.GroupId select dev).ToList();
                int count = devicelist.Count;
                ddm.Label = subgroup.GroupName;
                ddm.Yvalue = count;

                List<Devices> dList = new List<Devices>();

                foreach (var dev in devicelist)
                {

                    Devices device = new Devices();
                    string dname = (from dnm in dBContext.Players where dnm.PlayerId == dev.PlayerId select dnm.PlayerName).SingleOrDefault();
                    device.DeviceId = dev.PlayerId;
                    device.DeviceName = dname;
                    dList.Add(device);

                }
                ddm.DeviceList = dList;
                dashboardList.Add(ddm);

            }

            // Stand alone Devices

            var assignedDevice = (from item in dBContext.AssignPlayerDisplays select item.PlayerId).ToList();
            var assignedSubGroupDevice = (from dev in dBContext.PlayerGroupMappings.Where(dev => !assignedDevice.Contains(dev.PlayerId)) select dev.PlayerId).ToList();
            assignedDevice.AddRange(assignedSubGroupDevice);
            var devicelist_not_in_parent = dBContext.Players.Where(item => !assignedDevice.Contains(item.PlayerId)).ToList();
            if (devicelist_not_in_parent.Count > 0)
            {
                DashBoardDisplayModel ddm = new DashBoardDisplayModel();
                ddm.Label = "No Group";
                ddm.Yvalue = devicelist_not_in_parent.Count;
                List<Devices> dList = new List<Devices>();
                foreach (var dev in devicelist_not_in_parent)
                {
                    Devices device = new Devices();
                    string dname = (from dnm in dBContext.Players where dnm.PlayerId == dev.PlayerId select dnm.PlayerName).SingleOrDefault();
                    device.DeviceId = dev.PlayerId;
                    device.DeviceName = dname;
                    dList.Add(device);
                }
                ddm.DeviceList = dList;
                dashboardList.Add(ddm);
            }

            return dashboardList;
        }


    }
}
