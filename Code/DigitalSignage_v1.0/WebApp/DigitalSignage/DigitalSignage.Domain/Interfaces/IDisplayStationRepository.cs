using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
    public interface IDisplayStationRepository : IDisposable
    {
        int AddDisplayStation(StationViewModel station);
        int EditDisplayStation(StationViewModel station);
        int DeleteDisplayStation(int diplayStationId);
        List<StationViewModel> GetAllDiplayStations();
        StationViewModel GetDiplayStation(int diplayStationId);
        List<PlayerViewModel> GetAvailablePlayers();
        List<PlayerViewModel> GetAssignnedPlayers(int diplayStationId);
        int AssignPlayers(List<int> playerIdList, int diplayStationId);
        List<PlayerByGroupTree> GetAvailablePlayersByGroup();
        List<PlayerByGroupTree> GetAssignedPlayersByGroup(int diplayStationId);
    }
}
