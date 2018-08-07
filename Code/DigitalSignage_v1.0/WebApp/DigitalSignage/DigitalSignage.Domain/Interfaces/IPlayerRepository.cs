using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignage.Data.EF;
namespace DigitalSignage.Domain
{
    public interface IPlayerRepository:IDisposable
    {        
        int SavePlayer(PlayerViewModel player);
        List<PlayerViewModel> GetPlayers();
        PlayerViewModel GetPlayer(int playerId);
        int EditPlayer(PlayerViewModel player);
        int Delete(int playerId);
        List<PlayerViewModel> GetUnassignedDevices();

        int SaveGroup(PlayerGroupViewModel groupmodel);
        List<PlayerGroupViewModel> GetGroups();
        List<PlayerGroupViewModel> GetGroupDetails();
        PlayerGroupViewModel GetGroup(int i);
        int EditGroup(PlayerGroupViewModel vmPlayermodel);
        int DeleteGroup(int i);
        List<PlayerJoinGroupViewmodel> GetPlayerJoinGroup();

        List<PlayerViewModel> GetUngroupedPlayers();
        List<PlayerViewModel> GetGroupedPlayers(int GroupId);
        int SavePlayersToGroup(List<int> PlayerList, int GroupId);
    }
}
