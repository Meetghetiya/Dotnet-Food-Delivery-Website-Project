using Foodi.Areas.Dashboard.Models;
using Foodi.BAL;
using Foodi.DAL;
using Foodi.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Data.SqlClient;

namespace Foodi.Areas.Admin.Controllers
{
    [CheckAdminAccess]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]

    public class PaymentController : Controller
    {
        private readonly Order_DalBase _OrderService;
        private readonly IConfiguration configuration;
        public PaymentController(IConfiguration configuration, IToastNotification toastNotification)
        {
            this._OrderService = new Order_DalBase();
            this.configuration = configuration;
        }
        public IActionResult Payment()
        {
            List<Order> Orderdetail = _OrderService.GetAllOrders();

            return View(Orderdetail);
        }


        public IActionResult PaymentEdit(int orderId)
        {


            Order Order = _OrderService.GetOrdersByOrderId(orderId);
            List<OrderItem> item = _OrderService.GetOrderItems(orderId);

           
                OrderDetail OrderDetail = new OrderDetail
                {
                    Order = Order,
                    OrderItem = item
                };
                return View(OrderDetail);
            
              
            
        }

        [HttpPost]
        public IActionResult StatusEdit(string status , int orderId)
        {

            _OrderService.UpdateOrderStatus(orderId, status);
            Order Order = _OrderService.GetOrdersByOrderId(orderId);

            return Json(new { success = true, message = "Status updated successfully" , Status = Order.Status });

        }
    }
}
