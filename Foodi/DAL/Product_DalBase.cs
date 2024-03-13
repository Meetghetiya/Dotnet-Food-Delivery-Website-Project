using APIDemo.DAL;
using Foodi.Areas.Admin.Models;
using System.Data.SqlClient;

namespace Foodi.DAL
{

    public class Product_DalBase : DAL_Helpers
    {

        #region GetAllProducts
        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_Product_Selectall", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                ProductId = (int)reader["ProductId"],
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = (decimal)reader["Price"],
                                Quantity = (int)reader["Quantity"],
                                ImageUrl = reader["ImageUrl"].ToString(),
                                CategoryName = reader["CategoryName"].ToString(),
                                CategoryId = (int)reader["CategoryId"],
                                IsActive = (bool)reader["IsActive"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                ModifiedDate = (DateTime)reader["ModifiedDate"]
                            };
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }
        #endregion

        #region GetFilterProducts
        public List<Product> GetFilterProducts(string searchName = null, bool? isActive = null)
        {
            List<Product> products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_Product_Filter", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ProductName ", (object)searchName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", isActive);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Product product = new Product
                            {
                                ProductId = (int)reader["ProductId"],
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = (decimal)reader["Price"],
                                Quantity = (int)reader["Quantity"],
                                ImageUrl = reader["ImageUrl"].ToString(),
                                CategoryName = reader["CategoryName"].ToString(),
                                CategoryId = (int)reader["CategoryId"],
                                IsActive = (bool)reader["IsActive"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                ModifiedDate = (DateTime)reader["ModifiedDate"]
                            };
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }
        #endregion

        #region GetProductById
        public Product GetProductById(int ProductId)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_Product_GetById", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ProductId", ProductId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Product product = new Product
                            {
                                ProductId = (int)reader["ProductId"],
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = (decimal)reader["Price"],
                                Quantity = (int)reader["Quantity"],
                                ImageUrl = reader["ImageUrl"].ToString(),
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = reader["CategoryName"].ToString(),
                                IsActive = (bool)reader["IsActive"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                ModifiedDate = (DateTime)reader["ModifiedDate"]
                            };
                            return product;
                        }
                        return null;
                    }
                }
            }
        }
        #endregion

        #region InsertProduct
        public void InsertProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("PR_Product_Insert", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Description", product.Description);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Quantity", product.Quantity);
                    command.Parameters.AddWithValue("@ImageUrl", product.ImageUrl);
                    command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                    command.Parameters.AddWithValue("@IsActive", product.IsActive);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region UpdateProduct
        public void UpdateProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_Product_Update", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ProductId", product.ProductId);
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Description", product.Description);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Quantity", product.Quantity);

                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        command.Parameters.AddWithValue("@ImageUrl", product.ImageUrl);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ImageUrl", GetExistingImagePath(product.ProductId));
                    }
                    command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                    command.Parameters.AddWithValue("@IsActive", product.IsActive);
                    command.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region DeleteProducts
        public void Delete(int ProductId)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_Product_DeleteById", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductId", ProductId);
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion


        #region GetExistingImagePath
        private string GetExistingImagePath(int productId)
        {
            string imagePath = null;
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT ImageUrl FROM Products WHERE ProductId = @ProductId", connection))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);

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
