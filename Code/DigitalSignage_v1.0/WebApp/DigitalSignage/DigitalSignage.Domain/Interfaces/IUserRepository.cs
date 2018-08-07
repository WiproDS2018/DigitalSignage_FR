using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignage.Data.EF;
namespace DigitalSignage.Domain
{
    public interface IUserRepository:IDisposable
    {        
        int SaveUser(UserViewModel user);
        List<UserViewModel> GetUsers();
        UserViewModel GetUser(int userId);
        int EditUser(UserViewModel user);
        int Delete(int userId);
        List<UserRoleViewModel> GetUserRoles();
        int UpdateUserRoles(int userId,int roleId);
        int EditUserPassword(string oldPass,string newPass,int userId);
        int ResetUserPassword(UserViewModel password);

    }
}
