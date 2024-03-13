using APIDemo.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Foodi.Areas.Admin.Models;

namespace Foodi.DAL
{
    public class Cart_DalBase : DAL_Helpers
    {
        #region InsertCart
        public void InsertCart(Cart cart)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("PR_Cart_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ProductId", cart.ProductId);
                        command.Parameters.AddWithValue("@Quantity", cart.Quantity);
                        command.Parameters.AddWithValue("@UserId", cart.UserId);
                        command.Parameters.AddWithValue("@CreatedDate", DateTimeOffset.Now);
                        command.Parameters.AddWithValue("@ModifiedDate", DateTimeOffset.Now);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region GetAllCarts
        public List<Cart> GetAllCarts()
        {
            List<Cart> carts = new List<Cart>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("PR_Cart_SelectAll", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Cart cart = new Cart
                                {
                                    CartId = Convert.ToInt32(reader["CartId"]),
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    ProductName = reader["ProductName"].ToString(),
                                    Quantity = Convert.ToInt32(reader["Quantity"]),
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    UserName = reader["UserName"].ToString(),
                                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                    ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"])
                                };

                                carts.Add(cart);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return carts;
        }
        #endregion

        #region GetFilterCart
        public List<Cart> GetFilterCarts(int? ProductId = null, int? UserId=null)
        {
            List<Cart> carts = new List<Cart>();
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_Cart_Filter", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductId", ProductId);
                    command.Parameters.AddWithValue("@UserId", UserId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Cart cart = new Cart
                            {
                                CartId = Convert.ToInt32(reader["CartId"]) ,
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductName = reader["ProductName"].ToString(),
                                Price = Convert.ToInt32(reader["Price"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                UserName = reader["UserName"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"])
                            };
                            carts.Add(cart);
                        }

                    }
                }
            }
            return carts;
        }
        #endregion

        #region GetCartByUserId
        public Cart GetCartbyUserId(int? ProductId = null, int? UserId = null)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_Cart_Filter", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductId", ProductId);
                    command.Parameters.AddWithValue("@UserId", UserId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Cart cart = new Cart
                            {
                                CartId = Convert.ToInt32(reader["CartId"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                ProductName = reader["ProductName"].ToString(),
                                Price = Convert.ToInt32(reader["Price"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                UserName = reader["UserName"].ToString(),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"])
                            };
                            return cart;
                        }

                    }
                }
            }
            return null;
        }
        #endregion

        #region UpdateCart
        public void UpdateCart(Cart cart)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("PR_Cart_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CartId", cart.CartId);
                    command.Parameters.AddWithValue("@ProductId", cart.ProductId);
                    command.Parameters.AddWithValue("@Quantity", cart.Quantity);
                    command.Parameters.AddWithValue("@UserId", cart.UserId);
                    command.Parameters.AddWithValue("@ModifiedDate", DateTimeOffset.Now);
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region GetCartById
        public Cart GetCartById(int cartId)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("PR_Cart_SelectById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CartId", cartId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Create a Cart object and populate its properties from the query result
                            Cart cart = new Cart
                            {
                                CartId = Convert.ToInt32(reader["CartId"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                                ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"])
                            };

                            return cart;
                        }
                    }
                }
                return null;
            }
        }
        #endregion

        #region DeleteCart
        public void DeleteCart(int cartId)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("PR_Cart_DeleteById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CartId", cartId);

                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region DeleteCartByUserId
        public void DeleteCartByUserId(int userId)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("PR_Cart_DeleteByUserId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserId", userId);

                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion
    }
}
