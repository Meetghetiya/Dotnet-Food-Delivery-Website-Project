using APIDemo.DAL;
using System.Data.SqlClient;
using System.Data;
using Foodi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Foodi.DAL
{
    public class Count_DalBase : DAL_Helpers
    {

        #region Count
        public Tuple<int, int, int , int , int > Count()
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            using (SqlCommand command = new SqlCommand("PR_Count", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Add OUTPUT parameters
                SqlParameter productCountParam = new SqlParameter("@ProductCount", SqlDbType.Int);
                productCountParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(productCountParam);

                SqlParameter categoryCountParam = new SqlParameter("@CategoryCount", SqlDbType.Int);
                categoryCountParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(categoryCountParam);

                SqlParameter userCountParam = new SqlParameter("@UserCount", SqlDbType.Int);
                userCountParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(userCountParam);

                SqlParameter orderPendingParam = new SqlParameter("@OrderPending", SqlDbType.Int);
                orderPendingParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(orderPendingParam);

                SqlParameter orderDeliverParam = new SqlParameter("@Orderdelivered", SqlDbType.Int);
                orderDeliverParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(orderDeliverParam);

                // Open the connection
                connection.Open();

                // Execute the command
                command.ExecuteNonQuery();

                // Retrieve the results from OUTPUT parameters
                int productCount = (int)productCountParam.Value;
                int categoryCount = (int)categoryCountParam.Value;
                int userCount = (int)userCountParam.Value;
                int orderPending = (int)orderPendingParam.Value;
                int orderDeliver = (int)orderDeliverParam.Value;

                return new Tuple<int, int, int , int , int>(productCount, categoryCount, userCount, orderPending , orderDeliver);
            }
        }
        #endregion
    }
}
