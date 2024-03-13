using Foodi.Areas.Dashboard.Models;
using Foodi.DAL;

namespace Foodi.BAL
{
    public class Payment_BalBase
    {

        public bool ValidatePayment(Payment payment, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!System.Text.RegularExpressions.Regex.IsMatch(payment.CardNo, @"^\d{16}$"))
            {
                errorMessage = "Invalid card number format.";
                return false;
            }

            return true;
        }

        public int AddPayment(Payment payment)
        {
                Payment_DalBase Payment_DalBase = new Payment_DalBase();
                return Payment_DalBase.InsertPayment(payment);
        }

       
    }
}
