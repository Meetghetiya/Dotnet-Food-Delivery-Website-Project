using APIDemo.DAL;
using Foodi.Areas.Admin.Models;
using System.Data.SqlClient;
using System.Globalization;

namespace Foodi.DAL
{
    public class Category_DALBase : DAL_Helpers
    {
        #region GetCategory
        public List<Category> GetCategoriesFromDatabase()
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_Category_SelectAll", connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            CategoryId = (int)reader["CategoryId"],
                            CategoryName = reader["Name"].ToString(),
                            Image = reader["ImageUrl"].ToString(),
                            IsActive = (bool)reader["IsActive"],
                            CreatedDate = (DateTime)reader["CreatedDate"],
                            ModifiedDate = (DateTime)reader["ModifiedDate"]
                        });
                    }
                }
            }
            return categories;
        }
        #endregion

        #region GetFilterCategory
        public List<Category> GetFilterCategories(string search = null)
        {
            List<Category> categories = new List<Category>();
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_Category_filter", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SearchString", (object)search ?? DBNull.Value);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            categories.Add(new Category
                            {
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = reader["Name"].ToString(),
                                Image = reader["ImageUrl"].ToString(),
                                IsActive = (bool)reader["IsActive"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                ModifiedDate = (DateTime)reader["ModifiedDate"]
                            });
                        }
                    }
                }
            }
            return categories;
        }
        #endregion

        #region GetCategoryById
        public Category GetCategoryByIdFromDatabase(int categoryId)
        {

            Category category = null;
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_Category_SelectById", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CategoryId", categoryId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            category = new Category
                            {
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = reader["Name"].ToString(),
                                Image = reader["ImageUrl"].ToString(),
                                IsActive = (bool)reader["IsActive"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                ModifiedDate = (DateTime)reader["ModifiedDate"]
                            };
                        }
                    }
                }
            }
            return category;
        }
        #endregion

        #region AddCategory
        public void AddCategoryToDatabase(Category category)
        {


            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("PR_Category_Insert", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                    command.Parameters.AddWithValue("@Image", (object)category.Image ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", category.IsActive);
                    command.Parameters.AddWithValue("@CreatedDate", DateTimeOffset.Now);
                    command.Parameters.AddWithValue("@ModifiedDate", DateTimeOffset.Now);
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region UpdateCategory
        public void UpdateCategoryInDatabase(Category category)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_UpdateCategory", connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Name ", category.CategoryName);

                    if (!string.IsNullOrEmpty(category.Image))
                    {
                        command.Parameters.AddWithValue("@Image", category.Image);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Image", GetExistingImagePath(category.CategoryId));
                    }
                    command.Parameters.AddWithValue("@IsActive", category.IsActive);
                    command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                    command.Parameters.AddWithValue("@CategoryId", category.CategoryId);

                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region DeleteCategory
        public void DeleteCategoryFromDatabase(int categoryId)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DeleteCategory", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CategoryId", categoryId);
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region GetExistingImagePath
        private string GetExistingImagePath(int categoryId)
        {
            string imagePath = null;
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT ImageUrl FROM Categories WHERE CategoryId = @CategoryId", connection))
                {
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);

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
