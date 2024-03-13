using APIDemo.DAL;
using Foodi.Models;
using System.Data.SqlClient;

namespace Foodi.DAL
{
    public class User_DalBase : DAL_Helpers
    {

        #region GetAllUser
        public List<User> GetAllUser()
        {

            List<User> users = new List<User>();
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("PR_User_SelectAll", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                UserID = reader["UserID"] as int?,
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

                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }
        #endregion

        #region GetFilterUser
        public List<User> GetFilterUser(string searchString = null, bool? isAdmin = null)
        {

            List<User> users = new List<User>();
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("PR_User_Filter", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SearchString", (object)searchString ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsAdmin", isAdmin);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                UserID = reader["UserID"] as int?,
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

                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }
        #endregion

        #region GetUserById
        public User GetUserById(int userId)
        {
            User user = null;
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_User_SelectById", connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@UserID", userId);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Name = reader["Name"].ToString(),
                            UserName = reader["UserName"].ToString(),
                            Mobile = reader["Mobile"].ToString(),
                            Email = reader["Email"].ToString(),
                            Address = reader["Address"].ToString(),
                            PostCode = reader["PostCode"].ToString(),
                            Password = reader["Password"].ToString(),
                            Image = reader["ImageUrl"].ToString(),
                            IsAdmin = (bool)reader["IsAdmin"],
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"])
                        };
                    }
                }
            }
            return user;
        }
        #endregion

        #region GetUserByEmail
        public User GetUserByEmail(string email)
        {
            User user = null;
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_User_ByEmail", connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Email", email);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Name = reader["Name"].ToString(),
                            UserName = reader["UserName"].ToString(),
                            Mobile = reader["Mobile"].ToString(),
                            Email = reader["Email"].ToString(),
                            Address = reader["Address"].ToString(),
                            PostCode = reader["PostCode"].ToString(),
                            Password = reader["Password"].ToString(),
                            Image = reader["ImageUrl"].ToString(),
                            IsAdmin = (bool)reader["IsAdmin"],
                            CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                            ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"])
                        };
                    }
                }
            }
            return user;
        }
        #endregion

        #region UpdateUser
        public void UpdateUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_User_Update", connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@UserID", user.UserID);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@UserName", user.Email);
                command.Parameters.AddWithValue("@Mobile", user.Mobile ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Address", user.Address ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@PostCode", user.PostCode ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Password", user.Password ?? (object)DBNull.Value);
                if (!string.IsNullOrEmpty(user.Image))
                {
                    command.Parameters.AddWithValue("@Image", user.Image);
                }
                else
                {
                    command.Parameters.AddWithValue("@Image", GetExistingImagePath(user.UserID));
                }
                command.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);
                command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                command.ExecuteNonQuery();
            }
        }
        #endregion

        #region DeleteUser
        public void DeleteUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("PR_User_Delete", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@UserID", userId);

                command.ExecuteNonQuery();
            }
        }
        #endregion

        #region GetExistingImagePath
        public string GetExistingImagePath(int? UserId)
        {
            string imagePath = null;
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT ImageUrl FROM Users WHERE UserId = @UserId", connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Check if the ImagePath column is not DBNull
                            if (reader["ImageUrl"] != DBNull.Value)
                            {
                                imagePath = reader["ImageUrl"].ToString();
                            }
                            else
                            {
                                imagePath = "null";
                            }
                        }
                    }
                }
            }

            return imagePath;
        }
        #endregion
    }
}
