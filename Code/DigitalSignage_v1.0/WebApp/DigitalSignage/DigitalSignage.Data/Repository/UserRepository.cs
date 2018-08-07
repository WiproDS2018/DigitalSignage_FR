using System;
using System.Collections.Generic;
using System.Linq;
using DigitalSignage.Data.EF;
using DigitalSignage.Domain;
using System.Data;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace DigitalSignage.Data
{

    public class UserRepository : IUserRepository
    {
        SignageDBContext dbContext;
        public UserRepository()
        {
            dbContext = SinageDBManager.Context;
        }

        #region Public Methods
        public int SaveUser(UserViewModel userData)
        {
            int t = 0;
            try
            {
                var model = ToModel(userData, false);
                dbContext.Users.Add(model);
                dbContext.SaveChanges();

                //Newly created player id
                t = dbContext.Users.FirstOrDefault(u => u.UserName.Equals(userData.UserName)).UserId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return t;
        }

        public List<UserViewModel> GetUsers()
        {
            var userList = (from x in dbContext.Users
                            orderby x.UserName ascending
                            select x).ToList();

            return ToViewModelList(userList);
        }

        public UserViewModel GetUser(int userId)
        {
            var user = dbContext.Users.Where(c => c.UserId == userId).FirstOrDefault();
            return ToViewModel(user);

        }

        public UserViewModel GetUserByName(string userName)
        {
            var user = dbContext.Users.Where(c => c.UserName == userName).FirstOrDefault();
            return ToViewModel(user);
        }

        public int Delete(int userId)
        {
            int success = 0;
            try
            {
                var user = dbContext.Users.Find(userId);
                dbContext.Users.Remove(user);
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

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int EditUser(UserViewModel userData)
        {
            int success = 0;
            var user = ToModel(userData, true);
            try
            {
                dbContext.Entry(user).State = System.Data.EntityState.Modified;
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

        public int EditUserPassword(string oldPassword, string newPassword, int userId)
        {

            int success = 0;
            UserViewModel model = new UserViewModel();
            var user = dbContext.Users.Where(c => c.UserId == userId).FirstOrDefault();
            try
            {
                if (user.Password != oldPassword) return -1;
                if (user.Password == newPassword) return -2;
                user.Password = newPassword;
                dbContext.Entry(user).State = System.Data.EntityState.Modified;
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

        public int ResetUserPassword(UserViewModel password)
        {
            int success = 0;
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            int length = 10;
            UserViewModel model = new UserViewModel();
            var user = dbContext.Users.Where(c => c.UserName == password.UserName).FirstOrDefault();
            try
            {
                if (password != null)
                {
                    if ((user.UserName != password.UserName))
                    {
                        return -1;
                    }
                    else
                    {
                        Random random = new Random();

                        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                        var randomPass = new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
                        user.Password = randomPass;
                        dbContext.Entry(user).State = System.Data.EntityState.Modified;
                        dbContext.SaveChanges();
                        var mailStatus = SendEmail(user.Email, randomPass);
                        success = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                success = 0;
                throw ex;
            }
            return success;
        }

        private int SendEmail(string Email, string randomPass)
        {
            int success = 0;
            try
            {
                string message = "";
                
                if (!string.IsNullOrEmpty(randomPass))
                {
                    MailMessage mm = new MailMessage("mandahasa.anamika@gmail.com", Email.Trim());
                    mm.Subject = "Password Recovery";
                    mm.Body = string.Format("Hi {0},<br /><br />Your password is {1}.<br /><br />Thank You.", Email,randomPass);
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential();
                    NetworkCred.UserName = "mandahasa.anamika@gmail.com";
                    NetworkCred.Password = "rangitaranga";
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);

                    message = "Password has been sent to your email address.";
                    success = 1;
                }
                else
                {

                    message = "This email address does not match our records.";
                    success = 0;
                }
            }
            catch (Exception ex)
            {
                success = 0;
                throw ex;
            }
            return success;
        }
    

        public List<UserRoleViewModel> GetUserRoles()
        {

            var userList = (from x in dbContext.Users
                            orderby x.UserName ascending
                            select x).ToList();

            return ToUserRoleViewModelList(userList);
        }

        public int UpdateUserRoles(int userId, int roleId)
        {
            int success = 0;
            try
            {
                var user = dbContext.Users.Where(c => c.UserId == userId).FirstOrDefault();
                var roles = dbContext.Roles.Where(r => r.RoleId == roleId).FirstOrDefault();
                user.Role = roles.RoleName;
                dbContext.Entry(user).State = System.Data.EntityState.Modified;
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
        #endregion

        #region Private Methods
        private User ToModel(UserViewModel userData,bool modified)
        {
            User user = new User();

            if (modified)
            {
                user=dbContext.Users.Where(c => c.UserId == userData.UserId).FirstOrDefault();               
            }

            user.UserName = userData.UserName;
            user.Password = userData.Password;
            user.Email = userData.Email;
            user.IsActive =true;
            user.Role = userData.Role;
            user.AccountID = userData.AccountID;
            return user;
        }

        //private User ToModelPassword(string oldPassword,string newPassword,int UserId,bool modified)
        //{
        //    User currUser = new User();
        //    UserViewModel model = new UserViewModel();

        //    return 
            
        //    //if(model.UserId == UserId)
        //    //{
        //    //    pass = dbContext.Users.Where(p => p.Password == oldPassword).FirstOrDefault();
        //    //}


        //}

        private UserViewModel ToViewModel(User user)
        {
            UserViewModel userModel = new UserViewModel();
            userModel.UserId = user.UserId;
            userModel.UserName = user.UserName;
            userModel.Password = user.Password;
            userModel.IsActive = user.IsActive ?? false;
            userModel.Role = user.Role;

            return userModel;
        }

        private List<UserViewModel> ToViewModelList(List<User> userList)
        {

            List<UserViewModel> userViewList = new List<UserViewModel>();

            foreach (User user in userList)
            {
                UserViewModel userModel = new UserViewModel();
                userModel.UserId = user.UserId;
                userModel.UserName = user.UserName;
                userModel.Password = user.Password;
                userModel.Email = user.Email;
                userModel.Email = user.Email;
                userModel.IsActive = user.IsActive ?? false;
                userModel.Role = user.Role;
                userModel.AccountID = user.AccountID.GetValueOrDefault();
                userViewList.Add(userModel);

            }           
           
            return userViewList;
        }

        private List<UserRoleViewModel> ToUserRoleViewModelList(List<User> userList)
        {

            List<UserRoleViewModel> userRoleViewList = new List<UserRoleViewModel>();

            foreach (User user in userList)
            {
                UserRoleViewModel userRoleModel = new UserRoleViewModel();
                userRoleModel.UserName = user.UserName;
                userRoleModel.UserId = user.UserId;
                userRoleModel.Role = user.Role;
                userRoleModel.RoleList = GetRoles();

                userRoleViewList.Add(userRoleModel);
            }

            return userRoleViewList;
        }

        private List<RoleView> GetRoles()
        {
            List<RoleView> roleList = new List<RoleView>();

            var roles = dbContext.Roles.ToList();

            foreach (var role in roles)
            {
                roleList.Add(new RoleView()
                {
                    RoleId = role.RoleId,
                    RoleName = role.RoleName
                });
            }

            return roleList;
        }



        #endregion
    }
}
