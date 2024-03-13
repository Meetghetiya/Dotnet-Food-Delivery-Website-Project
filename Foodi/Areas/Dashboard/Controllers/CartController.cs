using Foodi.Areas.Admin.Models;
using Foodi.BAL;
using Foodi.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Foodi.Areas.Dashboard.Controllers
{
    [CheckUserAccess]
    [Area("Dashboard")]
    [Route("Dashboard/[controller]/[action]")]
    public class CartController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly Cart_BalBase _cartbalbase;
        private readonly IToastNotification _toastNotification;
        private readonly Product_BalBase _productService;

        public CartController(IConfiguration configuration, IToastNotification toastNotification)
        {
            this.configuration = configuration;
            this._cartbalbase = new Cart_BalBase();
            _toastNotification = toastNotification;
            this._productService = new Product_BalBase();
        }
        public IActionResult Cart()
        {
            string userId = HttpContext.Session.GetString("UserId");
            int userID = int.Parse(userId);

            List<Cart> cart = _cartbalbase.FilterCart(userID);
            return View(cart);
        }

        [HttpGet]
        public IActionResult GetCartData()
        {
            string userId = HttpContext.Session.GetString("UserId");
            int userID = int.Parse(userId);

            List<Cart> cart = _cartbalbase.FilterCart(userID);
            return PartialView("_CartTablePartial", cart);
        }


        [HttpPost]
        public ActionResult EditCart(int cartId,int updatedQuantity)
        {
            
            

            Cart cart = _cartbalbase.GetByIdCart(cartId);
            Product product = _productService.GetProductById(cart.ProductId);
            int currentProductQuantity = product.Quantity;
            if (updatedQuantity <= 100)
            {
                if (cart != null)
                {
                    Cart cartupdated = new Cart
                    {
                        CartId = cart.CartId,
                        ProductId = cart.ProductId,
                        Quantity = updatedQuantity,
                        UserId = cart.UserId,
                    };
                    _cartbalbase.UpdateCart(cartupdated);
                    
                }
                else
                {
                    Console.WriteLine("null");
                }
                int GrandTotal = Grandtotal();
                return Json(new { success = true, updatedQuantity = updatedQuantity, GrandTotal = GrandTotal });
            }
            else
            {
                int GrandTotal = Grandtotal();
                _toastNotification.AddInfoToastMessage("Quantity out of stock");
                return Json(new { success = false, message = "Quantity out of stock" , GrandTotal = GrandTotal });
            }
        }

        [HttpPost]
        public ActionResult DeleteCart(int cartId)
        {
            _toastNotification.AddInfoToastMessage("Delete successfully");
            _cartbalbase.DeleteCart(cartId);
            return Json(new { success = true });
        }
        public int Grandtotal()
        {
            var GrandTotal = 0;
            string userId = HttpContext.Session.GetString("UserId");
            int userID = int.Parse(userId);
            List<Cart> cartlist = _cartbalbase.FilterCart(UserId: userID);
            foreach (Cart c in cartlist)
            {
                GrandTotal += c.Price * c.Quantity;
            }

            return GrandTotal;
        }
    }

    
}
