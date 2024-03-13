using Foodi.BAL;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Foodi.Areas.Admin.Models;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using System.Data;
using System.Data.SqlClient;
using Foodi.Models;

namespace Foodi.Areas.Admin.Controllers
{
    [CheckAdminAccess]
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class CartAdminController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly Cart_BalBase _cartbalbase;
        private readonly Product_BalBase _productService;
        private readonly IToastNotification _toastNotification;
        private readonly User_BalBase _userbalbase;

        public CartAdminController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IToastNotification toastNotification)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
            this._productService = new Product_BalBase();
            this._cartbalbase = new Cart_BalBase();
            _userbalbase = new User_BalBase();
            _toastNotification = toastNotification;
        }
        public IActionResult Cart()
        {
            List<Cart> carts = _cartbalbase.GetAllCarts();
            return View(carts);
        }

        [HttpGet]
        public IActionResult AddCart()
        {
            List<Product> products = _productService.GetAllProducts();
            ViewBag.Product = products ?? new List<Product>();
            List<User> Users = _userbalbase.GetAllUsers();
            ViewBag.User = Users ?? new List<User>();

            return View("AddEditCart");
        }

        [HttpPost]
        public ActionResult AddCart(Cart cart)
        {
            /* _toastNotification.AddInfoToastMessage("Product Add successfully");*/
            _cartbalbase.InsertCart(cart);
            return RedirectToAction("Cart");
        }

        public IActionResult EditCart(int CartId)
        {
            List<Product> products = _productService.GetAllProducts();
            ViewBag.Product = products ?? new List<Product>();
            List<User> Users = _userbalbase.GetAllUsers();
            ViewBag.User = Users ?? new List<User>();

            Cart cart = _cartbalbase.GetByIdCart(CartId);
            return View("AddEditCart", cart);
        }

        [HttpPost]
        public ActionResult EditCart(Cart cart)
        {
            _toastNotification.AddInfoToastMessage("Information successfully Update");
            _cartbalbase.UpdateCart(cart);
            return RedirectToAction("Cart");
        }

        public ActionResult DeleteCart(int CartId)
        {
            _toastNotification.AddInfoToastMessage("Delete successfully");
            _cartbalbase.DeleteCart(CartId);
            return RedirectToAction("Cart");
        }


        [HttpPost]
        public IActionResult DeleteMultipleCarts(List<int> cartIds)
        {
            // Validate if cartIds is not null and contains at least one item
            if (cartIds != null && cartIds.Count > 0)
            {
                // Implement your logic to delete multiple carts by their IDs
                foreach (var cartId in cartIds)
                {
                    _cartbalbase.DeleteCart(cartId);
                }

                return RedirectToAction("Cart");
            }

            return RedirectToAction("Cart");
        }
    }
}
