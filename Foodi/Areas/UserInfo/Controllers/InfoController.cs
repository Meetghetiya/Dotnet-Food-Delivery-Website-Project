using Foodi.Areas.Dashboard.Models;
using Foodi.BAL;
using Foodi.DAL;
using Foodi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Foodi.Areas.UserInfo.Controllers
{
    [Area("UserInfo")]
    [Route("UserInfo/[controller]/[action]")]
    public class InfoController : Controller
    {
        private readonly IToastNotification _toastNotification;
        private readonly User_BalBase _userbalbase;
        private readonly Order_DalBase _OrderService;
        private readonly Payment_DalBase _PaymentService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public InfoController(IWebHostEnvironment webHostEnvironment,IToastNotification toastNotification)
        {
            this.webHostEnvironment = webHostEnvironment;
            _toastNotification = toastNotification;
            _userbalbase = new User_BalBase();
            _PaymentService = new Payment_DalBase();
            this._OrderService = new Order_DalBase();
        }
        public IActionResult Detail()
        {
            string UserId = HttpContext.Session.GetString("UserId");

            if (!string.IsNullOrEmpty(UserId))
            {
                int UserID = int.Parse(UserId);
                User User = _userbalbase.GetUserById(UserID);
                List<Order> Orderdetail = _OrderService.GetOrdersByUserId(UserID);

                UserOrderTab userOrder = new UserOrderTab
                {
                    User = User,
                    Orders = Orderdetail,
                    ActivateConnectedServicesTab = false
                };

                return View(userOrder);
            }
            else
            {
                _toastNotification.AddInfoToastMessage("Login required.");
                return View("~/Views/Home/Index.cshtml");
            }

        }

        public IActionResult DeleteOrderAndItem(int orderId)
        {
            try
            {
                _OrderService.DeleteOrderItem(orderId);
                _OrderService.DeleteOrder(orderId);

                string UserId = HttpContext.Session.GetString("UserId");
                int UserID = int.Parse(UserId);
                User User = _userbalbase.GetUserById(UserID);
                List<Order> Orderdetail = _OrderService.GetOrdersByUserId(UserID);

                UserOrderTab userOrder = new UserOrderTab
                {
                    User = User,
                    Orders = Orderdetail,
                    ActivateConnectedServicesTab = true
                };
                return View("Detail", userOrder);
            }
            catch (Exception ex)
            {
                return View("Detail");
            }
        }

        public ActionResult EditUserDetail()
        {
            string UserId = HttpContext.Session.GetString("UserId");
            int UserID = int.Parse(UserId);
            User users = _userbalbase.GetUserById(UserID);
            return View(users);
        }

      
        public IActionResult SoftUserDelete(string status, int orderId)
        {
            try
            {
                _OrderService.UpdateOrderStatus(orderId, status);

                string UserId = HttpContext.Session.GetString("UserId");
                int UserID = int.Parse(UserId);
                User User = _userbalbase.GetUserById(UserID);
                List<Order> Orderdetail = _OrderService.GetNotDeleteOrdersByUserId(UserID);

                UserOrderTab userOrder = new UserOrderTab
                {
                    User = User,
                    Orders = Orderdetail,
                    ActivateConnectedServicesTab = true
                };
                return View("Detail", userOrder);
            }
            catch (Exception ex)
            {
                return View("Detail");
            }

         

        }

        [HttpPost]
        public ActionResult EditUserDetail(User user, IFormFile Image)
        {
            if (Image != null)
            {
                user.Image = SaveImageAndGetPath(Image);
            }else
            {
                user.Image = null;
            }
            _toastNotification.AddInfoToastMessage("Information successfully Update");
            _userbalbase.UpdateUser(user);
            return RedirectToAction("Detail");



        }

        #region SaveImageAndGetPath
        private string SaveImageAndGetPath(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
                string uniqfilename = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadPath, uniqfilename);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var relativePath = filePath.Replace(webHostEnvironment.WebRootPath, string.Empty)
                                  .TrimStart(Path.DirectorySeparatorChar);
                return relativePath;
            }
            return null;
        }
        #endregion
    }
}
