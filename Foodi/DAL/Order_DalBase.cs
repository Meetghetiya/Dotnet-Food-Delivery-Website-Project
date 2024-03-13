using APIDemo.DAL;
using Foodi.Areas.Dashboard.Models;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Foodi.DAL
{
    public class Order_DalBase : DAL_Helpers
    {
        #region InsertOrder
        public int InsertOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();


                using (SqlCommand command = new SqlCommand("PR_Order_Insert", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@OrderNo", order.OrderNo);
                    command.Parameters.AddWithValue("@UserId", order.UserId);
                    command.Parameters.AddWithValue("@Status", order.Status);
                    command.Parameters.AddWithValue("@PaymentId", order.PaymentId);
                    command.Parameters.AddWithValue("@OrderDate", order.OrderDate);

                    SqlParameter paymentIdParam = new SqlParameter("@OrderId", SqlDbType.Int);
                    paymentIdParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(paymentIdParam);

                    command.ExecuteNonQuery();
                    return Convert.ToInt32(paymentIdParam.Value);
                }
            }
        }
        #endregion

        #region InsertOrderItem
        public void InsertOrderItem(OrderItem orderItem)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();


                using (SqlCommand command = new SqlCommand("PR_OrderItem_Insert", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ProductId", orderItem.ProductId);
                    command.Parameters.AddWithValue("@Quantity", orderItem.Quantity);
                    command.Parameters.AddWithValue("@OrderId", orderItem.OrderId);

                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion



        #region GetOrdersByOrderId
        public Order GetOrdersByOrderId(int orderId)
        {

            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                Order order = new Order();
                string query = "SELECT * FROM Orders WHERE OrderId = @OrderId";

                using (SqlCommand command = new SqlCommand("PR_Order_GetById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@OrderId", orderId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            order = new Order
                            {
                                OrderId = (int)reader["OrderId"],
                                OrderNo = (string)reader["OrderNo"],
                                UserId = (int)reader["UserId"],
                                UserName = (string)reader["UserName"],
                                PaymentId = (int)reader["PaymentId"],
                                PaymentMode = (string)reader["PaymentMode"],
                                Address = (string)reader["Address"],
                                Status = (string)reader["Status"],
                                OrderDate = (DateTime)reader["OrderDate"]
                            };

                            return order;
                        }
                    }
                }
                return order;
            }
           

        }
        #endregion

        #region GetOrdersByUserId
        public List<Order> GetOrdersByUserId(int UserId)
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                string query = "SELECT * FROM Orders WHERE UserId = @UserId";

                using (SqlCommand command = new SqlCommand("PR_Order_GetByUserId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", UserId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order
                            {
                                OrderId = (int)reader["OrderId"],
                                OrderNo = (string)reader["OrderNo"],
                                UserId = (int)reader["UserId"],
                                UserName = (string)reader["UserName"],
                                PaymentMode = (string)reader["PaymentMode"],
                                Status = (string)reader["Status"],  
                                OrderDate = (DateTime)reader["OrderDate"]
                            };

                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }
        #endregion

        #region GetNotDeleteOrdersByUserId
        public List<Order> GetNotDeleteOrdersByUserId(int UserId)
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();


                using (SqlCommand command = new SqlCommand("PR_Order_GetNotDleleteByUserId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", UserId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order
                            {
                                OrderId = (int)reader["OrderId"],
                                OrderNo = (string)reader["OrderNo"],
                                UserId = (int)reader["UserId"],
                                UserName = (string)reader["UserName"],
                                PaymentMode = (string)reader["PaymentMode"],
                                Status = (string)reader["Status"],
                                OrderDate = (DateTime)reader["OrderDate"]
                            };

                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }
        #endregion

        #region GetOrdersByUserIdUserDelete
        public List<Order> GetOrdersByUserIdUserDelete(int UserId)
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                string query = "SELECT * FROM Orders WHERE UserId = @UserId";

                using (SqlCommand command = new SqlCommand("PR_Order_GetByUserIdDelete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", UserId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order
                            {
                                OrderId = (int)reader["OrderId"],
                                OrderNo = (string)reader["OrderNo"],
                                UserId = (int)reader["UserId"],
                                UserName = (string)reader["UserName"],
                                PaymentMode = (string)reader["PaymentMode"],
                                Status = (string)reader["Status"],
                                OrderDate = (DateTime)reader["OrderDate"]
                            };

                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }
        #endregion

        #region GetAllOrders
        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                string query = "SELECT * FROM Orders";

                using (SqlCommand command = new SqlCommand("PR_Order_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Order order = new Order
                            {
                                OrderId = (int)reader["OrderId"],
                                OrderNo = (string)reader["OrderNo"],
                                UserId = (int)reader["UserId"],
                                UserName = (string)reader["UserName"],
                                PaymentMode = (string)reader["PaymentMode"],
                                PaymentId = (int)reader["PaymentId"],
                                Status = (string)reader["Status"],
                                OrderDate = (DateTime)reader["OrderDate"]
                            };

                            orders.Add(order);
                        }
                    }
                }
            }

            return orders;
        }
        #endregion

        #region GetOrderItems
        public List<OrderItem> GetOrderItems(int orderId)
        {
            List<OrderItem> orderItems = new List<OrderItem>();

            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();


                using (SqlCommand command = new SqlCommand("PR_OrderItom_GetByOrderId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@OrderId", orderId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OrderItem orderItem = new OrderItem
                            {
                                OrderItemId = (int)reader["OrderItemId"],
                                OrderId = (int)reader["OrderId"],
                                ProductId = (int)reader["ProductId"],
                                ProductName = (string)reader["Name"],
                                Quantity = (int)reader["Quantity"],
                                Price = (decimal)reader["Price"]
                            };

                            orderItems.Add(orderItem);
                        }
                    }
                }
            }

            return orderItems;
        }
        #endregion

        #region DeleteOrderItem
        public void DeleteOrderItem(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM OrderItem WHERE OrderId = @OrderId", connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region DeleteOrder
        public void DeleteOrder(int orderId)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Orders WHERE OrderId = @OrderId", connection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region UpdateOrderStatus
        public void UpdateOrderStatus(int OrderId, string status)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("PR_Orders_UpdateStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderId", OrderId);
                    command.Parameters.AddWithValue("@Status", status);

                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region UpdateDeleteStatus
        public void UpdateDeleteStatus(int OrderId)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("PR_Orders_DeleteStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OrderId", OrderId);
                    command.Parameters.AddWithValue("@Status", true);

                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion
    }
}
