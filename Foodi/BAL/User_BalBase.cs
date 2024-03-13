using Foodi.Areas.Admin.Models;
using Foodi.DAL;
using Foodi.Models;

namespace Foodi.BAL
{
    public class User_BalBase
    {
        #region GetAllUsers
        public List<User> GetAllUsers(string searchString = null, bool? isAdmin = null)
        {

            User_DalBase User_DalBase = new User_DalBase();
            return User_DalBase.GetFilterUser(searchString , isAdmin);
        }
        #endregion

        #region GetUserById
        public User GetUserById(int UserId)
        {
            User_DalBase User_DalBase = new User_DalBase();
            return User_DalBase.GetUserById(UserId);
        }
        #endregion

        #region GetUserByEmail
        public User GetUserByEmail(string email)
        {
            User_DalBase User_DalBase = new User_DalBase();
            return User_DalBase.GetUserByEmail(email);
        }
        #endregion

        #region UpdateUser
        public bool UpdateUser(User user)
        {
            try
            {
                User_DalBase User_DalBase = new User_DalBase();
                User_DalBase.UpdateUser(user);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region DeleteUser
        public bool DeleteUser(int UserId)
        {
            try
            {
                User_DalBase User_DalBase = new User_DalBase();
                User_DalBase.DeleteUser(UserId);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }
}
