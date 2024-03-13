using Foodi.DAL;
using Foodi.Models;

namespace Foodi.BAL
{
    public class Auth_BalBase
    {

        #region AddUser
        public bool AddUser(User user)
        {

            try
            {
                Auth_DalBase UserDAL = new Auth_DalBase();
                UserDAL.InsertUser(user);
                return true; 
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion

    /*    #region ValidateUser
        public bool ValidateUser(User user)
        {

            try
            {
                Auth_DalBase UserDAL = new Auth_DalBase();
                return UserDAL.ValidateUserFromDatabase(user);
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion*/

        public User ValidateUser(User user)
        {
            Auth_DalBase UserDAL = new Auth_DalBase();
            return UserDAL.ValidateUser(user);
        }

        #region ValidateUserByEmail
        public bool ValidateUserByEmail(User user)
        {

            try
            {
                Auth_DalBase UserDAL = new Auth_DalBase();
                return UserDAL.ValidateUserByEmailFromDatabase(user);
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion
    }
}
