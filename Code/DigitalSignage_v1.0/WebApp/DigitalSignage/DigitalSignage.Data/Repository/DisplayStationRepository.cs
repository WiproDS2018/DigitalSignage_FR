using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignage.Data.EF;
using DigitalSignage.Domain;

namespace DigitalSignage.Data
{
    public class DisplayStationRepository : IDisplayStationRepository
    {
        SignageDBContext dbContext;
        public DisplayStationRepository()
        {
            dbContext = SinageDBManager.Context;
        }

        public int AddDisplayStation(StationViewModel vmDisplay)
        {
            int id = 0;
            try
            {
                var display = ToModel(vmDisplay, false);
                dbContext.Displays.Add(display);
                dbContext.SaveChanges();

                //Newly created Display
                id = dbContext.Displays.FirstOrDefault(p => p.DisplayName.Equals(vmDisplay.DisplayStationName)).DisplayId;
            }
            catch (Exception ex)
            { throw ex; }
            return id;
        }

        public int DeleteDisplayStation(int Stationid)
        {
            int success = 0;
            try
            {
                var assignPlayers = dbContext.AssignPlayerDisplays.Where(c => c.DisplayId == Stationid).ToList();

                foreach (AssignPlayerDisplay assignPlayer in assignPlayers)
                {
                    if (assignPlayer != null)
                    {
                        dbContext.AssignPlayerDisplays.Remove(assignPlayer);
                        dbContext.SaveChanges();
                    }
                }

                var display = dbContext.Displays.Find(Stationid);
                dbContext.Displays.Remove(display);
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



        public int EditDisplayStation(StationViewModel vmStation)
        {
            int success = 0;
            var displayStation = ToModel(vmStation, true);
            try
            {
                dbContext.Entry(displayStation).State = System.Data.EntityState.Modified;
                dbContext.SaveChanges();
                success = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return success;
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public List<StationViewModel> GetAllDiplayStations()
        {
            var displayList = (from x in dbContext.Displays.AsNoTracking()
                               orderby x.DisplayName ascending
                               select x).ToList();

            return ToViewModelList(displayList);
        }

        public StationViewModel GetDiplayStation(int diplayStationId)
        {
            var display = dbContext.Displays.Where(c => c.DisplayId == diplayStationId).FirstOrDefault();
            return ToViewModel(display);
        }

        public List<PlayerViewModel> GetAvailablePlayers()
        {

            var players = (from pl in dbContext.AssignPlayerDisplays.AsNoTracking() select pl.PlayerId).ToList();

            var playerList = (dbContext.Players.AsNoTracking().Where(x => !players.Contains(x.PlayerId)).ToList());

            return ToPlayerViewModelList(playerList);
        }

        public List<PlayerViewModel> GetAssignnedPlayers(int diplayStationId)
        {

            var players = (from pl in dbContext.AssignPlayerDisplays.AsNoTracking() where pl.DisplayId == diplayStationId select pl.PlayerId).ToList();

            var playerList = (dbContext.Players.AsNoTracking().Where(x => players.Contains(x.PlayerId)).ToList());

            return ToPlayerViewModelList(playerList);


        }

        public int AssignPlayers(List<int> playerIdList, int diplayStationId)
        {
            int id = 0;
            try
            {
                var assignPlayers = dbContext.AssignPlayerDisplays.Where(c => c.DisplayId == diplayStationId).ToList();

                foreach (AssignPlayerDisplay assignPlayer in assignPlayers)
                {
                    if (assignPlayer != null)
                    {
                        dbContext.AssignPlayerDisplays.Remove(assignPlayer);
                        dbContext.SaveChanges();
                    }
                }

                foreach (int playerId in playerIdList)
                {

                    AssignPlayerDisplay assignPlayer = new AssignPlayerDisplay();
                    assignPlayer.PlayerId = playerId;
                    assignPlayer.DisplayId = diplayStationId;
                    assignPlayer.UpdatedDate = DateTime.UtcNow;
                    dbContext.AssignPlayerDisplays.Add(assignPlayer);
                    dbContext.SaveChanges();

                }

                id = 1;
               

            }
            catch (Exception ex)
            {  throw ex; }
            return id;
        }

        public List<PlayerByGroupTree> GetAvailablePlayersByGroup()
        {
            List<PlayerByGroupTree> groupTree = new List<PlayerByGroupTree>();

            // Getting all players which are assigned
            var players = (from pl in dbContext.AssignPlayerDisplays.AsNoTracking() select pl.PlayerId).ToList();
            // Getting all players which are not assigned
            var playerList = (dbContext.Players.AsNoTracking().Where(x => !players.Contains(x.PlayerId)).ToList());

            //selectedPlayersNotAssigned
            var selectedPlayersNotAssign = (from pl in playerList select pl.PlayerId).ToList();

            //var GroupIdList1 = (dbContext.PlayerGroupMappings.AsNoTracking().Where(x => selectedPlayersNotAssign.Contains(x.PlayerId)).Distinct().ToList());

            // Getting Group Id of players which are not assigned
            var groupIdList = (from plg in dbContext.PlayerGroupMappings where selectedPlayersNotAssign.Contains(plg.PlayerId) select plg.GroupId).Distinct().ToList();

            var groupedPlayerNotAssign = (from plg in dbContext.PlayerGroupMappings where selectedPlayersNotAssign.Contains(plg.PlayerId) select plg.PlayerId).Distinct().ToList();

            var orphanPlayerList = (from freepl in playerList.Where(x => !groupedPlayerNotAssign.Contains(x.PlayerId)) select freepl.PlayerId).ToList();

            //var orphanPlayersNotAssigned = (from pl in dbContext.Players where groupedPlayerNotAssign.Contains(pl.PlayerId) select pl.PlayerId).Distinct().ToList();

            foreach (int grpId in groupIdList)
            {

                PlayerByGroupTree plgroupTree = new PlayerByGroupTree();
                plgroupTree.GroupId = grpId;
                plgroupTree.GroupName = (from plg in dbContext.PlayerGroups where plg.GroupId == grpId select plg.GroupName).SingleOrDefault();
                plgroupTree.Indicator = true;

                var groupedPlayers = dbContext.PlayerGroupMappings.Where(c => c.GroupId == grpId && selectedPlayersNotAssign.Contains(c.PlayerId)).ToList();
                List<PlayerTree> groupedPlayList = new List<PlayerTree>();

                foreach (var grPlayer in groupedPlayers)
                {
                    PlayerTree playerChild = new PlayerTree();
                    playerChild.PlayerId = grPlayer.PlayerId;
                    playerChild.PlayerName = (from pl in dbContext.Players where pl.PlayerId == grPlayer.PlayerId select pl.PlayerName).SingleOrDefault();
                    playerChild.ParentId = grpId;
                    groupedPlayList.Add(playerChild);
                }

                plgroupTree.Players = groupedPlayList;
                groupTree.Add(plgroupTree);

            }

            foreach (int orphanPlayeId in orphanPlayerList)
            {
                PlayerByGroupTree plgroupTree = new PlayerByGroupTree();
                plgroupTree.GroupId = orphanPlayeId;
                plgroupTree.GroupName = (from pl in dbContext.Players where pl.PlayerId == orphanPlayeId select pl.PlayerName).SingleOrDefault();
                plgroupTree.Indicator = false;
                groupTree.Add(plgroupTree);
            }

            return groupTree;
        }

        public List<PlayerByGroupTree> GetAssignedPlayersByGroup(int diplayStationId)
        {
            List<PlayerByGroupTree> groupTree = new List<PlayerByGroupTree>();

            var players = (from pl in dbContext.AssignPlayerDisplays.AsNoTracking() where pl.DisplayId == diplayStationId select pl.PlayerId).ToList();

            //Assigned Players List
            var playerList = (dbContext.Players.AsNoTracking().Where(x => players.Contains(x.PlayerId)).ToList());
            //Assigned Players
            var assignedPlayers = (from pl in playerList select pl.PlayerId).ToList();
            //Assigned Players Group.
            var groupIdList = (from plg in dbContext.PlayerGroupMappings where assignedPlayers.Contains(plg.PlayerId) select plg.GroupId).Distinct().ToList();
            //Assigned PlayerIds Grouped.
            var groupedPlayersAssigned = (from plg in dbContext.PlayerGroupMappings where assignedPlayers.Contains(plg.PlayerId) select plg.PlayerId).Distinct().ToList();

            var assignedOrphanPlayerList = (from freepl in playerList.Where(x => !groupedPlayersAssigned.Contains(x.PlayerId)) select freepl.PlayerId).ToList();

            foreach (int grpId in groupIdList)
            {

                PlayerByGroupTree plgroupTree = new PlayerByGroupTree();
                plgroupTree.GroupId = grpId;
                plgroupTree.GroupName = (from plg in dbContext.PlayerGroups where plg.GroupId == grpId select plg.GroupName).SingleOrDefault();
                plgroupTree.Indicator = true;

                var groupedPlayers = dbContext.PlayerGroupMappings.Where(c => c.GroupId == grpId && assignedPlayers.Contains(c.PlayerId)).ToList();
                List<PlayerTree> groupedPlayList = new List<PlayerTree>();

                foreach (var grPlayer in groupedPlayers)
                {
                    PlayerTree playerChild = new PlayerTree();
                    playerChild.PlayerId = grPlayer.PlayerId;
                    playerChild.PlayerName = (from pl in dbContext.Players where pl.PlayerId == grPlayer.PlayerId select pl.PlayerName).SingleOrDefault();
                    playerChild.ParentId = grpId;
                    groupedPlayList.Add(playerChild);
                }

                plgroupTree.Players = groupedPlayList;
                groupTree.Add(plgroupTree);

            }
            foreach (int orphanPlayeId in assignedOrphanPlayerList)
            {
                PlayerByGroupTree plgroupTree = new PlayerByGroupTree();
                plgroupTree.GroupId = orphanPlayeId;
                plgroupTree.GroupName = (from pl in dbContext.Players where pl.PlayerId == orphanPlayeId select pl.PlayerName).SingleOrDefault();
                plgroupTree.Indicator = false;
                groupTree.Add(plgroupTree);
            }

            return groupTree;

        }


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

        private Display ToModel(StationViewModel vmDisplay, bool modified)
        {
            Display DisplayStation = new Display();
            if (modified)
            {
                DisplayStation = dbContext.Displays.Where(c => c.DisplayId == vmDisplay.DisplayStationid).FirstOrDefault();
            }
            else
            {
                DisplayStation.CreatedBy = vmDisplay.CreatedBy;
            }
            DisplayStation.DisplayName = vmDisplay.DisplayStationName;
            DisplayStation.Location = vmDisplay.DisplayStationLocation;
            DisplayStation.AccountID = vmDisplay.AccountID;
            DisplayStation.UpdatedBy = vmDisplay.UpdatedBy;
            
            //player.IsActive = vmPlayer.IsActive;
            //player.CreatedBy = vmPlayer.CreatedBy;
            //player.CreatedDate = DateTime.UtcNow;

            //if (vmPlayer.CreatedBy > 0)
            //{
            //    player.User = dbContext.Users.Find(vmPlayer.CreatedBy);
            //}


            return DisplayStation;
        }

        private StationViewModel ToViewModel(Display display)
        {
            StationViewModel vmDisplay = new StationViewModel();
            vmDisplay.DisplayStationName = display.DisplayName;
            vmDisplay.DisplayStationLocation = display.Location;
            vmDisplay.DisplayStationid = display.DisplayId;
            //player.CreatedBy = vmPlayer.CreatedBy;
            //player.CreatedDate = DateTime.UtcNow;

            //if (vmPlayer.CreatedBy > 0)
            //{
            //    player.User = dbContext.Users.Find(vmPlayer.CreatedBy);
            //}
            return vmDisplay;
        }

        private List<StationViewModel> ToViewModelList(List<Display> displayList)
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

        private List<PlayerViewModel> ToPlayerViewModelList(List<Player> playerList)
        {

            List<PlayerViewModel> playerViewList = new List<PlayerViewModel>();

            foreach (Player play in playerList)
            {
                PlayerViewModel vmplayer = new PlayerViewModel();
                vmplayer.PlayerSerialNo = play.PlayerSerialNo;
                vmplayer.PlayerName = play.PlayerName;
                vmplayer.PlayerId = play.PlayerId;

                playerViewList.Add(vmplayer);

            }

            return playerViewList;
        }


        #endregion
    }
}
