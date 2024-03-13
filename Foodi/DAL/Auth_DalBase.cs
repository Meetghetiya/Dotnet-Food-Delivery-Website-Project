using APIDemo.DAL;
using Foodi.Areas.Admin.Models;
using Foodi.Models;
using System.Data;
using System.Data.SqlClient;

namespace Foodi.DAL
{
    public class Auth_DalBase : DAL_Helpers
    {

        #region ValidateUserFromDatabase
        public bool ValidateUserFromDatabase(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnString))
                {

                    connection.Open();
                    using (SqlCommand command = new SqlCommand("PR_User_Validate", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Password", user.Password); // In a real application, you should hash the password before comparing
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region ValidateUser
        public User ValidateUser(User user)
        {
            User users = null;
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                string storedProcedure = "PR_User_Validate";
                SqlCommand command = new SqlCommand(storedProcedure, connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        users = new User
                        {
                            UserID = (int)reader["UserID"],
                            Name = reader["Name"] as string,
                            UserName = reader["UserName"] as string,
                            Mobile = reader["Mobile"] as string,
                            Email = reader["Email"] as string,
                            Address = reader["Address"] as string,
                            PostCode = reader["PostCode"] as string,
                            Password = reader["Password"] as string,
                            Image = reader["ImageUrl"] as string,
                            IsAdmin = (bool)reader["IsAdmin"],
                            CreatedDate = (DateTime)reader["CreatedDate"],
                            ModifiedDate = (DateTime)reader["ModifiedDate"]
                        };
                    }
                    else
                    {
                        return null; // User not found
                    }

                }

                return users;
            }
        }

        #endregion

        #region ValidateAdminFromDatabase
        public bool ValidateAdminFromDatabase(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnString))
                {

                    connection.Open();
                    using (SqlCommand command = new SqlCommand("PR_User_Validate", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@Password", user.Password); // In a real application, you should hash the password before comparing
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region ValidateUserByEmailFromDatabase
        public bool ValidateUserByEmailFromDatabase(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnString))
                {

                    connection.Open();
                    using (SqlCommand command = new SqlCommand("PR_User_ValidateByEmail", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Email", user.Email);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion

        #region InsertUser
        public bool InsertUser(User user)
        {

            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_User_Insert", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@UserName", user.Email);
                    command.Parameters.AddWithValue("@Mobile", user.Mobile);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    if (!string.IsNullOrEmpty(user.Address))
                    {
                        command.Parameters.AddWithValue("@Address", user.Address);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Address", DBNull.Value);
                    }
                    if (!string.IsNullOrEmpty(user.PostCode))
                    {
                        command.Parameters.AddWithValue("@PostCode", user.PostCode);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@PostCode", DBNull.Value);
                    }
                    /* command.Parameters.AddWithValue("@PostCode", user.PostCode);*/
                    command.Parameters.AddWithValue("@Password", user.Password);
                    if (!string.IsNullOrEmpty(user.Image))
                    {
                        command.Parameters.AddWithValue("@Image", user.Image);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Image", DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@IsAdmin", false);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                    command.ExecuteNonQuery();
                }

                return true;
            }
            #endregion

        }

    }
}

