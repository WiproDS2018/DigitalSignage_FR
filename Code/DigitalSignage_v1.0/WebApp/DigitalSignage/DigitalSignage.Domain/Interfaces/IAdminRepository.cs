using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignage.Domain
{
    public interface IAdminRepository : IDisposable
    {
        
        int UpdateSceneStatus(SavedSceneViewModel savedScene);       
        List<SavedSceneViewModel> GetAllSavedScenes(int approver);
    }
}
