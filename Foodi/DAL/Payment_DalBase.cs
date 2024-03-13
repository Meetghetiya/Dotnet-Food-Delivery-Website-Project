using System.Data.SqlClient;
using System.Data;
using Foodi.Areas.Dashboard.Models;
using APIDemo.DAL;

namespace Foodi.DAL
{
    public class Payment_DalBase : DAL_Helpers
    {
        #region InsertPayment
        public int InsertPayment(Payment payment)
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("PR_Payment_Insert", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Name", (object)payment.Name ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CardNo", (object)payment.CardNo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ExpiryDate", (object)payment.ExpiryDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@CvvNo", (object)payment.CvvNo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Address", payment.Address);
                    command.Parameters.AddWithValue("@UserId", payment.UserId);
                    command.Parameters.AddWithValue("@PaymentMode", payment.PaymentMode);

                    SqlParameter paymentIdParam = new SqlParameter("@PaymentId", SqlDbType.Int);
                    paymentIdParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(paymentIdParam);

                    try
                    {
                        command.ExecuteNonQuery();
                        return Convert.ToInt32(paymentIdParam.Value);
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Error executing SQL command", ex);
                    }
                }
            }
        }
        #endregion

    }
}
