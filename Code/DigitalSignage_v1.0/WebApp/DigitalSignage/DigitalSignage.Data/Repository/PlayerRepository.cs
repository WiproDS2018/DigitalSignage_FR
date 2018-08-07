using System;
using System.Collections.Generic;
using System.Linq;
using DigitalSignage.Data.EF;
using DigitalSignage.Domain;
using System.Data;

namespace DigitalSignage.Data
{

    public class PlayerRepository : IPlayerRepository
    {
        SignageDBContext dbContext;
        public PlayerRepository()
        {
            dbContext = SinageDBManager.Context;
        }

        #region Public Methods
        public int SavePlayer(PlayerViewModel vmPlayer)
        {
            int t = 0;

            try
            {
                var model = ToModel(vmPlayer, false);
                dbContext.Players.Add(model);
                dbContext.SaveChanges();

                //Newly created player id
                t = dbContext.Players.FirstOrDefault(p => p.PlayerSerialNo.Equals(vmPlayer.PlayerSerialNo)).PlayerId;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return t;
        }

        public List<PlayerViewModel> GetPlayers()
        {

            var playerList = (from x in dbContext.Players
                              orderby x.PlayerName ascending
                              select x).ToList();


            return ToViewModelList(playerList);
        }

        public PlayerViewModel GetPlayer(int playerId)
        {
            var player = dbContext.Players.Where(c => c.PlayerId == playerId).FirstOrDefault();
            return ToViewModel(player);
        }

        public int Delete(int playerId)
        {
            int success = 0;
            try
            {

                var player = dbContext.Players.Find(playerId);
                dbContext.Players.Remove(player);
                dbContext.SaveChanges();

                var playerMapping = dbContext.PlayerGroupMappings.Where(c => c.PlayerId == playerId).FirstOrDefault();
                if (playerMapping != null)
                {
                    dbContext.PlayerGroupMappings.Remove(playerMapping);
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int EditPlayer(PlayerViewModel vmPlayer)
        {
            int success = 0;
            var player = ToModel(vmPlayer, true);
            try
            {
                dbContext.Entry(player).State = System.Data.EntityState.Modified;
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

        public List<PlayerViewModel> GetUnassignedDevices()
        {
            var devicelist_not_in_parent = (dynamic)null;
            try
            {
                var assignedDevice = (from item in dbContext.AssignPlayerDisplays select item.PlayerId).ToList();
                var assignedSubGroupDevice = (from dev in dbContext.PlayerGroupMappings.Where(dev => !assignedDevice.Contains(dev.PlayerId)) select dev.PlayerId).ToList();
                assignedDevice.AddRange(assignedSubGroupDevice);
                devicelist_not_in_parent = dbContext.Players.Where(item => !assignedDevice.Contains(item.PlayerId)).ToList();
            }
            catch (Exception ex)
            {              
                throw ex;
            }
            return ToPlayerViewModelList(devicelist_not_in_parent);


        }
        #endregion

        #region Private Methods
        private Player ToModel(PlayerViewModel vmPlayer, bool modified)
        {
            Player player = new Player();
            try
            {
                if (modified)
                {
                    player = dbContext.Players.Where(c => c.PlayerId == vmPlayer.PlayerId).FirstOrDefault();
                }
                else
                {
                    player.CreatedBy = vmPlayer.CreatedBy;
                }
                player.PlayerSerialNo = vmPlayer.PlayerSerialNo;
                player.PlayerName = vmPlayer.PlayerName;
                player.IsActive = true;
                player.UpdatedDate = DateTime.UtcNow;
                player.AccountID = vmPlayer.AccountID;

                player.UpdatedBy = vmPlayer.UpdatedBy;
                //player.CreatedDate = DateTime.UtcNow;           

                //if (vmPlayer.CreatedBy > 0)
                //{
                //    player.UpdatedBy = dbContext.Users.Find(vmPlayer.CreatedBy);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return player;
        }

        private PlayerViewModel ToViewModel(Player player)
        {
            PlayerViewModel vmplayer = new PlayerViewModel();
            try
            {
                vmplayer.PlayerSerialNo = player.PlayerSerialNo;
                vmplayer.PlayerName = player.PlayerName;
                vmplayer.PlayerId = player.PlayerId;
                //player.CreatedBy = vmPlayer.CreatedBy;
                //player.CreatedDate = DateTime.UtcNow;

                //if (vmPlayer.CreatedBy > 0)
                //{
                //    player.User = dbContext.Users.Find(vmPlayer.CreatedBy);
                //}
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return vmplayer;
        }

        private List<PlayerViewModel> ToViewModelList(List<Player> playerList)
        {

            List<PlayerViewModel> playerViewList = new List<PlayerViewModel>();
            try
            {
                foreach (Player play in playerList)
                {
                    PlayerViewModel vmplayer = new PlayerViewModel();
                    vmplayer.PlayerSerialNo = play.PlayerSerialNo;
                    vmplayer.PlayerName = play.PlayerName;
                    vmplayer.PlayerId = play.PlayerId;

                    playerViewList.Add(vmplayer);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return playerViewList;
        }


        private PlayerGroup ToGroupModel(PlayerGroupViewModel playergrpModel, bool modified)
        {
            PlayerGroup playergroup = new PlayerGroup();
            try
            {

                if (modified)
                {
                    playergroup = dbContext.PlayerGroups.Where(c => c.GroupId == playergrpModel.GroupId).FirstOrDefault();
                }
                else
                {
                    playergroup.CreatedBy = playergrpModel.CreatedBy;
                }
                playergroup.GroupId = playergrpModel.GroupId;
                playergroup.GroupName = playergrpModel.GroupName;
                playergroup.GroupDescription = playergrpModel.GroupDescription;
                playergroup.AccountID = playergrpModel.AccountID;
                playergroup.UpdatedBy = playergrpModel.UpdatedBy;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return playergroup;
        }
        private PlayerGroupViewModel ToGroupViewModel(PlayerGroup playerGrp)
        {
            PlayerGroupViewModel vmplayerGrp = new PlayerGroupViewModel();
            try
            {

                vmplayerGrp.GroupId = playerGrp.GroupId;
                vmplayerGrp.GroupName = playerGrp.GroupName;
                vmplayerGrp.GroupDescription = playerGrp.GroupDescription;
                //player.CreatedBy = vmPlayer.CreatedBy;
                //player.CreatedDate = DateTime.UtcNow;

                //if (vmPlayer.CreatedBy > 0)
                //{
                //    player.User = dbContext.Users.Find(vmPlayer.CreatedBy);
                //}
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return vmplayerGrp;
        }

        private List<PlayerGroupViewModel> ToGroupViewModelList(List<PlayerGroup> playerList)
        {

            List<PlayerGroupViewModel> playerGroupViewList = new List<PlayerGroupViewModel>();
            try
            {
                foreach (PlayerGroup play in playerList)
                {
                    PlayerGroupViewModel VMPlayerGroup = new PlayerGroupViewModel();
                    VMPlayerGroup.GroupId = play.GroupId;
                    VMPlayerGroup.GroupName = play.GroupName;
                    VMPlayerGroup.GroupDescription = play.GroupDescription;

                    playerGroupViewList.Add(VMPlayerGroup);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return playerGroupViewList;
        }

        private List<PlayerGroupViewModel> ToPlayerGroupViewModelList(List<PlayerGroup> playerList)
        {

            List<PlayerGroupViewModel> playerGroupViewList = new List<PlayerGroupViewModel>();
            try
            {
                foreach (PlayerGroup play in playerList)
                {
                    PlayerGroupViewModel VMPlayerGroup = new PlayerGroupViewModel();
                    VMPlayerGroup.GroupId = play.GroupId;
                    VMPlayerGroup.GroupName = play.GroupName;
                    VMPlayerGroup.GroupDescription = play.GroupDescription;

                    playerGroupViewList.Add(VMPlayerGroup);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return playerGroupViewList;
        }

        private PlayerGroupMapping ToModelPGMapping(PlayerGroupMappingViewModel vmPlayerGrpMapping, bool modified)
        {
            PlayerGroupMapping PGMapping = new PlayerGroupMapping();
            try
            {
                if (modified)
                {

                    PGMapping = dbContext.PlayerGroupMappings.Where(c => c.PlayerId == vmPlayerGrpMapping.PlayerId).FirstOrDefault();
                }

                PGMapping.GroupId = vmPlayerGrpMapping.GroupId;
                PGMapping.PlayerId = vmPlayerGrpMapping.PlayerId;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return PGMapping;
        }

        #endregion

        #region Player Group


        public int SaveGroup(PlayerGroupViewModel vmGroupmodel)
        {

            int t = 0;
            try
            {
                var playerGroup = ToGroupModel(vmGroupmodel, false);
                dbContext.PlayerGroups.Add(playerGroup);
                dbContext.SaveChanges();
                t = dbContext.PlayerGroups.FirstOrDefault(p => p.GroupId.Equals(playerGroup.GroupId)).GroupId;
            }
            catch (Exception ex)
            {
                t = 0;
                throw ex;

            }
            return t;


        }
        public List<PlayerGroupViewModel> GetGroups()
        {
            List<PlayerGroupViewModel> playergroupViewModelList = new List<PlayerGroupViewModel>();
            List<PlayerGroup> playerGroup = new List<PlayerGroup>();
            try
            {

                playerGroup = dbContext.PlayerGroups.ToList();
                playergroupViewModelList = ToGroupViewModelList(playerGroup);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return playergroupViewModelList;
        }

        public List<PlayerGroupViewModel> GetGroupDetails()
        {
            var assignedDevice = (from c in dbContext.PlayerGroupMappings select c.GroupId).ToList();
            var deviceList = dbContext.PlayerGroups.Where(c => assignedDevice.Contains(c.GroupId)).ToList();
            return ToGroupViewModelList(deviceList);

        }



        public PlayerGroupViewModel GetGroup(int i)
        {

            PlayerGroupViewModel PlayerGroupObj = new PlayerGroupViewModel();
            return PlayerGroupObj;
        }
        public int EditGroup(PlayerGroupViewModel vmPlayerGroup)
        {
            int success = 0;

            var playerGroup = ToGroupModel(vmPlayerGroup, true);
            try
            {

                dbContext.Entry(playerGroup).State = System.Data.EntityState.Modified;
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

        public int DeleteGroup(int groupid)
        {
            int success = 0;
            try
            {
                var playergrpmapping = dbContext.PlayerGroupMappings.Where(x => x.GroupId == groupid).ToList();
                foreach (var item in playergrpmapping)
                {
                    var playergroupmapping = dbContext.PlayerGroupMappings.Where(x => x.PlayerId == item.PlayerId).Single();
                    dbContext.PlayerGroupMappings.Remove(playergroupmapping);
                    dbContext.SaveChanges();
                }

                var PlayerModel = dbContext.PlayerGroups.Find(groupid);
                dbContext.PlayerGroups.Remove(PlayerModel);
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

        public List<PlayerJoinGroupViewmodel> GetPlayerJoinGroup()
        {
            List<PlayerJoinGroupViewmodel> list = new List<PlayerJoinGroupViewmodel>();
            try
            {          

                //New Approach with view
                //var playerList = dbContext.VWDevicewithGroups.AsNoTracking().ToList();
                //foreach (var player in playerList)
                //{
                //    PlayerJoinGroupViewmodel VMpjg = new PlayerJoinGroupViewmodel();
                //    VMpjg.PlayerId = player.PlayerId;
                //    VMpjg.PlayerName = player.PlayerName;
                //    VMpjg.PlayerserialNo = player.PlayerSerialNo;
                //    VMpjg.GroupId = player.GroupId;
                //    VMpjg.GroupName = player.GroupName;
                //    list.Add(VMpjg);
                //}
                var  playerList  = dbContext.Database
                    .SqlQuery<PlayerJoinGroupViewmodel>("SP_GetDeviceAndGroup")
                     .ToList();


                return playerList;


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return list;

        }

        #endregion

        #region Assign
        public List<PlayerViewModel> GetUngroupedPlayers()
        {
            var players = (from pl in dbContext.PlayerGroupMappings.AsNoTracking() select pl.PlayerId).ToList();
            //Changes
            var playersParent = (from pl in dbContext.AssignPlayerDisplays.AsNoTracking() select pl.PlayerId).ToList();
            players.AddRange(playersParent);
            //
            var playerList = (dbContext.Players.AsNoTracking().Where(x => !players.Contains(x.PlayerId)).ToList());

            return ToPlayerViewModelList(playerList);

        }
        public List<PlayerViewModel> GetGroupedPlayers(int GroupId)
        {
            var PlayerGroupmapping = (from pl in dbContext.PlayerGroupMappings.AsNoTracking() where pl.GroupId == GroupId select pl.PlayerId).ToList();

            var playerList = (dbContext.Players.AsNoTracking().Where(x => PlayerGroupmapping.Contains(x.PlayerId)).ToList());

            return ToPlayerViewModelList(playerList);
        }

        public int SavePlayersToGroup(List<int> PlayerList, int Groupid)
        {
            int sucess = 0;
            try
            {
                var PlayerGrpmapping = (from pl in dbContext.PlayerGroupMappings where pl.GroupId == Groupid select pl).ToList();
                foreach (var item in PlayerGrpmapping)
                {
                    dbContext.PlayerGroupMappings.Remove(item);
                    dbContext.SaveChanges();
                }


                foreach (int item in PlayerList)
                {
                    PlayerGroupMapping PlayerGroupmapping = new PlayerGroupMapping();
                    PlayerGroupmapping.PlayerId = item;
                    PlayerGroupmapping.GroupId = Groupid;
                    dbContext.PlayerGroupMappings.Add(PlayerGroupmapping);
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
    public class PlayerViewModelCompararer : IEqualityComparer<PlayerViewModel>
    {
        public bool Equals(PlayerViewModel x, PlayerViewModel y)
        {
            return x.PlayerId == y.PlayerId;
        }

        public int GetHashCode(PlayerViewModel obj)
        {
            return obj.PlayerId;
        }
    }
}
